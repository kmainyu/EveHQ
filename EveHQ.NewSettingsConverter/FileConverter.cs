// ===========================================================================
// <copyright file="FileConverter.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (FileConverter.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================

// ==============================================================================
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Windows.Forms;
using System.Xml;

using EveHQ.Common.Extensions;
using EveHQ.Core;
using EveHQ.EveData;
using EveHQ.HQF;
using EveHQ.Prism;
using EveHQ.Prism.BPCalc;
using EveHQ.Prism.Classes;

using Microsoft.VisualBasic;

using Newtonsoft.Json;

namespace EveHQ.NewSettingsConverter
{
    /// <summary>
    ///     Converts the old settings format into the new one
    /// </summary>
    /// <remarks></remarks>
    public class FileConverter
    {
        /// <summary>The _settings folder.</summary>
        private readonly string _settingsFolder;

        /// <summary>The _worker.</summary>
        private readonly BackgroundWorker _worker;

        /// <summary>The _new settings.</summary>
        private EveHQSettings _newSettings;

        /// <summary>Initializes a new instance of the <see cref="FileConverter"/> class.</summary>
        /// <param name="worker">The worker.</param>
        /// <param name="settingsFolder">The settings folder.</param>
        public FileConverter(BackgroundWorker worker, string settingsFolder)
        {
            _worker = worker;
            _settingsFolder = settingsFolder;
            _newSettings = new EveHQSettings();
        }

        #region "Public Methods"

        /// <summary>The convert.</summary>
        public void Convert()
        {
            // Convert the Core settings
            ConvertCoreSettings(_settingsFolder);

            // Convert the Prism settings
            ConvertPrismSettings(_settingsFolder);

            // Convert the HQF settings
            ConvertHQFSettings(_settingsFolder);
        }

        #endregion

        #region "Core Data/Settings Conversion Methods"

