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
            _label1 = new System.Windows.Forms.Label();
            _sourceDataFolderPath = new System.Windows.Forms.TextBox();
            _browseSourceFolder = new System.Windows.Forms.Button();
            _label2 = new System.Windows.Forms.Label();
            _label3 = new System.Windows.Forms.Label();
            _databaseServerName = new System.Windows.Forms.TextBox();
            _databaseName = new System.Windows.Forms.TextBox();
            _label4 = new System.Windows.Forms.Label();
            _label5 = new System.Windows.Forms.Label();
            _sqlUserName = new System.Windows.Forms.TextBox();
            _sqlPassword = new System.Windows.Forms.TextBox();
            _groupBox1 = new System.Windows.Forms.GroupBox();
            _executeImport = new System.Windows.Forms.Button();
            _sourceFolderBrowseDialog = new System.Windows.Forms.FolderBrowserDialog();
            _groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _label1
            // 
            _label1.AutoSize = true;
            _label1.Location = new System.Drawing.Point(45, 43);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(73, 13);
            _label1.TabIndex = 0;
            _label1.Text = "Source Folder";
            // 
            // _sourceDataFolderPath
            // 
            _sourceDataFolderPath.Location = new System.Drawing.Point(124, 40);
            _sourceDataFolderPath.Name = "_sourceDataFolderPath";
            _sourceDataFolderPath.Size = new System.Drawing.Size(279, 20);
            _sourceDataFolderPath.TabIndex = 1;
            // 
            // _browseSourceFolder
            // 
            _browseSourceFolder.Location = new System.Drawing.Point(409, 36);
            _browseSourceFolder.Name = "_browseSourceFolder";
            _browseSourceFolder.Size = new System.Drawing.Size(69, 26);
            _browseSourceFolder.TabIndex = 2;
            _browseSourceFolder.Text = "Browse";
            _browseSourceFolder.UseVisualStyleBackColor = true;
            _browseSourceFolder.Click += new System.EventHandler(this.OpenFolderDialog);
            // 
            // _label2
            // 
            _label2.AutoSize = true;
            _label2.Location = new System.Drawing.Point(59, 36);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(38, 13);
            _label2.TabIndex = 3;
            _label2.Text = "Server";
            // 
            // _label3
            // 
            _label3.AutoSize = true;
            _label3.Location = new System.Drawing.Point(39, 59);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(53, 13);
            _label3.TabIndex = 4;
            _label3.Text = "Database";
            // 
            // _databaseServerName
            // 
            _databaseServerName.Location = new System.Drawing.Point(108, 33);
            _databaseServerName.Name = "_databaseServerName";
            _databaseServerName.Size = new System.Drawing.Size(279, 20);
            _databaseServerName.TabIndex = 5;
            // 
            // _databaseName
            // 
            _databaseName.Location = new System.Drawing.Point(108, 59);
            _databaseName.Name = "_databaseName";
            _databaseName.Size = new System.Drawing.Size(279, 20);
            _databaseName.TabIndex = 6;
            // 
            // _label4
            // 
            _label4.AutoSize = true;
            _label4.Location = new System.Drawing.Point(30, 124);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(77, 13);
            _label4.TabIndex = 7;
            _label4.Text = "SQL username";
            // 
            // _label5
            // 
            _label5.AutoSize = true;
            _label5.Location = new System.Drawing.Point(26, 146);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(76, 13);
            _label5.TabIndex = 8;
            _label5.Text = "SQL password";
            // 
            // _sqlUserName
            // 
            _sqlUserName.Location = new System.Drawing.Point(108, 121);
            _sqlUserName.Name = "_sqlUserName";
            _sqlUserName.Size = new System.Drawing.Size(279, 20);
            _sqlUserName.TabIndex = 9;
            // 
            // _sqlPassword
            // 
            _sqlPassword.Location = new System.Drawing.Point(108, 143);
            _sqlPassword.Name = "_sqlPassword";
            _sqlPassword.PasswordChar = '*';
            _sqlPassword.Size = new System.Drawing.Size(279, 20);
            _sqlPassword.TabIndex = 10;
            // 
            // _groupBox1
            // 
            _groupBox1.Controls.Add(_sqlPassword);
            _groupBox1.Controls.Add(_sqlUserName);
            _groupBox1.Controls.Add(_label5);
            _groupBox1.Controls.Add(_label4);
            _groupBox1.Controls.Add(_databaseName);
            _groupBox1.Controls.Add(_databaseServerName);
            _groupBox1.Controls.Add(_label3);
            _groupBox1.Controls.Add(_label2);
            _groupBox1.Location = new System.Drawing.Point(16, 84);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(446, 212);
            _groupBox1.TabIndex = 11;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "Database Info";
            // 
            // _executeImport
            // 
            _executeImport.Location = new System.Drawing.Point(144, 337);
            _executeImport.Name = "_executeImport";
            _executeImport.Size = new System.Drawing.Size(162, 32);
            _executeImport.TabIndex = 12;
            _executeImport.Text = "Execute Import";
            _executeImport.UseVisualStyleBackColor = true;
            _executeImport.Click += new System.EventHandler(this.OnExcuteImportClicked);
            // 
            // YamlDataLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 381);
            this.Controls.Add(_executeImport);
            this.Controls.Add(_groupBox1);
            this.Controls.Add(_browseSourceFolder);
            this.Controls.Add(_sourceDataFolderPath);
            this.Controls.Add(_label1);
            this.Name = "YamlDataLoader";
            this.Text = "EveHQ Yaml -> SQL Data loading Utility";
            _groupBox1.ResumeLayout(false);
            _groupBox1.PerformLayout();
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

