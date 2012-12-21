namespace YamlDataLoader
{
    partial class YamlDataLoader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._label1 = new System.Windows.Forms.Label();
            this._sourceDataFolderPath = new System.Windows.Forms.TextBox();
            this._browseSourceFolder = new System.Windows.Forms.Button();
            this._label2 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._databaseServerName = new System.Windows.Forms.TextBox();
            this._databaseName = new System.Windows.Forms.TextBox();
            this._label4 = new System.Windows.Forms.Label();
            this._label5 = new System.Windows.Forms.Label();
            this._sqlUserName = new System.Windows.Forms.TextBox();
            this._sqlPassword = new System.Windows.Forms.TextBox();
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._executeImport = new System.Windows.Forms.Button();
            this._sourceFolderBrowseDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(45, 43);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(73, 13);
            this._label1.TabIndex = 0;
            this._label1.Text = "Source Folder";
            // 
            // _sourceDataFolderPath
            // 
            this._sourceDataFolderPath.Location = new System.Drawing.Point(124, 40);
            this._sourceDataFolderPath.Name = "_sourceDataFolderPath";
            this._sourceDataFolderPath.Size = new System.Drawing.Size(279, 20);
            this._sourceDataFolderPath.TabIndex = 1;
            // 
            // _browseSourceFolder
            // 
            this._browseSourceFolder.Location = new System.Drawing.Point(409, 36);
            this._browseSourceFolder.Name = "_browseSourceFolder";
            this._browseSourceFolder.Size = new System.Drawing.Size(69, 26);
            this._browseSourceFolder.TabIndex = 2;
            this._browseSourceFolder.Text = "Browse";
            this._browseSourceFolder.UseVisualStyleBackColor = true;
            this._browseSourceFolder.Click += new System.EventHandler(this.OpenFolderDialog);
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.Location = new System.Drawing.Point(59, 36);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(38, 13);
            this._label2.TabIndex = 3;
            this._label2.Text = "Server";
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.Location = new System.Drawing.Point(39, 59);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(53, 13);
            this._label3.TabIndex = 4;
            this._label3.Text = "Database";
            // 
            // _databaseServerName
            // 
            this._databaseServerName.Location = new System.Drawing.Point(108, 33);
            this._databaseServerName.Name = "_databaseServerName";
            this._databaseServerName.Size = new System.Drawing.Size(279, 20);
            this._databaseServerName.TabIndex = 5;
            // 
            // _databaseName
            // 
            this._databaseName.Location = new System.Drawing.Point(108, 59);
            this._databaseName.Name = "_databaseName";
            this._databaseName.Size = new System.Drawing.Size(279, 20);
            this._databaseName.TabIndex = 6;
            // 
            // _label4
            // 
            this._label4.AutoSize = true;
            this._label4.Location = new System.Drawing.Point(30, 124);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(77, 13);
            this._label4.TabIndex = 7;
            this._label4.Text = "SQL username";
            // 
            // _label5
            // 
            this._label5.AutoSize = true;
            this._label5.Location = new System.Drawing.Point(26, 146);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(76, 13);
            this._label5.TabIndex = 8;
            this._label5.Text = "SQL password";
            // 
            // _sqlUserName
            // 
            this._sqlUserName.Location = new System.Drawing.Point(108, 121);
            this._sqlUserName.Name = "_sqlUserName";
            this._sqlUserName.Size = new System.Drawing.Size(279, 20);
            this._sqlUserName.TabIndex = 9;
            // 
            // _sqlPassword
            // 
            this._sqlPassword.Location = new System.Drawing.Point(108, 143);
            this._sqlPassword.Name = "_sqlPassword";
            this._sqlPassword.PasswordChar = '*';
            this._sqlPassword.Size = new System.Drawing.Size(279, 20);
            this._sqlPassword.TabIndex = 10;
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this._sqlPassword);
            this._groupBox1.Controls.Add(this._sqlUserName);
            this._groupBox1.Controls.Add(this._label5);
            this._groupBox1.Controls.Add(this._label4);
            this._groupBox1.Controls.Add(this._databaseName);
            this._groupBox1.Controls.Add(this._databaseServerName);
            this._groupBox1.Controls.Add(this._label3);
            this._groupBox1.Controls.Add(this._label2);
            this._groupBox1.Location = new System.Drawing.Point(16, 84);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(446, 212);
            this._groupBox1.TabIndex = 11;
            this._groupBox1.TabStop = false;
            this._groupBox1.Text = "Database Info";
            // 
            // _executeImport
            // 
            this._executeImport.Location = new System.Drawing.Point(144, 337);
            this._executeImport.Name = "_executeImport";
            this._executeImport.Size = new System.Drawing.Size(162, 32);
            this._executeImport.TabIndex = 12;
            this._executeImport.Text = "Execute Import";
            this._executeImport.UseVisualStyleBackColor = true;
            this._executeImport.Click += new System.EventHandler(this.OnExcuteImportClicked);
            // 
            // YamlDataLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 381);
            this.Controls.Add(this._executeImport);
            this.Controls.Add(this._groupBox1);
            this.Controls.Add(this._browseSourceFolder);
            this.Controls.Add(this._sourceDataFolderPath);
            this.Controls.Add(this._label1);
            this.Name = "YamlDataLoader";
            this.Text = "EveHQ Yaml -> SQL Data loading Utility";
            this._groupBox1.ResumeLayout(false);
            this._groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.TextBox _sourceDataFolderPath;
        private System.Windows.Forms.Button _browseSourceFolder;
        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.Label _label3;
        private System.Windows.Forms.TextBox _databaseServerName;
        private System.Windows.Forms.TextBox _databaseName;
        private System.Windows.Forms.Label _label4;
        private System.Windows.Forms.Label _label5;
        private System.Windows.Forms.TextBox _sqlUserName;
        private System.Windows.Forms.TextBox _sqlPassword;
        private System.Windows.Forms.GroupBox _groupBox1;
        private System.Windows.Forms.Button _executeImport;
        private System.Windows.Forms.FolderBrowserDialog _sourceFolderBrowseDialog;
    }
}