        /// <summary>The convert core settings.</summary>
        /// <param name="settingsFolder">The settings folder.</param>
        private void ConvertCoreSettings(string settingsFolder)
        {
            var oldSettings = new EveSettings();
            _newSettings = new EveHQSettings();

            // Load in the old EveHQ Settings
            _worker.ReportProgress(0, "Attempting to load original EveHQ Core Settings file...");
            if (File.Exists(Path.Combine(settingsFolder, "EveHQSettings.bin")))
            {
                try
                {
                    using (var s = new FileStream(Path.Combine(settingsFolder, "EveHQSettings.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldSettings = (EveSettings)f.Deserialize(s);
                    }
                }
                catch (Exception e)
                {
                    _worker.ReportProgress(0, "Error loading EveHQ Settings file: " + e.Message);
                }

                _worker.ReportProgress(0, "Core Settings Conversion Step 1/8: Loading training queues...");
                LoadTraining(oldSettings, Path.Combine(settingsFolder, "Data"));

                _worker.ReportProgress(0, "Core Settings Conversion Step 2/8: Converting main settings...");
                ConvertMainSettings(oldSettings);

                _worker.ReportProgress(0, "Core Settings Conversion Step 3/8: Converting API accounts...");
                ConvertAccounts(oldSettings);

                _worker.ReportProgress(0, "Core Settings Conversion Step 4/8: Converting characters...");
                ConvertPilots(oldSettings);

                _worker.ReportProgress(0, "Core Settings Conversion Step 5/8: Converting plug-in settings...");
                ConvertPlugins(oldSettings);

                _worker.ReportProgress(0, "Core Settings Conversion Step 6/8: Converting dashboard configuration...");
                ConvertDashboard(oldSettings);

                // Dim startTime, endTime As DateTime
                // Dim timeTaken As TimeSpan

                // startTime = Now
                string json = JsonConvert.SerializeObject(_newSettings, Newtonsoft.Json.Formatting.Indented);

                // endTime = Now
                // timeTaken = (endTime - startTime)
                _worker.ReportProgress(0, "Core Settings Conversion Step 7/8: Saving new settings file...");

                // Write a JSON version of the settings
                try
                {
                    using (var s = new StreamWriter(Path.Combine(settingsFolder, "EveHQSettings.json"), false))
                    {
                        s.Write(json);
                        s.Flush();
                    }
                }
                catch (Exception e)
                {
                    _worker.ReportProgress(0, "Error writing new settings file: " + e.Message);
                }

                // Rename the old settings file
                _worker.ReportProgress(0, "Core Settings Conversion Step 8/8: Archiving old settings...");
                try
                {
                    File.Move(Path.Combine(settingsFolder, "EveHQSettings.bin"), Path.Combine(settingsFolder, "OldEveHQSettings.bin"));
                }
                catch (Exception e)
                {
                    _worker.ReportProgress(0, "Error archiving old settings file: " + e.Message);
                }

                // MessageBox.Show("Successfully converted settings in " & timeTaken.TotalMilliseconds.ToString("N2") & "ms", "Settings conversion complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            }
        }

        /// <summary>The load training.</summary>
        /// <param name="oldSettings">The old settings.</param>
        /// <param name="skillXMLFolder">The skill xml folder.</param>
        private void LoadTraining(EveSettings oldSettings, string skillXMLFolder)
        {
            Pilot currentPilot = default(Pilot);
            var xmlDoc = new XmlDocument();
            string tFileName = null;

            XmlNodeList trainingList = default(XmlNodeList);
            XmlNodeList queueList = default(XmlNodeList);
            XmlNode trainingDetails = default(XmlNode);

            string[] obsoleteSkills = { "Analytical Mind", "Clarity", "Eidetic Memory", "Empathy", "Focus", "Instant Recall", "Iron Will", "Learning", "Logic", "Presence", "Spatial Awareness" };
            var obsoleteList = new List<string>(obsoleteSkills);

            foreach (object pilot in oldSettings.Pilots)
            {
                currentPilot = pilot as Pilot;
                currentPilot.ActiveQueue = new SkillQueue();

                // currentPilot.ActiveQueue.Queue.Clear()
                currentPilot.TrainingQueues = new SortedList();
                currentPilot.TrainingQueues.Clear();
                currentPilot.PrimaryQueue = string.Empty;

                tFileName = "Q_" + currentPilot.Name + ".xml";
                if (File.Exists(Path.Combine(skillXMLFolder, tFileName)))
                {
                    try
                    {
                        xmlDoc.Load(Path.Combine(skillXMLFolder, tFileName));

                        // Get the pilot details
                        trainingList = xmlDoc.SelectNodes("/training/skill");

                        if (trainingList.Count > 0)
                        {
                            // Using version prior to 1.3
                            // Start a new SkillQueue class (using "primary" as the default name)
                            var newQ = new SkillQueue();
                            newQ.Name = "Primary";
                            newQ.IncCurrentTraining = true;
                            newQ.Primary = true;
                            foreach (object details in trainingList)
                            {
                                trainingDetails = details as XmlNode;
                                var myskill = new SkillQueueItem();
                                myskill.Name = trainingDetails.ChildNodes[0].InnerText;
                                myskill.FromLevel = trainingDetails.ChildNodes[1].InnerText.ToInt32();
                                myskill.ToLevel = trainingDetails.ChildNodes[2].InnerText.ToInt32();
                                myskill.Pos = trainingDetails.ChildNodes[3].InnerText.ToInt32();
                                string keyName = myskill.Name + myskill.FromLevel + myskill.ToLevel;
                                currentPilot.ActiveQueue.Queue.Add(myskill, keyName);
                            }

                            newQ.Queue = currentPilot.ActiveQueue.Queue;
                            currentPilot.PrimaryQueue = newQ.Name;
                            currentPilot.TrainingQueues.Add(newQ.Name, newQ);
                        }
                        else
                        {
                            // Try for the post 1.3 version
                            // Get version
                            XmlNode rootNode = xmlDoc.SelectSingleNode("/training");
                            double version = 0;
                            var culture = new CultureInfo("en-GB");
                            if (rootNode.Attributes.Count > 0)
                            {
                                version = double.Parse(rootNode.Attributes["version"].Value, NumberStyles.Any, culture);
                            }

                            queueList = xmlDoc.SelectNodes("/training/queue");
                            if (queueList.Count > 0)
                            {
                                foreach (XmlNode queueDetails in queueList)
                                {
                                    var newQ = new SkillQueue();
                                    newQ.Name = HttpUtility.HtmlDecode(queueDetails.Attributes["name"].Value);
                                    newQ.IncCurrentTraining = queueDetails.Attributes["ICT"].Value.ToBoolean();
                                    newQ.Primary = queueDetails.Attributes["primary"].Value.ToBoolean();
                                    if (newQ.Primary)
                                    {
                                        if (!string.IsNullOrEmpty(currentPilot.PrimaryQueue))
                                        {
                                            // There can be only one!
                                            newQ.Primary = false;
                                        }
                                        else
                                        {
                                            currentPilot.PrimaryQueue = newQ.Name;
                                        }
                                    }

                                    if (queueDetails.ChildNodes.Count > 0)
                                    {
                                        // Using version prior to 1.3
                                        // Start a new SkillQueue class (using "primary" as the default name)
                                        foreach (object details in queueDetails.ChildNodes)
                                        {
                                            trainingDetails = details as XmlNode;
                                            if (obsoleteList.Contains(trainingDetails.ChildNodes[0].InnerText) == false)
                                            {
                                                var myskill = new SkillQueueItem();
                                                myskill.Name = trainingDetails.ChildNodes[0].InnerText;

                                                // Adjust for the 1.9 version
                                                if (version < 1.9)
                                                {
                                                    if (myskill.Name == "Astrometric Triangulation")
                                                    {
                                                        myskill.Name = "Astrometric Acquisition";
                                                    }

                                                    if (myskill.Name == "Signal Acquisition")
                                                    {
                                                        myskill.Name = "Astrometric Triangulation";
                                                    }
                                                }

                                                try
                                                {
                                                    myskill.FromLevel = trainingDetails.ChildNodes[1].InnerText.ToInt32();
                                                    myskill.ToLevel = trainingDetails.ChildNodes[2].InnerText.ToInt32();
                                                    myskill.Pos = trainingDetails.ChildNodes[3].InnerText.ToInt32();
                                                    myskill.Notes = HttpUtility.HtmlDecode(trainingDetails.ChildNodes[4].InnerText);
                                                }
                                                catch (Exception e)
                                                {
                                                    // We don't have the required info
                                                }

                                                string keyName = myskill.Name + myskill.FromLevel + myskill.ToLevel;
                                                if (newQ.Queue.Contains(keyName) == false)
                                                {
                                                    if (myskill.ToLevel > myskill.FromLevel)
                                                    {
                                                        newQ.Queue.Add(myskill, keyName);

                                                        // Multi queue method
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    currentPilot.TrainingQueues.Add(newQ.Name, newQ);
                                }
                            }
                        }

                        // Iterate through the relevant nodes
                    }
                    catch (Exception e)
                    {
                        _worker.ReportProgress(0, "Error loading training for " + currentPilot.Name + ": " + e.Message);
                        MessageBox.Show(
                            "Error importing Training data for " + currentPilot.Name + "." + ControlChars.CrLf + "The error reported was " + e.Message, 
                            "Training Data Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>The convert main settings.</summary>
        /// <param name="oldSettings">The old settings.</param>
        private void ConvertMainSettings(EveSettings oldSettings)
        {
            try
            {
                _newSettings.MarketDataProvider = oldSettings.MarketDataProvider;
                _newSettings.MaxUpdateThreads = oldSettings.MaxUpdateThreads;
                _newSettings.MarketDataSource = oldSettings.MarketDataSource;
                _newSettings.Corporations = oldSettings.Corporations;
                _newSettings.PriceGroups = oldSettings.PriceGroups;
                _newSettings.SkillQueuePanelWidth = oldSettings.SkillQueuePanelWidth;
                _newSettings.AccountTimeLimit = oldSettings.AccountTimeLimit;
                _newSettings.NotifyAccountTime = oldSettings.NotifyAccountTime;
                _newSettings.NotifyInsuffClone = oldSettings.NotifyInsuffClone;
                _newSettings.StartWithPrimaryQueue = oldSettings.StartWithPrimaryQueue;
                _newSettings.IgnoreLastMessage = oldSettings.IgnoreLastMessage;
                _newSettings.LastMessageDate = oldSettings.LastMessageDate;
                _newSettings.DisableTrainingBar = oldSettings.DisableTrainingBar;
                _newSettings.EnableAutomaticSave = oldSettings.EnableAutomaticSave;
                _newSettings.AutomaticSaveTime = oldSettings.AutomaticSaveTime;
                _newSettings.RibbonMinimised = oldSettings.RibbonMinimised;
                _newSettings.ThemeSetByUser = oldSettings.ThemeSetByUser;
                _newSettings.ThemeTint = oldSettings.ThemeTint;
                _newSettings.ThemeStyle = oldSettings.ThemeStyle;
                _newSettings.SQLQueries = oldSettings.SQLQueries;
                _newSettings.BackupBeforeUpdate = oldSettings.BackupBeforeUpdate;
                _newSettings.QatLayout = oldSettings.QATLayout;
                _newSettings.NotifyEveNotification = oldSettings.NotifyEveNotification;
                _newSettings.NotifyEveMail = oldSettings.NotifyEveMail;
                _newSettings.AutoMailAPI = oldSettings.AutoMailAPI;
                _newSettings.EveHqBackupWarnFreq = oldSettings.EveHQBackupWarnFreq;
                _newSettings.EveHqBackupMode = oldSettings.EveHQBackupMode;
                _newSettings.EveHqBackupStart = oldSettings.EveHQBackupStart;
                _newSettings.EveHqBackupFreq = oldSettings.EveHQBackupFreq;
                _newSettings.EveHqBackupLast = oldSettings.EveHQBackupLast;
                _newSettings.EveHqBackupLastResult = oldSettings.EveHQBackupLastResult;
                _newSettings.IbShowAllItems = oldSettings.IBShowAllItems;
                _newSettings.EmailSenderAddress = oldSettings.EmailSenderAddress;
                _newSettings.UserQueueColumns = oldSettings.UserQueueColumns;
                _newSettings.StandardQueueColumns = oldSettings.StandardQueueColumns;
                _newSettings.DBTickerLocation = oldSettings.DBTickerLocation;
                _newSettings.DBTicker = oldSettings.DBTicker;
                _newSettings.CsvSeparatorChar = oldSettings.CSVSeparatorChar;
                _newSettings.DisableVisualStyles = oldSettings.DisableVisualStyles;
                _newSettings.DisableAutoWebConnections = oldSettings.DisableAutoWebConnections;
                _newSettings.TrainingBarHeight = oldSettings.TrainingBarHeight;
                _newSettings.TrainingBarWidth = oldSettings.TrainingBarWidth;
                _newSettings.TrainingBarDockPosition = oldSettings.TrainingBarDockPosition;
                _newSettings.MdiTabPosition = oldSettings.MDITabPosition;
                _newSettings.MarketRegionList = oldSettings.MarketRegionList;
                _newSettings.IgnoreBuyOrderLimit = oldSettings.IgnoreBuyOrderLimit;
                _newSettings.IgnoreSellOrderLimit = oldSettings.IgnoreSellOrderLimit;
                for (int i = 0; i <= 11; i++)
                {
                    _newSettings.set_PriceCriteria(i,oldSettings.get_PriceCriteria(i));
                }

                _newSettings.MarketLogUpdateData = oldSettings.MarketLogUpdateData;
                _newSettings.MarketLogUpdatePrice = oldSettings.MarketLogUpdatePrice;
                _newSettings.MarketLogPopupConfirm = oldSettings.MarketLogPopupConfirm;
                _newSettings.MarketLogToolTipConfirm = oldSettings.MarketLogToolTipConfirm;
                _newSettings.IgnoreBuyOrders = oldSettings.IgnoreBuyOrders;
                _newSettings.IgnoreSellOrders = oldSettings.IgnoreSellOrders;
                _newSettings.DBTimeout = oldSettings.DBTimeout;
                _newSettings.PilotSkillHighlightColor = oldSettings.PilotSkillHighlightColor;
                _newSettings.PilotSkillTextColor = oldSettings.PilotSkillTextColor;
                _newSettings.PilotGroupTextColor = oldSettings.PilotGroupTextColor;
                _newSettings.PilotGroupBackgroundColor = oldSettings.PilotGroupBackgroundColor;
                _newSettings.ErrorReportingEmail = oldSettings.ErrorReportingEmail;
                _newSettings.ErrorReportingName = oldSettings.ErrorReportingName;
                _newSettings.ErrorReportingEnabled = oldSettings.ErrorReportingEnabled;
                _newSettings.TaskbarIconMode = oldSettings.TaskbarIconMode;
                _newSettings.EcmDefaultLocation = oldSettings.ECMDefaultLocation;
                _newSettings.APIFileExtension = oldSettings.APIFileExtension;
                _newSettings.UseAppDirectoryForDB = oldSettings.UseAppDirectoryForDB;
                _newSettings.OmitCurrentSkill = oldSettings.OmitCurrentSkill;
                _newSettings.UpdateUrl = oldSettings.UpdateURL;
                _newSettings.UseCcpapiBackup = oldSettings.UseCCPAPIBackup;
                _newSettings.UseApirs = oldSettings.UseAPIRS;
                _newSettings.ApirsAddress = oldSettings.APIRSAddress;
                _newSettings.CcpapiServerAddress = oldSettings.CCPAPIServerAddress;
                for (int i = 1; i <= 4; i++)
                {
                    _newSettings.set_EveFolderLabel(i,oldSettings.get_EveFolderLabel(i));
                }

                _newSettings.PilotCurrentTrainSkillColor = oldSettings.PilotCurrentTrainSkillColor;
                _newSettings.PilotPartTrainedSkillColor = oldSettings.PilotPartTrainedSkillColor;
                _newSettings.PilotLevel5SkillColor = oldSettings.PilotLevel5SkillColor;
                _newSettings.PilotStandardSkillColor = oldSettings.PilotStandardSkillColor;
                _newSettings.PanelHighlightColor = oldSettings.PanelHighlightColor;
                _newSettings.PanelTextColor = oldSettings.PanelTextColor;
                _newSettings.PanelRightColor = oldSettings.PanelRightColor;
                _newSettings.PanelLeftColor = oldSettings.PanelLeftColor;
                _newSettings.PanelBottomRightColor = oldSettings.PanelBottomRightColor;
                _newSettings.PanelTopLeftColor = oldSettings.PanelTopLeftColor;
                _newSettings.PanelOutlineColor = oldSettings.PanelOutlineColor;
                _newSettings.PanelBackgroundColor = oldSettings.PanelBackgroundColor;
                _newSettings.LastMarketPriceUpdate = oldSettings.LastMarketPriceUpdate;
                _newSettings.LastFactionPriceUpdate = oldSettings.LastFactionPriceUpdate;
                for (int i = 1; i <= 4; i++)
                {
                    _newSettings.set_EveFolderLua(i,oldSettings.get_EveFolderLUA(i));
                }

                _newSettings.CycleG15Time = oldSettings.CycleG15Time;
                _newSettings.CycleG15Pilots = oldSettings.CycleG15Pilots;
                _newSettings.ActivateG15 = oldSettings.ActivateG15;
                _newSettings.AutoAPI = oldSettings.AutoAPI;
                _newSettings.MainFormWindowState = (FormWindowState)oldSettings.MainFormPosition[4];
                _newSettings.MainFormLocation = new Point(oldSettings.MainFormPosition[0], oldSettings.MainFormPosition[1]);
                _newSettings.MainFormSize = new Size(oldSettings.MainFormPosition[2], oldSettings.MainFormPositio[3]);
                _newSettings.DeleteSkills = oldSettings.DeleteSkills;
                _newSettings.PartialTrainColor = oldSettings.PartialTrainColor;
                _newSettings.ReadySkillColor = oldSettings.ReadySkillColor;
                _newSettings.IsPreReqColor = oldSettings.IsPreReqColor;
                _newSettings.HasPreReqColor = oldSettings.HasPreReqColor;
                _newSettings.BothPreReqColor = oldSettings.BothPreReqColor;
                _newSettings.DtClashColor = oldSettings.DTClashColor;
                _newSettings.ColorHighlightQueuePreReq = oldSettings.ColorHighlightQueuePreReq;
                _newSettings.ColorHighlightQueueTraining = oldSettings.ColorHighlightQueueTraining;
                _newSettings.ColorHighlightPilotTraining = oldSettings.ColorHighlightPilotTraining;
                _newSettings.ContinueTraining = oldSettings.ContinueTraining;
                _newSettings.EMailPassword = oldSettings.EMailPassword;
                _newSettings.EMailUsername = oldSettings.EMailUsername;
                _newSettings.UseSsl = oldSettings.UseSSL;
                _newSettings.UseSmtpAuth = oldSettings.UseSMTPAuth;
                _newSettings.EMailAddress = oldSettings.EMailAddress;
                _newSettings.EMailPort = oldSettings.EMailPort;
                _newSettings.EMailServer = oldSettings.EMailServer;
                _newSettings.NotifySoundFile = oldSettings.NotifySoundFile;
                _newSettings.NotifyOffset = oldSettings.NotifyOffset;
                _newSettings.NotifyEarly = oldSettings.NotifyEarly;
                _newSettings.NotifyNow = oldSettings.NotifyNow;
                _newSettings.NotifySound = oldSettings.NotifySound;
                _newSettings.NotifyEMail = oldSettings.NotifyEMail;
                _newSettings.NotifyDialog = oldSettings.NotifyDialog;
                _newSettings.NotifyToolTip = oldSettings.NotifyToolTip;
                _newSettings.ShutdownNotifyPeriod = oldSettings.ShutdownNotifyPeriod;
                _newSettings.ShutdownNotify = oldSettings.ShutdownNotify;
                _newSettings.ServerOffset = oldSettings.ServerOffset;
                _newSettings.EnableEveStatus = oldSettings.EnableEveStatus;
                _newSettings.ProxyUseDefault = oldSettings.ProxyUseDefault;
                _newSettings.ProxyUseBasic = oldSettings.ProxyUseBasic;
                _newSettings.ProxyPassword = oldSettings.ProxyPassword;
                _newSettings.ProxyUsername = oldSettings.ProxyUsername;
                _newSettings.ProxyPort = oldSettings.ProxyPort;
                _newSettings.ProxyServer = oldSettings.ProxyServer;
                _newSettings.ProxyRequired = oldSettings.ProxyRequired;
                _newSettings.IgbPort = oldSettings.IGBPort;
                _newSettings.IgbAutoStart = oldSettings.IGBAutoStart;
                _newSettings.IgbFullMode = oldSettings.IGBFullMode;
                _newSettings.IgbAllowedData = oldSettings.IGBAllowedData;
                _newSettings.AutoHide = oldSettings.AutoHide;
                _newSettings.AutoStart = oldSettings.AutoStart;
                _newSettings.AutoCheck = oldSettings.AutoCheck;
                _newSettings.MinimiseExit = oldSettings.MinimiseExit;
                _newSettings.AutoMinimise = oldSettings.AutoMinimise;
                _newSettings.StartupPilot = oldSettings.StartupPilot;
                _newSettings.StartupForms = 0;
                switch (oldSettings.StartupView)
                {
                    case "EveHQ Dashboard":
                        _newSettings.StartupForms = 8;
                        break;
                    case "Pilot Information":
                        _newSettings.StartupForms = 1;
                        break;
                    case "Pilot Summary Report":
                        _newSettings.StartupForms = 32;
                        break;
                    case "Skill Training":
                        _newSettings.StartupForms = 2;
                        break;
                }

                for (int i = 1; i <= 4; i++)
                {
                    _newSettings.EveFolder[i] = oldSettings.EveFolder[i];
                }

                _newSettings.BackupAuto = oldSettings.BackupAuto;
                _newSettings.BackupStart = oldSettings.BackupStart;
                _newSettings.BackupFreq = oldSettings.BackupFreq;
                _newSettings.BackupLast = oldSettings.BackupLast;
                _newSettings.BackupLastResult = oldSettings.BackupLastResult;
                _newSettings.QColumnsSet = oldSettings.QColumnsSet;
                for (int i = 0; i <= 20; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        _newSettings.QColumns[i, j] = oldSettings.QColumns[i, j];
                    }
                }

                _newSettings.MarketRegions = oldSettings.MarketRegions;
                _newSettings.MarketSystem = oldSettings.MarketSystem;
                _newSettings.MarketUseRegionMarket = oldSettings.MarketUseRegionMarket;
                _newSettings.MarketDefaultMetric = oldSettings.MarketDefaultMetric;
                _newSettings.MarketDataUploadEnabled = oldSettings.MarketDataUploadEnabled;
                _newSettings.MarketStatOverrides = oldSettings.MarketStatOverrides;
                _newSettings.MarketDefaultTransactionType = oldSettings.MarketDefaultTransactionType;
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting main settings: " + e.Message);
            }
        }

        /// <summary>The convert accounts.</summary>
        /// <param name="oldSettings">The old settings.</param>
        private void ConvertAccounts(EveSettings oldSettings)
        {
            try
            {
                _newSettings.Accounts.Clear();
                foreach (EveAccount account in oldSettings.Accounts)
                {
                    var newAccount = new EveHQAccount();
                    newAccount.AccessMask = account.AccessMask;
                    newAccount.APIAccountStatus = account.APIAccountStatus;
                    newAccount.APIKey = account.APIKey;
                    newAccount.ApiKeyExpiryDate = account.APIKeyExpiryDate;
                    newAccount.ApiKeySystem = account.APIKeySystem;
                    newAccount.APIKeyType = account.APIKeyType;
                    newAccount.Characters.Clear();
                    foreach (string name in account.Characters)
                    {
                        newAccount.Characters.Add(name);
                    }

                    newAccount.CorpApiAccountKey = account.CorpAPIAccountKey;
                    newAccount.CreateDate = account.CreateDate;
                    newAccount.FailedAttempts = account.FailedAttempts;
                    newAccount.FriendlyName = account.FriendlyName;
                    newAccount.LastAccountStatusCheck = account.LastAccountStatusCheck;
                    newAccount.LogonCount = account.LogonCount;
                    newAccount.LogonMinutes = account.LogonMinutes;
                    newAccount.PaidUntil = account.PaidUntil;
                    newAccount.UserID = account.userID;
                    _newSettings.Accounts.Add(newAccount.UserID, newAccount);
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting API account settings: " + e.Message);
            }
        }

        /// <summary>The convert pilots.</summary>
        /// <param name="oldSettings">The old settings.</param>
        private void ConvertPilots(EveSettings oldSettings)
        {
            try
            {
                _newSettings.Pilots.Clear();
                foreach (Pilot pilot in oldSettings.Pilots)
                {
                    var newPilot = new EveHQPilot();

                    newPilot.Name = pilot.Name;
                    newPilot.ID = pilot.ID;
                    newPilot.Account = pilot.Account;
                    newPilot.AccountPosition = pilot.AccountPosition;
                    newPilot.Race = pilot.Race;
                    newPilot.Blood = pilot.Blood;
                    newPilot.Gender = pilot.Gender;
                    newPilot.Corp = pilot.Corp;
                    newPilot.CorpID = pilot.CorpID;
                    newPilot.Isk = pilot.Isk;
                    newPilot.CloneName = pilot.CloneName;
                    if (Information.IsNumeric(pilot.CloneSP))
                    {
                        newPilot.CloneSP = Convert.ToInt32(pilot.CloneSP);
                    }
                    else
                    {
                        newPilot.CloneSP = 0;
                    }

                    newPilot.SkillPoints = pilot.SkillPoints;
                    newPilot.Training = pilot.Training;
                    newPilot.TrainingStartTime = pilot.TrainingStartTime;
                    newPilot.TrainingStartTimeActual = pilot.TrainingStartTimeActual;
                    newPilot.TrainingEndTime = pilot.TrainingEndTime;
                    newPilot.TrainingEndTimeActual = pilot.TrainingEndTimeActual;
                    if (Information.IsNumeric(pilot.TrainingSkillID))
                    {
                        newPilot.TrainingSkillID = Convert.ToInt32(pilot.TrainingSkillID);
                    }
                    else
                    {
                        newPilot.TrainingSkillID = 0;
                    }

                    newPilot.TrainingSkillName = pilot.TrainingSkillName;
                    newPilot.TrainingStartSP = pilot.TrainingStartSP;
                    newPilot.TrainingEndSP = pilot.TrainingEndSP;
                    newPilot.TrainingCurrentSP = pilot.TrainingCurrentSP;
                    newPilot.TrainingCurrentTime = pilot.TrainingCurrentTime;
                    newPilot.TrainingSkillLevel = pilot.TrainingSkillLevel;
                    newPilot.TrainingNotifiedNow = pilot.TrainingNotifiedNow;
                    newPilot.TrainingNotifiedEarly = pilot.TrainingNotifiedEarly;
                    newPilot.CAtt = pilot.CAtt;
                    newPilot.IntAtt = pilot.IAtt;
                    newPilot.MAtt = pilot.MAtt;
                    newPilot.PAtt = pilot.PAtt;
                    newPilot.WAtt = pilot.WAtt;
                    newPilot.CImplant = pilot.CImplant;
                    newPilot.IntImplant = pilot.IImplant;
                    newPilot.MImplant = pilot.MImplant;
                    newPilot.PImplant = pilot.PImplant;
                    newPilot.WImplant = pilot.WImplant;
                    newPilot.CImplantA = pilot.CImplantA;
                    newPilot.IntImplantA = pilot.IImplantA;
                    newPilot.MImplantA = pilot.MImplantA;
                    newPilot.PImplantA = pilot.PImplantA;
                    newPilot.WImplantA = pilot.WImplantA;
                    newPilot.CImplantM = pilot.CImplantM;
                    newPilot.IntImplantM = pilot.IImplantM;
                    newPilot.MImplantM = pilot.MImplantM;
                    newPilot.PImplantM = pilot.PImplantM;
                    newPilot.WImplantM = pilot.WImplantM;
                    newPilot.UseManualImplants = pilot.UseManualImplants;
                    newPilot.CAttT = pilot.CAttT;
                    newPilot.IntAttT = pilot.IAttT;
                    newPilot.MAttT = pilot.MAttT;
                    newPilot.PAttT = pilot.PAttT;
                    newPilot.WAttT = pilot.WAttT;
                    ConvertPilotSkills(pilot, newPilot);
                    ConvertPilotQueuedSkills(pilot, newPilot);
                    newPilot.QueuedSkillTime = pilot.QueuedSkillTime;
                    newPilot.QualifiedCertificates.Clear();

                    // For Each c As String In pilot.Certificates
                    // newPilot.Certificates.Add(CInt(c))
                    // Next
                    ConvertPilotCertificates(ref newPilot);

                    newPilot.PrimaryQueue = pilot.PrimaryQueue;
                    ConvertTrainingQueues(pilot, newPilot);
                    newPilot.ActiveQueue = ConvertPilotSkillQueue(pilot.ActiveQueue);
                    newPilot.ActiveQueueName = pilot.ActiveQueueName;
                    newPilot.CacheFileTime = pilot.CacheFileTime;
                    newPilot.CacheExpirationTime = pilot.CacheExpirationTime;
                    newPilot.TrainingFileTime = pilot.TrainingFileTime;
                    newPilot.TrainingExpirationTime = pilot.TrainingExpirationTime;
                    newPilot.Updated = pilot.Updated;
                    newPilot.LastUpdate = pilot.LastUpdate;
                    newPilot.Active = pilot.Active;
                    for (int index = 0; index <= 53; index++)
                    {
                        newPilot.KeySkills((KeySkill)index) = Convert.ToInt32(pilot.KeySkills(index));
                    }

                    newPilot.Standings = pilot.Standings;
                    newPilot.CorpRoles = pilot.CorpRoles;
                    _newSettings.Pilots.Add(newPilot.Name, newPilot);
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting pilot settings: " + e.Message);
            }
        }

        /// <summary>The convert pilot skills.</summary>
        /// <param name="pilot">The pilot.</param>
        /// <param name="newPilot">The new pilot.</param>
        private void ConvertPilotSkills(Pilot pilot, EveHQPilot newPilot)
        {
            newPilot.PilotSkills.Clear();
            foreach (PilotSkill oldskill in pilot.PilotSkills)
            {
                var newSkill = new EveHQPilotSkill();
                newSkill.ID = oldskill.ID.ToInt32();
                newSkill.Name = oldskill.Name;
                newSkill.GroupID = oldskill.GroupID.ToInt32();
                newSkill.Rank = oldskill.Rank;
                newSkill.SP = oldskill.SP;
                newSkill.Level = oldskill.Level;
                newPilot.PilotSkills.Add(newSkill.Name, newSkill);
            }
        }

        /// <summary>The convert pilot certificates.</summary>
        /// <param name="pilot">The pilot.</param>
        private void ConvertPilotCertificates(ref EveHQPilot pilot)
        {
            pilot.QualifiedCertificates.Clear();

            var qualifiedCerts = new Dictionary<int, CertificateGrade>();

            foreach (Certificate cert_loopVariable in StaticData.Certificates.Values)
            {
                Certificate cert = cert_loopVariable;
                foreach (CertificateGrade grade_loopVariable in cert.GradesAndSkills.Keys)
                {
                    CertificateGrade grade = grade_loopVariable;
                    if (CheckPilotSkillsForCertGrade(cert.GradesAndSkills[grade], ref pilot))
                    {
                        if (qualifiedCerts.ContainsKey(cert.Id))
                        {
                            qualifiedCerts[cert.Id] = grade;
                        }
                        else
                        {
                            qualifiedCerts.Add(cert.Id, grade);
                        }
                    }
                }
            }

            // take the collection of qualified certs and apply them to the pilot
            foreach (int certId_loopVariable in qualifiedCerts.Keys)
            {
                int certId = certId_loopVariable;
                pilot.QualifiedCertificates.Add(certId, qualifiedCerts[certId]);
            }
        }

        /// <summary>The check pilot skills for cert grade.</summary>
        /// <param name="reqSkills">The req skills.</param>
        /// <param name="pilot">The pilot.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool CheckPilotSkillsForCertGrade(SortedList<int, int> reqSkills, ref EveHQPilot pilot)
        {
            var qualifications = new SortedList<int, bool>();
            foreach (int skill_loopVariable in reqSkills.Keys)
            {
                int skill = skill_loopVariable;
                var pSkill = new EveHQPilotSkill();
                qualifications.Add(skill, false);
                if (pilot.PilotSkills.TryGetValue(skill.ToString(), out pSkill))
                {
                    if (pSkill.Rank >= reqSkills[skill])
                    {
                        qualifications[skill] = true;
                    }
                }
            }

            return qualifications.Values.All(q => q);
        }

        /// <summary>The convert pilot queued skills.</summary>
        /// <param name="pilot">The pilot.</param>
        /// <param name="newPilot">The new pilot.</param>
        private void ConvertPilotQueuedSkills(Pilot pilot, EveHQPilot newPilot)
        {
            newPilot.QueuedSkills.Clear();
            foreach (PilotQueuedSkill oldskill in pilot.QueuedSkills.Values)
            {
                var newSkill = new EveHQPilotQueuedSkill();
                newSkill.Position = oldskill.Position;
                newSkill.SkillID = oldskill.SkillID;
                newSkill.Level = oldskill.Level;
                newSkill.StartSP = oldskill.StartSP;
                newSkill.EndSP = oldskill.EndSP;
                newSkill.StartTime = oldskill.StartTime;
                newSkill.EndTime = oldskill.EndTime;
                newPilot.QueuedSkills.Add(newSkill.Position, newSkill);
            }
        }

        /// <summary>The convert training queues.</summary>
        /// <param name="pilot">The pilot.</param>
        /// <param name="newPilot">The new pilot.</param>
        private void ConvertTrainingQueues(Pilot pilot, EveHQPilot newPilot)
        {
            newPilot.TrainingQueues.Clear();
            foreach (SkillQueue oldQueue in pilot.TrainingQueues.Values)
            {
                newPilot.TrainingQueues.Add(oldQueue.Name, ConvertPilotSkillQueue(oldQueue));
            }
        }

        /// <summary>The convert pilot skill queue.</summary>
        /// <param name="oldQueue">The old queue.</param>
        /// <returns>The <see cref="EveHQSkillQueue"/>.</returns>
        private EveHQSkillQueue ConvertPilotSkillQueue(SkillQueue oldQueue)
        {
            var newQueue = new EveHQSkillQueue();
            newQueue.Name = oldQueue.Name;
            newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining;
            newQueue.Primary = oldQueue.Primary;
            newQueue.QueueSkills = oldQueue.QueueSkills;
            newQueue.QueueTime = oldQueue.QueueTime;
            newQueue.Queue.Clear();
            foreach (SkillQueueItem oldQueueItem in oldQueue.Queue)
            {
                var newQueueItem = new EveHQSkillQueueItem();
                newQueueItem.Name = oldQueueItem.Name;
                newQueueItem.FromLevel = oldQueueItem.FromLevel;
                newQueueItem.ToLevel = oldQueueItem.ToLevel;
                newQueueItem.Pos = oldQueueItem.Pos;
                newQueueItem.Priority = oldQueueItem.Priority;
                newQueueItem.Notes = oldQueueItem.Notes;
                newQueue.Queue.Add(newQueueItem.Key, newQueueItem);
            }

            return newQueue;
        }

        /// <summary>The convert plugins.</summary>
        /// <param name="oldSettings">The old settings.</param>
        private void ConvertPlugins(EveSettings oldSettings)
        {
            try
            {
                _newSettings.Plugins.Clear();
                foreach (PlugIn plugin in oldSettings.Plugins.Values)
                {
                    var newPlugin = new EveHQPlugInConfig();
                    newPlugin.Name = plugin.Name;
                    newPlugin.Disabled = plugin.Disabled;
                    _newSettings.Plugins.Add(newPlugin.Name, newPlugin);
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting plug-in settings: " + e.Message);
            }
        }

        /// <summary>The convert dashboard.</summary>
        /// <param name="oldSettings">The old settings.</param>
        private void ConvertDashboard(EveSettings oldSettings)
        {
            _newSettings.DashboardConfiguration.Clear();
            foreach (SortedList<string, object> config in oldSettings.DashboardConfiguration)
            {
                try
                {
                    var newConfig = new SortedList<string, object>();
                    foreach (string configProperty in config.Keys)
                    {
                        switch (configProperty)
                        {
                            case "ControlLocation":
                            case "ControlSize":
                                newConfig.Add(configProperty, config[configProperty].ToString());
                                break;
                            default:
                                newConfig.Add(configProperty, config[configProperty]);
                                break;
                        }
                    }

                    _newSettings.DashboardConfiguration.Add(newConfig);
                }
                catch (Exception e)
                {
                    _worker.ReportProgress(0, "Error converting dashboard widget: " + e.Message);
                }
            }
        }

        #endregion

        #region "Prism Data/Settings Conversion Methods"

        /// <summary>The convert prism settings.</summary>
        /// <param name="settingsFolder">The settings folder.</param>
        private void ConvertPrismSettings(string settingsFolder)
        {
            string prismFolder = Path.Combine(settingsFolder, "Prism");

            _worker.ReportProgress(0, "Prism Settings Conversion Step 1/4: Converting settings...");
            ConvertSettings(prismFolder);

            _worker.ReportProgress(0, "Prism Settings Conversion Step 2/4: Converting blueprints...");
            ConvertBlueprintAssets(prismFolder);

            _worker.ReportProgress(0, "Prism Settings Conversion Step 3/4: Converting production jobs...");
            ConvertProductionJobs(prismFolder);

            _worker.ReportProgress(0, "Prism Settings Conversion Step 4/4: Converting batch jobs...");
            ConvertBatchJobs(prismFolder);
        }

        /// <summary>The convert settings.</summary>
        /// <param name="prismFolder">The prism folder.</param>
        private void ConvertSettings(string prismFolder)
        {
            try
            {
                Prism.Settings oldSettings = default(Prism.Settings);

                if (File.Exists(Path.Combine(prismFolder, "PrismSettings.bin")))
                {
                    using (var s = new FileStream(Path.Combine(prismFolder, "PrismSettings.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldSettings = (Prism.Settings)f.Deserialize(s);
                    }

                    var newSettings = new PrismSettings();

                    newSettings.CorpReps = oldSettings.CorpReps;
                    newSettings.SlotNameWidth = oldSettings.SlotNameWidth;
                    newSettings.UserSlotColumns = oldSettings.UserSlotColumns;
                    newSettings.StandardSlotColumns = oldSettings.StandardSlotColumns;
                    newSettings.DefaultBPCalcAssetOwner = oldSettings.DefaultBPCalcAssetOwner;
                    newSettings.DefaultBPCalcManufacturer = oldSettings.DefaultBPCalcManufacturer;
                    newSettings.DefaultBPOwner = oldSettings.DefaultBPOwner;
                    newSettings.DefaultCharacter = oldSettings.DefaultCharacter;
                    newSettings.BPCCosts.Clear();
                    foreach (BPCCostInfo bpc in oldSettings.BPCCosts.Values)
                    {
                        var newBPC = new BlueprintCopyCostInfo(Convert.ToInt32(bpc.ID), bpc.MaxRunCost, bpc.MinRunCost);
                        newSettings.BPCCosts.Add(newBPC.ID, newBPC);
                    }

                    newSettings.LabRunningCost = oldSettings.LabRunningCost;
                    newSettings.LabInstallCost = oldSettings.LabInstallCost;
                    newSettings.FactoryRunningCost = oldSettings.FactoryRunningCost;
                    newSettings.FactoryInstallCost = oldSettings.FactoryInstallCost;

                    string json = JsonConvert.SerializeObject(newSettings, Newtonsoft.Json.Formatting.Indented);

                    // Write a JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(prismFolder, "PrismSettings.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    File.Move(Path.Combine(prismFolder, "PrismSettings.bin"), Path.Combine(prismFolder, "OldPrismSettings.bin"));
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting Prism main settings: " + e.Message);
            }
        }

        /// <summary>The convert blueprint assets.</summary>
        /// <param name="prismFolder">The prism folder.</param>
        private void ConvertBlueprintAssets(string prismFolder)
        {
            try
            {
                SortedList<string, SortedList<string, BlueprintAsset>> ownerBlueprints = default(SortedList<string, SortedList<string, BlueprintAsset>>);

                if (File.Exists(Path.Combine(prismFolder, "OwnerBlueprints.bin")))
                {
                    using (var s = new FileStream(Path.Combine(prismFolder, "OwnerBlueprints.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        ownerBlueprints = (SortedList<string, SortedList<string, BlueprintAsset>>)f.Deserialize(s);
                    }

                    string json = JsonConvert.SerializeObject(ownerBlueprints, Newtonsoft.Json.Formatting.Indented);

                    // Write a JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(prismFolder, "OwnerBlueprints.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    File.Move(Path.Combine(prismFolder, "OwnerBlueprints.bin"), Path.Combine(prismFolder, "OldOwnerBlueprints.bin"));
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting Prism blueprint assets: " + e.Message);
            }
        }

        /// <summary>The convert production jobs.</summary>
        /// <param name="prismFolder">The prism folder.</param>
        private void ConvertProductionJobs(string prismFolder)
        {
            try
            {
                SortedList<string, ProductionJob> oldJobs = default(SortedList<string, ProductionJob>);

                if (File.Exists(Path.Combine(prismFolder, "ProductionJobs.bin")))
                {
                    using (var s = new FileStream(Path.Combine(prismFolder, "ProductionJobs.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldJobs = (SortedList<string, ProductionJob>)f.Deserialize(s);
                    }

                    // Load up some static data for the conversion
                    StaticData.LoadCoreCacheForConversion(Path.Combine(Application.StartupPath, "StaticData"));

                    // Convert the old jobs into the new format
                    var newJobs = new SortedList<string, Job>();
                    foreach (ProductionJob job in oldJobs.Values)
                    {
                        newJobs.Add(job.JobName, ConvertProductionJob(job));
                    }

                    // Create the JSON string
                    string json = JsonConvert.SerializeObject(newJobs, Newtonsoft.Json.Formatting.Indented);

                    // Write a JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(prismFolder, "ProductionJobs.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    File.Move(Path.Combine(prismFolder, "ProductionJobs.bin"), Path.Combine(prismFolder, "OldProductionJobs.bin"));
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting Prism production jobs: " + e.Message);
            }
        }

        /// <summary>The convert production job.</summary>
        /// <param name="oldJob">The old job.</param>
        /// <returns>The <see cref="Job"/>.</returns>
        private Job ConvertProductionJob(ProductionJob oldJob)
        {
            if (oldJob == null)
            {
                return null;
            }

            var newJob = new Job();
            newJob.JobName = oldJob.JobName;
            newJob.BlueprintId = oldJob.BPID;
            newJob.TypeID = oldJob.TypeID;
            newJob.TypeName = oldJob.TypeName;
            newJob.PerfectUnits = oldJob.PerfectUnits;
            newJob.WasteUnits = oldJob.WasteUnits;
            newJob.Runs = oldJob.Runs;
            newJob.Manufacturer = oldJob.Manufacturer;
            newJob.BlueprintOwner = oldJob.BPOwner;
            newJob.PESkill = oldJob.PESkill;
            newJob.IndSkill = oldJob.IndSkill;
            newJob.ProdImplant = oldJob.ProdImplant;
            newJob.OverridingME = oldJob.OverridingME;
            newJob.OverridingPE = oldJob.OverridingPE;
            newJob.StartTime = oldJob.StartTime;
            newJob.RunTime = oldJob.RunTime;
            newJob.Cost = oldJob.Cost;
            newJob.HasInventionJob = oldJob.HasInventionJob;
            newJob.ProduceSubJob = oldJob.ProduceSubJob;
            newJob.InventionJob = ConvertInventionJob(oldJob.InventionJob);
            if (oldJob.AssemblyArray == null)
            {
                newJob.AssemblyArray = null;
            }
            else
            {
                newJob.AssemblyArray = StaticData.AssemblyArrays[oldJob.AssemblyArray.ID];
            }

            foreach (string key in oldJob.SubJobMEs.Keys)
            {
                newJob.SubJobMEs.Add(key.ToInt32(), oldJob.SubJobMEs[key]);
            }

            if (StaticData.Blueprints.ContainsKey(oldJob.CurrentBP.ID))
            {
                newJob.CurrentBlueprint = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints[oldJob.CurrentBP.ID]);
                newJob.CurrentBlueprint.MELevel = oldJob.CurrentBP.MELevel;
                newJob.CurrentBlueprint.PELevel = oldJob.CurrentBP.PELevel;
                newJob.CurrentBlueprint.Runs = oldJob.CurrentBP.Runs;
                newJob.CurrentBlueprint.AssetId = oldJob.CurrentBP.AssetID;
            }
            else
            {
                newJob.CurrentBlueprint = null;
            }

            foreach (object resource in oldJob.RequiredResources.Values)
            {
                if (resource is RequiredResource)
                {
                    var rResource = (RequiredResource)resource;
                    var newResource = new JobResource();
                    newResource.TypeID = rResource.TypeID;
                    newResource.TypeName = rResource.TypeName;
                    newResource.TypeGroup = rResource.TypeGroup;
                    newResource.TypeCategory = rResource.TypeCategory;
                    newResource.PerfectUnits = rResource.PerfectUnits;
                    newResource.BaseUnits = rResource.BaseUnits;
                    newResource.WasteUnits = rResource.WasteUnits;
                    newJob.Resources.Add(newResource.TypeID, newResource);
                }
                else
                {
                    // This is another production job
                    var subJob = (ProductionJob)resource;
                    newJob.SubJobs.Add(subJob.TypeID, ConvertProductionJob(subJob));
                }
            }

            return newJob;
        }

        /// <summary>The convert invention job.</summary>
        /// <param name="oldJob">The old job.</param>
        /// <returns>The <see cref="InventionJob"/>.</returns>
        private Prism.BPCalc.InventionJob ConvertInventionJob(Prism.BPCalc.InventionJob oldJob)
        {
            if (oldJob == null)
            {
                return null;
            }

            var newJob = new Prism.BPCalc.InventionJob();
            newJob.InventedBpid = oldJob.InventedBpid;
            newJob.BaseChance = oldJob.BaseChance;
            newJob.DecryptorUsed = ConvertDecryptor(oldJob.DecryptorUsed);
            newJob.MetaItemId = oldJob.MetaItemId;
            newJob.MetaItemLevel = oldJob.MetaItemLevel;
            newJob.OverrideBpcRuns = oldJob.OverrideBpcRuns;
            newJob.BpcRuns = oldJob.BpcRuns;
            newJob.OverrideEncSkill = oldJob.OverrideEncSkill;
            newJob.OverrideDcSkill1 = oldJob.OverrideDcSkill1;
            newJob.OverrideDcSkill2 = oldJob.OverrideDcSkill2;
            newJob.EncryptionSkill = oldJob.EncryptionSkill;
            newJob.DatacoreSkill1 = oldJob.DatacoreSkill1;
            newJob.DatacoreSkill2 = oldJob.DatacoreSkill2;
            newJob.ProductionJob = ConvertProductionJob(oldJob.ProductionJob);
            return newJob;
        }

        /// <summary>The convert decryptor.</summary>
        /// <param name="oldDecryptor">The old decryptor.</param>
        /// <returns>The <see cref="Decryptor"/>.</returns>
        private Prism.BPCalc.Decryptor ConvertDecryptor(Prism.BPCalc.Decryptor oldDecryptor)
        {
            if (oldDecryptor == null)
            {
                return null;
            }

            var newDecryptor = new Prism.BPCalc.Decryptor();
            newDecryptor.GroupID = oldDecryptor.GroupID;
            newDecryptor.ID = Convert.ToInt32(oldDecryptor.ID);
            newDecryptor.MEMod = oldDecryptor.MEMod;
            newDecryptor.Name = oldDecryptor.Name;
            newDecryptor.PEMod = oldDecryptor.PEMod;
            newDecryptor.ProbMod = oldDecryptor.ProbMod;
            newDecryptor.RunMod = oldDecryptor.RunMod;
            return newDecryptor;
        }

        /// <summary>The convert batch jobs.</summary>
        /// <param name="prismFolder">The prism folder.</param>
        private void ConvertBatchJobs(string prismFolder)
        {
            try
            {
                SortedList<string, BatchJob> oldJobs = default(SortedList<string, BatchJob>);

                if (File.Exists(Path.Combine(prismFolder, "BatchJobs.bin")))
                {
                    using (var s = new FileStream(Path.Combine(prismFolder, "BatchJobs.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldJobs = (SortedList<string, BatchJob>)f.Deserialize(s);
                    }

                    string json = JsonConvert.SerializeObject(oldJobs, Newtonsoft.Json.Formatting.Indented);

                    // Write a JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(prismFolder, "BatchJobs.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    My.Computer.FileSystem.RenameFile(Path.Combine(prismFolder, "BatchJobs.bin"), "OldBatchJobs.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting Prism batch jobs: " + e.Message);
            }
        }

        #endregion

        #region "HQF Data/Settings Conversion Methods"

        /// <summary>The convert hqf settings.</summary>
        /// <param name="settingsFolder">The settings folder.</param>
        private void ConvertHQFSettings(string settingsFolder)
        {
            string hqfFolder = Path.Combine(settingsFolder, "HQF");

            _worker.ReportProgress(0, "HQF Settings Conversion Step 1/7: Converting settings...");
            ConvertMainHQFSettings(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 2/7: Converting defence profiles...");
            ConvertDefenceProfiles(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 3/7: Converting damage profiles...");
            ConvertDamageProfiles(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 4/7: Converting fittings...");
            ConvertSavedFittings(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 5/7: Converting pilots...");
            ConvertPilots(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 6/7: Converting custom ship classes...");
            ConvertCustomShipClasses(hqfFolder);

            _worker.ReportProgress(0, "HQF Settings Conversion Step 7/7: Converting custom ships...");
            ConvertCustomShips(hqfFolder);
        }

        /// <summary>The convert main hqf settings.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertMainHQFSettings(string hqfFolder)
        {
            try
            {
                HQF.Settings oldSettings = default(HQF.Settings);

                // Check for the fittings file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "HQFSettings.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "HQFSettings.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldSettings = (HQF.Settings)f.Deserialize(s);
                    }

                    var newSettings = new PluginSettings();

                    newSettings.HiSlotColour = oldSettings.HiSlotColour;
                    newSettings.MidSlotColour = oldSettings.MidSlotColour;
                    newSettings.LowSlotColour = oldSettings.LowSlotColour;
                    newSettings.RigSlotColour = oldSettings.RigSlotColour;
                    newSettings.SubSlotColour = oldSettings.SubSlotColour;
                    newSettings.DefaultPilot = oldSettings.DefaultPilot;
                    newSettings.RestoreLastSession = oldSettings.RestoreLastSession;
                    newSettings.LastPriceUpdate = oldSettings.LastPriceUpdate;
                    newSettings.ModuleFilter = oldSettings.ModuleFilter;
                    newSettings.AutoUpdateHQFSkills = oldSettings.AutoUpdateHQFSkills;
                    newSettings.OpenFittingList = oldSettings.OpenFittingList;
                    newSettings.ShowPerformanceData = oldSettings.ShowPerformanceData;
                    newSettings.CloseInfoPanel = oldSettings.CloseInfoPanel;
                    newSettings.CapRechargeConstant = oldSettings.CapRechargeConstant;
                    newSettings.ShieldRechargeConstant = oldSettings.ShieldRechargeConstant;
                    newSettings.StandardSlotColumns = oldSettings.StandardSlotColumns;
                    newSettings.UserSlotColumns = oldSettings.UserSlotColumns;
                    newSettings.Favourites = oldSettings.Favourites;
                    newSettings.MruLimit = oldSettings.MRULimit;
                    newSettings.MruModules = oldSettings.MRUModules;
                    newSettings.ShipPanelWidth = oldSettings.ShipPanelWidth;
                    newSettings.ModPanelWidth = oldSettings.ModPanelWidth;
                    newSettings.ShipSplitterWidth = oldSettings.ShipSplitterWidth;
                    newSettings.ModSplitterWidth = oldSettings.ModSplitterWidth;
                    newSettings.MissileRangeConstant = oldSettings.MissileRangeConstant;
                    newSettings.IncludeCapReloadTime = oldSettings.IncludeCapReloadTime;
                    newSettings.IncludeAmmoReloadTime = oldSettings.IncludeAmmoReloadTime;
                    newSettings.UseLastPilot = oldSettings.UseLastPilot;
                    newSettings.StorageBayHeight = oldSettings.StorageBayHeight;
                    newSettings.SlotNameWidth = oldSettings.SlotNameWidth;
                    foreach (ImplantGroup ig in oldSettings.ImplantGroups.Values)
                    {
                        var ic = new ImplantCollection(true);
                        ic.GroupName = ig.GroupName;
                        for (int slot = 0; slot <= 10; slot++)
                        {
                            ic.ImplantName(slot) = ig.ImplantName(slot);
                        }

                        newSettings.ImplantGroups.Add(ic.GroupName, ic);
                    }

                    newSettings.ModuleListColWidths = oldSettings.ModuleListColWidths;
                    newSettings.IgnoredAttributeColumns = oldSettings.IgnoredAttributeColumns;
                    newSettings.SortedAttributeColumn = oldSettings.SortedAttributeColumn;
                    newSettings.MetaVariationsFormSize = oldSettings.MetaVariationsFormSize;
                    newSettings.DefensePanelIsCollapsed = oldSettings.DefensePanelIsCollapsed;
                    newSettings.CapacitorPanelIsCollapsed = oldSettings.CapacitorPanelIsCollapsed;
                    newSettings.DamagePanelIsCollapsed = oldSettings.DamagePanelIsCollapsed;
                    newSettings.TargetingPanelIsCollapsed = oldSettings.TargetingPanelIsCollapsed;
                    newSettings.PropulsionPanelIsCollapsed = oldSettings.PropulsionPanelIsCollapsed;
                    newSettings.CargoPanelIsCollapsed = oldSettings.CargoPanelIsCollapsed;
                    newSettings.SortedModuleListInfo = oldSettings.SortedModuleListInfo;
                    newSettings.AutoResizeColumns = oldSettings.AutoResizeColumns;

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(newSettings, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "HQFSettings.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "HQFSettings.bin"), "OldHQFSettings.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF main settings: " + e.Message);
            }
        }

        /// <summary>The convert defence profiles.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertDefenceProfiles(string hqfFolder)
        {
            try
            {
                SortedList oldProfiles = default(SortedList);

                // Check for the profiles file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "HQFDefenceProfiles.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "HQFDefenceProfiles.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldProfiles = (SortedList)f.Deserialize(s);
                    }

                    var newProfiles = new SortedList<string, HQFDefenceProfile>();
                    foreach (DefenceProfile profile in oldProfiles.Values)
                    {
                        var newProfile = new HQFDefenceProfile();
                        newProfile.Name = profile.Name;
                        newProfile.Type = (ProfileTypes)profile.Type;
                        newProfile.SEm = profile.SEM;
                        newProfile.SExplosive = profile.SExplosive;
                        newProfile.SKinetic = profile.SKinetic;
                        newProfile.SThermal = profile.SThermal;
                        newProfile.AEm = profile.AEM;
                        newProfile.AExplosive = profile.AExplosive;
                        newProfile.AKinetic = profile.AKinetic;
                        newProfile.AThermal = profile.AThermal;
                        newProfile.HEm = profile.HEM;
                        newProfile.HExplosive = profile.HExplosive;
                        newProfile.HKinetic = profile.HKinetic;
                        newProfile.HThermal = profile.HThermal;
                        newProfile.DPS = profile.DPS;
                        newProfile.Fitting = profile.Fitting;
                        newProfile.Pilot = profile.Pilot;
                        newProfiles.Add(newProfile.Name, newProfile);
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(newProfiles, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "HQFDefenceProfiles.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "HQFDefenceProfiles.bin"), "OldHQFDefenceProfiles.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF defence profiles: " + e.Message);
            }
        }

        /// <summary>The convert damage profiles.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertDamageProfiles(string hqfFolder)
        {
            try
            {
                SortedList oldProfiles = default(SortedList);

                // Check for the profiles file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "HQFProfiles.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "HQFProfiles.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        oldProfiles = (SortedList)f.Deserialize(s);
                    }

                    var newProfiles = new SortedList<string, HQFDamageProfile>();
                    foreach (DamageProfile profile in oldProfiles.Values)
                    {
                        var newProfile = new HQFDamageProfile();
                        newProfile.Name = profile.Name;
                        newProfile.Type = (ProfileTypes)profile.Type;
                        newProfile.EM = profile.EM;
                        newProfile.Explosive = profile.Explosive;
                        newProfile.Kinetic = profile.Kinetic;
                        newProfile.Thermal = profile.Thermal;
                        newProfile.DPS = profile.DPS;
                        newProfile.Fitting = profile.Fitting;
                        newProfile.Pilot = profile.Pilot;
                        newProfiles.Add(newProfile.Name, newProfile);
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(newProfiles, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "HQFDamageProfiles.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old settings file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "HQFProfiles.bin"), "OldHQFProfiles.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF damage profiles: " + e.Message);
            }
        }

        /// <summary>The convert saved fittings.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertSavedFittings(string hqfFolder)
        {
            try
            {
                SortedList<string, SavedFitting> fittings = default(SortedList<string, SavedFitting>);

                // Check for the fittings file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "Fittings.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "Fittings.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        fittings = (SortedList<string, SavedFitting>)f.Deserialize(s);
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(fittings, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "Fittings.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old fittings file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "Fittings.bin"), "OldFittings.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF fittings: " + e.Message);
            }
        }

        /// <summary>The convert pilots.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertPilots(string hqfFolder)
        {
            try
            {
                var oldPilots = new SortedList();

                if (File.Exists(Path.Combine(hqfFolder, "HQFPilotSettings.bin")))
                {
                    try
                    {
                        using (var s = new FileStream(Path.Combine(hqfFolder, "HQFPilotSettings.bin"), FileMode.Open))
                        {
                            var f = new BinaryFormatter();
                            oldPilots = (SortedList)f.Deserialize(s);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    var newPilots = new SortedList<string, FittingPilot>();
                    foreach (HQFPilot pilot in oldPilots.Values)
                    {
                        newPilots.Add(pilot.PilotName, ConvertPilot(pilot));
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(newPilots, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "HQFPilotSettings.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old fittings file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "HQFPilotSettings.bin"), "OldHQFPilotSettings.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF pilots: " + e.Message);
            }
        }

        /// <summary>The convert pilot.</summary>
        /// <param name="oldPilot">The old pilot.</param>
        /// <returns>The <see cref="FittingPilot"/>.</returns>
        private FittingPilot ConvertPilot(HQFPilot oldPilot)
        {
            var newPilot = new FittingPilot();
            newPilot.PilotName = oldPilot.PilotName;
            newPilot.SkillSet = new Dictionary<string, FittingSkill>();
            foreach (HQFSkill skill in oldPilot.SkillSet)
            {
                var newSkill = new FittingSkill();
                newSkill.ID = Convert.ToInt32(skill.ID);
                newSkill.Name = skill.Name;
                newSkill.Level = skill.Level;
                newPilot.SkillSet.Add(newSkill.Name, newSkill);
            }

            for (int implant = 0; implant <= 10; implant++)
            {
                newPilot.ImplantName(implant) = oldPilot.ImplantName(implant);
            }

            return newPilot;
        }

        /// <summary>The convert custom ship classes.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertCustomShipClasses(string hqfFolder)
        {
            try
            {
                SortedList<string, CustomShipClass> custom = default(SortedList<string, CustomShipClass>);

                // Check for the fittings file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "CustomShipClasses.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "CustomShipClasses.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        custom = (SortedList<string, CustomShipClass>)f.Deserialize(s);
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(custom, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "CustomShipClasses.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "CustomShipClasses.bin"), "OldCustomShipClasses.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF custom ship classes: " + e.Message);
            }
        }

        /// <summary>The convert custom ships.</summary>
        /// <param name="hqfFolder">The hqf folder.</param>
        private void ConvertCustomShips(string hqfFolder)
        {
            try
            {
                SortedList<string, CustomShip> custom = default(SortedList<string, CustomShip>);

                // Check for the fittings file so we can load it
                if (File.Exists(Path.Combine(hqfFolder, "CustomShips.bin")))
                {
                    using (var s = new FileStream(Path.Combine(hqfFolder, "CustomShips.bin"), FileMode.Open))
                    {
                        var f = new BinaryFormatter();
                        custom = (SortedList<string, CustomShip>)f.Deserialize(s);
                    }

                    // Create a JSON string for writing
                    string json = JsonConvert.SerializeObject(custom, Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON version of the settings
                    try
                    {
                        using (var s = new StreamWriter(Path.Combine(hqfFolder, "CustomShips.json"), false))
                        {
                            s.Write(json);
                            s.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    // Rename the old file
                    My.Computer.FileSystem.RenameFile(Path.Combine(hqfFolder, "CustomShips.bin"), "OldCustomShips.bin");
                }
            }
            catch (Exception e)
            {
                _worker.ReportProgress(0, "Error converting HQF custom ships: " + e.Message);
            }
        }

        #endregion
    }
}