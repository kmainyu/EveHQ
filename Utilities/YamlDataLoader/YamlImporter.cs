// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (YamlImporter.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================
namespace YamlDataLoader
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using YamlDotNet.RepresentationModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal static class YamlImporter
    {
        /// <summary>The connection string format.</summary>
        private const string ConnectionStringFormat = "Server={0};Database={1};User Id={2};Password={3};MultipleActiveResultSets=true;";

        /// <summary>The create icon table command.</summary>
        private const string CreateIconTableCommand =
            "CREATE TABLE [dbo].[eveIcons]([iconID] [int] NOT NULL,[iconFile] [varchar](500) NOT NULL,[description] [nvarchar](max) NOT NULL,CONSTRAINT [eveIcons_PK] PRIMARY KEY CLUSTERED ([iconID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]";

        private const string AlterTypeTableCommand = "Alter table dbo.invTypes add iconID int Null";

        private const string UpdateTypeTableCommand = "Update dbo.invTypes set iconID={1} where typeID={0}";

        /// <summary>The insert icon format.</summary>
        private const string InsertIconFormat = "insert into dbo.eveIcons values ({0},'{1}','{2}')";

        /// <summary>The execute import.</summary>
        /// <param name="sourcefolderPath">The source folder path.</param>
        /// <param name="databaseServer">The datbase server.</param>
        /// <param name="databaseName">The database name.</param>
        /// <param name="databaseUser">The database user.</param>
        /// <param name="databasePassword">The database password.</param>
        public static void ExecuteImport(string sourcefolderPath, string databaseServer, string databaseName, string databaseUser, string databasePassword)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(sourcefolderPath));
            Contract.Requires(!string.IsNullOrWhiteSpace(databaseServer));
            Contract.Requires(!string.IsNullOrWhiteSpace(databaseName));
            Contract.Requires(!string.IsNullOrWhiteSpace(databaseUser));
            Contract.Requires(!string.IsNullOrWhiteSpace(databasePassword));

            Task iconTask = null;
            Task typeTask = null;

            // validate the folder is actually there
            if (!Directory.Exists(sourcefolderPath))
            {
                MessageBox.Show("A non existant source folder was specified. Please correct this and try again.");
            }

            // enumerate the source folder to make sure there are acutally files in it
            IEnumerable<string> files = Directory.EnumerateFiles(sourcefolderPath);
            foreach (string file in files)
            {
                switch (Path.GetFileName(file))
                {
                    case "iconIDs.yaml":
                        string iconfile = file; // copy the string immediately, so the enumerated value isn't accessed in the defferred executed lambda.
                        iconTask = Task.Factory.StartNew(() => ProcessIconData(iconfile, databaseServer, databaseName, databaseUser, databasePassword));
                        break;
                    case "typeIDs.yaml":
                        string typeFile = file;
                        typeTask = Task.Factory.StartNew(() => ProcessTypeIds(typeFile, databaseServer, databaseName, databaseUser, databasePassword));

                        break;
                }
            }

            Task.WaitAll(iconTask, typeTask);


        }

        private static void ProcessTypeIds(string file, string databaseServer, string databaseName, string databaseUser, string databasePassword)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var textReader = new StreamReader(fs))
            using (var dbcon = new SqlConnection(string.Format(ConnectionStringFormat, databaseServer, databaseName, databaseUser, databasePassword)))
            {
                try
                {
                    dbcon.Open();
                    // alter the invTypes table
                    SqlCommand cmd = new SqlCommand(AlterTypeTableCommand, dbcon);
                    cmd.ExecuteNonQuery();

                    var yaml = new YamlStream();
                    yaml.Load(textReader);
                    var root = yaml.Documents[0].RootNode as YamlMappingNode;
                    foreach (KeyValuePair<YamlNode, YamlNode> item in root.Children)
                    {
                        int typeId, iconId;
                        int temp;
                        if (int.TryParse(item.Key.ToString(), out temp))
                        {
                            typeId = temp;

                            // parse the children nodes looking for an icon id (if assigned)
                            YamlMappingNode data;
                            if ((data = item.Value as YamlMappingNode) != null)
                            {
                                foreach (KeyValuePair<YamlNode, YamlNode> node in data.Children)
                                {
                                    if (node.Key.ToString() == "iconID" && int.TryParse(node.Value.ToString(), out temp))
                                    {
                                        iconId = temp;

                                        // commit to database
                                        cmd = new SqlCommand(string.Format(UpdateTypeTableCommand, typeId, iconId), dbcon);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    e = e;
                }
                finally
                {
                    dbcon.Close();
                }
            }
        }

        /// <summary>imports the icon YAML file into a new SQL table, as it was removed from the data export database file.</summary>
        /// <param name="file">file name to import</param>
        /// <param name="databaseServer">The database Server.</param>
        /// <param name="databaseName">The database Name.</param>
        /// <param name="databaseUser">The database User.</param>
        /// <param name="databasePassword">The database Password.</param>
        private static void ProcessIconData(string file, string databaseServer, string databaseName, string databaseUser, string databasePassword)
        {
            var icons = new List<EveIcon>();

            // first read the file to make sure its valid yaml
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var textReader = new StreamReader(fs))
            {
                var yaml = new YamlStream();
                yaml.Load(textReader);

                var root = yaml.Documents[0].RootNode as YamlMappingNode;
                foreach (KeyValuePair<YamlNode, YamlNode> item in root.Children)
                {
                    // convert each of the records in the yaml into objects
                    var icon = new EveIcon();
                    int temp;
                    if (int.TryParse(item.Key.ToString(), out temp))
                    {
                        icon.IconId = temp;

                        // parse the children nodes
                        YamlMappingNode data;
                        if ((data = item.Value as YamlMappingNode) != null)
                        {
                            foreach (KeyValuePair<YamlNode, YamlNode> node in data.Children)
                            {
                                switch (node.Key.ToString())
                                {
                                    case "description":
                                        icon.Description = node.Value.ToString();
                                        break;
                                    case "iconFile":
                                        icon.IconFile = node.Value.ToString();
                                        break;
                                }
                            }

                            icons.Add(icon);
                        }
                    }
                }
            }

            // icons have been parsed into objects... create the final SQL table and submit the objects.
            using (var dbcon = new SqlConnection(string.Format(ConnectionStringFormat, databaseServer, databaseName, databaseUser, databasePassword)))
            {
                try
                {
                    dbcon.Open();

                    // initialize the table
                    var cmd = new SqlCommand(CreateIconTableCommand, dbcon);
                    cmd.ExecuteNonQuery();
                    foreach (EveIcon icon in icons)
                    {
                        cmd = new SqlCommand(string.Format(InsertIconFormat, icon.IconId, icon.IconFile, string.IsNullOrWhiteSpace(icon.Description) ? null : icon.Description.Replace("\'", "\'\'")), dbcon);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    dbcon.Close();
                }
            }
        }
    }
}