// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (YamlDataLoader.cs), is part of EveHQ.
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
    using System.Windows.Forms;

    /// <summary>The YAML data loader.</summary>
    public partial class YamlDataLoader : Form
    {
        /// <summary>Initializes a new instance of the <see cref="YamlDataLoader"/> class.</summary>
        public YamlDataLoader()
        {
            this.InitializeComponent();
        }

        /// <summary>An event handler that will open the browsing dialog for the source folder</summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event Data payload.</param>
        private void OpenFolderDialog(object sender, EventArgs e)
        {
            _sourceFolderBrowseDialog.ShowDialog();

            _sourceDataFolderPath.Text = _sourceFolderBrowseDialog.SelectedPath;
        }


        /// <summary>
        /// Handler for the click event on the ExecuteImport button.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        /// <param name="e">Event payload</param>
        private void OnExcuteImportClicked(object sender, EventArgs e)
        {
            // validate there is enough information to proceed.
            if (string.IsNullOrWhiteSpace(_sourceDataFolderPath.Text) || string.IsNullOrWhiteSpace(_databaseServerName.Text) || string.IsNullOrWhiteSpace(_databaseName.Text)
                || string.IsNullOrWhiteSpace(_sqlUserName.Text) || string.IsNullOrWhiteSpace(_sqlPassword.Text))
            {
                const string ErrorMsg = "There is missing information and the inport cannot proceed. Please ensure all the fields are filled out";
                MessageBox.Show(ErrorMsg);
            }
            YamlImporter.ExecuteImport(_sourceDataFolderPath.Text, _databaseServerName.Text, _databaseName.Text, _sqlUserName.Text, _sqlPassword.Text);
        }

       
    }
}