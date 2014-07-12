
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace EveHQ.NewCacheCreator
{
    partial class FrmCacheCreator : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCacheCreator));
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.btnGenerateCache = new System.Windows.Forms.Button();
            this.btnCheckDB = new System.Windows.Forms.Button();
            this.lblDB = new System.Windows.Forms.Label();
            this.btnCheckMarketGroup = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.gbCheckingTools = new System.Windows.Forms.GroupBox();
            this.gbEveHQCacheGeneration = new System.Windows.Forms.GroupBox();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbInstructions = new System.Windows.Forms.GroupBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.EveHQDatabaseName = new System.Windows.Forms.TextBox();
            this.gbCheckingTools.SuspendLayout();
            this.gbEveHQCacheGeneration.SuspendLayout();
            this.gbInstructions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(106, 12);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(221, 20);
            this.txtServerName.TabIndex = 2;
            this.txtServerName.Text = "localhost\\SQL2008E";
            // 
            // btnGenerateCache
            // 
            this.btnGenerateCache.Location = new System.Drawing.Point(9, 121);
            this.btnGenerateCache.Name = "btnGenerateCache";
            this.btnGenerateCache.Size = new System.Drawing.Size(299, 23);
            this.btnGenerateCache.TabIndex = 5;
            this.btnGenerateCache.Text = "Generate All Cache Files";
            this.btnGenerateCache.UseVisualStyleBackColor = true;
            // 
            // btnCheckDB
            // 
            this.btnCheckDB.Location = new System.Drawing.Point(9, 94);
            this.btnCheckDB.Name = "btnCheckDB";
            this.btnCheckDB.Size = new System.Drawing.Size(299, 23);
            this.btnCheckDB.TabIndex = 6;
            this.btnCheckDB.Text = "Check SQL Database";
            this.btnCheckDB.UseVisualStyleBackColor = true;
            // 
            // lblDB
            // 
            this.lblDB.AutoSize = true;
            this.lblDB.Location = new System.Drawing.Point(6, 50);
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(56, 13);
            this.lblDB.TabIndex = 7;
            this.lblDB.Text = "Database:";
            // 
            // btnCheckMarketGroup
            // 
            this.btnCheckMarketGroup.Location = new System.Drawing.Point(6, 19);
            this.btnCheckMarketGroup.Name = "btnCheckMarketGroup";
            this.btnCheckMarketGroup.Size = new System.Drawing.Size(287, 23);
            this.btnCheckMarketGroup.TabIndex = 9;
            this.btnCheckMarketGroup.Text = "Check Market Groups";
            this.btnCheckMarketGroup.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(6, 19);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(302, 31);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Before starting this, ensure the typeID and iconID YAML files are in the resource" +
    "s folder so the database can be updated.";
            // 
            // gbCheckingTools
            // 
            this.gbCheckingTools.Controls.Add(this.btnCheckMarketGroup);
            this.gbCheckingTools.Location = new System.Drawing.Point(9, 149);
            this.gbCheckingTools.Name = "gbCheckingTools";
            this.gbCheckingTools.Size = new System.Drawing.Size(299, 58);
            this.gbCheckingTools.TabIndex = 11;
            this.gbCheckingTools.TabStop = false;
            this.gbCheckingTools.Text = "Checking Tools";
            // 
            // gbEveHQCacheGeneration
            // 
            this.gbEveHQCacheGeneration.Controls.Add(this.EveHQDatabaseName);
            this.gbEveHQCacheGeneration.Controls.Add(this.lblInfo);
            this.gbEveHQCacheGeneration.Controls.Add(this.gbCheckingTools);
            this.gbEveHQCacheGeneration.Controls.Add(this.btnGenerateCache);
            this.gbEveHQCacheGeneration.Controls.Add(this.btnCheckDB);
            this.gbEveHQCacheGeneration.Controls.Add(this.lblDB);
            this.gbEveHQCacheGeneration.Location = new System.Drawing.Point(12, 12);
            this.gbEveHQCacheGeneration.Name = "gbEveHQCacheGeneration";
            this.gbEveHQCacheGeneration.Size = new System.Drawing.Size(318, 213);
            this.gbEveHQCacheGeneration.TabIndex = 12;
            this.gbEveHQCacheGeneration.TabStop = false;
            this.gbEveHQCacheGeneration.Text = "EveHQ Cache Generation";
            // 
            // gbInstructions
            // 
            this.gbInstructions.Controls.Add(this.lblInstructions);
            this.gbInstructions.Location = new System.Drawing.Point(336, 12);
            this.gbInstructions.Name = "gbInstructions";
            this.gbInstructions.Size = new System.Drawing.Size(257, 213);
            this.gbInstructions.TabIndex = 14;
            this.gbInstructions.TabStop = false;
            this.gbInstructions.Text = "Instructions!";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInstructions.Location = new System.Drawing.Point(7, 20);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(244, 185);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // EveHQDatabaseName
            // 
            this.EveHQDatabaseName.Location = new System.Drawing.Point(9, 66);
            this.EveHQDatabaseName.Name = "EveHQDatabaseName";
            this.EveHQDatabaseName.Size = new System.Drawing.Size(299, 20);
            this.EveHQDatabaseName.TabIndex = 12;
            this.EveHQDatabaseName.Text = "EveHQMaster";
            // 
            // FrmCacheCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 232);
            this.Controls.Add(this.gbInstructions);
            this.Controls.Add(this.gbEveHQCacheGeneration);
            this.Name = "FrmCacheCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EveHQ Cache Creator";
            this.gbCheckingTools.ResumeLayout(false);
            this.gbEveHQCacheGeneration.ResumeLayout(false);
            this.gbEveHQCacheGeneration.PerformLayout();
            this.gbInstructions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        internal System.Windows.Forms.TextBox txtServerName;

        internal System.Windows.Forms.Button btnGenerateCache;

        internal System.Windows.Forms.Button btnCheckDB;

        internal System.Windows.Forms.Label lblDB;

        internal System.Windows.Forms.Button btnCheckMarketGroup;

        internal System.Windows.Forms.Label lblInfo;

        internal System.Windows.Forms.GroupBox gbCheckingTools;

        internal System.Windows.Forms.GroupBox gbEveHQCacheGeneration;

        internal System.Windows.Forms.ToolTip ToolTip1;

        internal System.Windows.Forms.GroupBox gbInstructions;

        internal System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.TextBox EveHQDatabaseName;
    }

}