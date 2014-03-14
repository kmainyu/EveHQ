//==============================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
// This file is part of EveHQ.
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// ------------------------------------------------------------------------------
// The MIT License (MIT)
// Copyright © 2005-2014  EveHQ Development Team
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// ==============================================================================
namespace EveHQ.NewSettingsConverter
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Data.SqlServerCe;
    using System.IO;
    using System.Windows.Forms;

    using EveHQ.Core;
    using EveHQ.Prism.Classes;

    /// <summary>
    ///     Converts the custom database in version 2.12.3 to an equivalent in SQLite.
    /// </summary>
    /// <remarks></remarks>
    public class DatabaseConverter
    {
        /// <summary>The _db database.</summary>
        private readonly string _dbDatabase = string.Empty;

        /// <summary>The _db file name.</summary>
        private readonly string _dbFileName = string.Empty;

        /// <summary>The _db format.</summary>
        private readonly DBFormat _dbFormat;

        /// <summary>The _db password.</summary>
        private readonly string _dbPassword = string.Empty;

        /// <summary>The _db sql sec.</summary>
        private readonly bool _dbSqlSec;

        /// <summary>The _db server name.</summary>
        private readonly string _dbServerName = string.Empty;

        /// <summary>The _db username.</summary>
        private readonly string _dbUsername = string.Empty;

        /// <summary>The _sq lite file.</summary>
        private readonly string _sqLiteFile;

        /// <summary>The _worker.</summary>
        private readonly BackgroundWorker _worker;

        /// <summary>The _sq lite conn.</summary>
        private string _sqLiteConn = string.Empty;

        /// <summary>The _sql conn.</summary>
        private string _sqlConn = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="DatabaseConverter"/> class. Create a new instance of the Database convert</summary>
        /// <param name="worker">The BackgroundWorker used to process the conversion.</param>
        /// <param name="settingsFolder">The location of the EveHQSettings.bin file (HQ.AppData folder).</param>
        /// <param name="dbFormat">The format of the database to convert.</param>
        /// <param name="dbFileName">The SQL Compact filename used.</param>
        /// <param name="dbServerName">The server instance</param>
        /// <param name="dbDatabase"></param>
        /// <param name="dbSqlSec"></param>
        /// <param name="dbUsername"></param>
        /// <param name="dbPassword"></param>
        /// <remarks></remarks>
        public DatabaseConverter(BackgroundWorker worker, string settingsFolder, DBFormat dbFormat, string dbFileName, string dbServerName, string dbDatabase, bool dbSqlSec, string dbUsername, string dbPassword)
        {
            _worker = worker;
            _dbFormat = dbFormat;
            _dbFileName = dbFileName;
            _dbServerName = dbServerName;
            _dbDatabase = dbDatabase;
            _dbSqlSec = dbSqlSec;
            _dbUsername = dbUsername;
            _dbPassword = dbPassword;

            _sqLiteFile = Path.Combine(settingsFolder, "EveHQData.db3");
        }

        /// <summary>The convert.</summary>
        public void Convert()
        {
            // Create some new settings
            HQ.Settings = new EveHQSettings();

            // Step 1 - Set the connection strings
            _worker.ReportProgress(0, "Database Conversion Step 1/15: Setting database connections...");
            SetOldConnectionString();
            SetNewConnectionString();

            // Step 2 - Check the database connection
            _worker.ReportProgress(0, "Database Conversion Step 2/15: Checking database connections...");
            if (CheckOldDatabaseConnection() == false)
            {
                _worker.ReportProgress(0, "Unable to connect to the v2 database!");
                MessageBox.Show("Unable to connect to the v2 database!", "Database connection error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 3 - Create the SQLite database
            _worker.ReportProgress(0, "Database Conversion Step 3/15: Creating SQLite database...");
            try
            {
                CustomDataFunctions.CreateCustomDB();
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error creating SQLite database: " + e.Message);
            }

            // Step 4 - Create the core database tables
            _worker.ReportProgress(0, "Database Conversion Step 4/15: Creating Core database tables...");
            try
            {
                CustomDataFunctions.CheckCoreDBTables();
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error creating Core database tables: " + e.Message);
            }

            // Step 5 - Convert the customPrices table
            _worker.ReportProgress(0, "Database Conversion Step 5/15: Converting Custom Prices database table...");
            ConvertCustomPricesTable();

            // Step 6 - Convert the marketPrices table
            _worker.ReportProgress(0, "Database Conversion Step 6/15: Converting Market Prices database table...");
            ConvertMarketPricesTable();

            // Step 7 - Convert the eveIDToName table
            _worker.ReportProgress(0, "Database Conversion Step 7/15: Converting ID To Name database table...");
            ConvertEveIdToNameTable();

            // Step 8 - Convert the eveMail table
            _worker.ReportProgress(0, "Database Conversion Step 8/15: Converting Eve Mail database table ...");
            ConvertEveMailTable();

            // Step 9 - Convert the eveNotifications table
            _worker.ReportProgress(0, "Database Conversion Step 9/15: Converting Eve Notifications database table...");
            ConvertEveNotificationsTable();

            // Step 10 - Convert the requisitions table
            _worker.ReportProgress(0, "Database Conversion Step 10/15: Converting Requisitions database table...");
            ConvertRequisitionsTable();

            // Step 11 - Create the Prism database tables
            _worker.ReportProgress(0, "Database Conversion Step 11/15: Creating Prism database tables...");
            try
            {
                PrismDataFunctions.CheckDatabaseTables();
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error creating Prism database tables: " + e.Message);
            }

            // Step 12 - Convert the assetItemNames tables
            _worker.ReportProgress(0, "Database Conversion Step 12/15: Converting Asset Item Names database table...");
            ConvertAssetItemNamesTable();

            // Step 13 - Convert the inventionResults tables
            _worker.ReportProgress(0, "Database Conversion Step 13/15: Converting Invention Results database table...");
            ConvertInventionResultsTable();

            // Step 14 - Convert the walletJournal tables
            _worker.ReportProgress(0, "Database Conversion Step 14/15: Converting Wallet Journal database table...");
            ConvertWalletJournalTable();

            // Step 15 - Convert the walletTransaction tables
            _worker.ReportProgress(0, "Database Conversion Step 15/15: Converting Wallet Transactions database table...");
            ConvertWalletTransactionsTable();

            // Report finished
            _worker.ReportProgress(0, "Database Conversion complete!");
        }

        /// <summary>The set old connection string.</summary>
        private void SetOldConnectionString()
        {
            switch (_dbFormat)
            {
                case DBFormat.Sqlce:

                    // SQL CE
                    _sqlConn = "Data Source = \""+ _dbFileName + "\";" + "Max Database Size = 512; ; Max Buffer Size = 2048;";
                    break;
                case DBFormat.Sql:

                    // SQL
                    _sqlConn = "Server=localhost\\" + _dbServerName + "; Database = " + _dbDatabase;
                    if (_dbSqlSec)
                    {
                        _sqlConn += "; User ID=" + _dbUsername + "; Password=" + _dbPassword + ";";
                    }
                    else
                    {
                        _sqlConn += "; Integrated Security = SSPI;";
                    }

                    break;
            }
        }

        /// <summary>The set new connection string.</summary>
        private void SetNewConnectionString()
        {
            _sqLiteConn = "Data Source=\"" + _sqLiteFile + "\";Version=3;";
            HQ.Settings.CustomDBFileName = _sqLiteFile;
            CustomDataFunctions.SetEveHQDataConnectionString();
        }

        /// <summary>The check old database connection.</summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckOldDatabaseConnection()
        {
            
            switch (_dbFormat)
            {
                case DBFormat.Sqlce:
                    var ceConnection = new SqlCeConnection(_sqlConn);
                    try
                    {
                        ceConnection.Open();
                        ceConnection.Close();
                        return true;
                    }
                    catch 
                    {
                        return false;
                    }

                case DBFormat.Sql:
                    var connection = new SqlConnection(_sqlConn);
                    try
                    {
                        connection.Open();
                        connection.Close();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>The get data.</summary>
        /// <param name="strSql">The str sql.</param>
        /// <param name="format">The format.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="DataSet"/>.</returns>
        public DataSet GetData(string strSql, DBFormat format, string connectionString)
        {
            var evehqData = new DataSet();

            switch (format)
            {
                case DBFormat.Sqlce:

                    // SQL CE
                    var ceConn = new SqlCeConnection();
                    ceConn.ConnectionString = connectionString;
                    try
                    {
                        ceConn.Open();
                        var da = new SqlCeDataAdapter(strSql, ceConn);
                        da.Fill(evehqData, "EveHQData");
                        ceConn.Close();
                        return evehqData;
                    }
                    catch (Exception e)
                    {
                        // MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        _worker.ReportProgress(0, "Error in SQLCE GetData: " + e.Message);
                        return null;
                    }
                    finally
                    {
                        if (ceConn.State == ConnectionState.Open)
                        {
                            ceConn.Close();
                        }
                    }

                case DBFormat.Sql:

                    // MSSQL
                    var conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    try
                    {
                        conn.Open();
                        var da = new SqlDataAdapter(strSql, conn);
                        da.Fill(evehqData, "EveHQData");
                        conn.Close();
                        return evehqData;
                    }
                    catch (Exception e)
                    {
                        // MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        _worker.ReportProgress(0, "Error in SQL GetData: " + e.Message);
                        return null;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                default:

                    // MessageBox.Show("Invalid database format!", "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _worker.ReportProgress(0, "Error in Database format!");
                    return null;
            }
        }

        /// <summary>The convert custom prices table.</summary>
        private void ConvertCustomPricesTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from customPrices;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO customPrices (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting custom prices table: " + e.Message);
            }
        }

        /// <summary>The convert market prices table.</summary>
        private void ConvertMarketPricesTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from marketPrices;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO marketPrices (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting market prices table: " + e.Message);
            }
        }

        /// <summary>The convert eve id to name table.</summary>
        private void ConvertEveIdToNameTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from eveIDToName;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO eveIDToName (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting EveID to Name table: " + e.Message);
            }
        }

        /// <summary>The convert eve mail table.</summary>
        private void ConvertEveMailTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from eveMail;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO eveMail (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting Evemail table: " + e.Message);
            }
        }

        /// <summary>The convert eve notifications table.</summary>
        private void ConvertEveNotificationsTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from eveNotifications;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO eveNotifications (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting notifications table: " + e.Message);
            }
        }

        /// <summary>The convert requisitions table.</summary>
        private void ConvertRequisitionsTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from requisitions;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO requisitions (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting requisitions table: " + e.Message);
            }
        }

        /// <summary>The convert asset item names table.</summary>
        private void ConvertAssetItemNamesTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from assetItemNames;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO assetItemNames (itemID, itemName";
                                cmd.CommandText += ") VALUES(?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                for (int col = 1; col <= 2; col++)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    cmd.Parameters[0].Value = row["itemID"];
                                    cmd.Parameters[1].Value = row["itemName"];
                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting asset item names table: " + e.Message);
            }
        }

        /// <summary>The convert invention results table.</summary>
        private void ConvertInventionResultsTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from inventionResults;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO inventionResults (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting invention results table: " + e.Message);
            }
        }

        /// <summary>The convert wallet journal table.</summary>
        private void ConvertWalletJournalTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from walletJournal;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO walletJournal (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting wallet journal table: " + e.Message);
            }
        }

        /// <summary>The convert wallet transactions table.</summary>
        private void ConvertWalletTransactionsTable()
        {
            try
            {
                // Step 1 - Load all the v2 data
                using (DataSet eveData = GetData("SELECT * from walletTransactions;", _dbFormat, _sqlConn))
                {
                    if (eveData != null)
                    {
                        // Step 2 - Put all the v2 data into the v3 table
                        var conn = new SQLiteConnection(_sqLiteConn);
                        conn.Open();
                        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                        {
                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                // Create the DB Command
                                cmd.CommandText = "INSERT INTO walletTransactions (";

                                // Add the columns
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    cmd.CommandText += col.ColumnName + ", ";
                                }

                                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2);
                                cmd.CommandText += ") VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                // Create the desired number of parameters
                                // ReSharper disable once RedundantAssignment - incorrect R# warning
                                foreach (DataColumn col in eveData.Tables[0].Columns)
                                {
                                    SQLiteParameter field = cmd.CreateParameter();
                                    cmd.Parameters.Add(field);
                                }

                                // Add the values
                                foreach (DataRow row in eveData.Tables[0].Rows)
                                {
                                    for (int col = 0; col <= eveData.Tables[0].Columns.Count - 1; col++)
                                    {
                                        cmd.Parameters[col].Value = row[col];
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                dbTrans.Commit();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting wallet transactions table: " + e.Message);
            }
        }
    }
}