// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
// 
// This file is part of EveHQ.
//
// EveHQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    [Serializable]
    public class NotificationList
    {
        public ArrayList NotifyList;
        public bool mailSendErr = false;

        public NotificationList()
        {
            NotifyList = new ArrayList();
        }

        public void SaveNotificationList()
        {
            string PoSBase_Path, PoSManage_Path, PoSSave_Path, fname;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSSave_Path = Path.Combine(PoSManage_Path, "PoSData");

            if (!Directory.Exists(PoSSave_Path))
                Directory.CreateDirectory(PoSSave_Path);

            fname = Path.Combine(PoSSave_Path, "NotifyList.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, NotifyList);
            pStream.Close();
        }

        public void LoadNotificationList()
        {
            string PoSBase_Path, PoSManage_Path, PoSSave_Path, fname;
            Stream cStr;
            BinaryFormatter myBf;

            if (EveHQ.Core.HQ.IsUsingLocalFolders == false)
            {
                PoSBase_Path = EveHQ.Core.HQ.appDataFolder;
            }
            else
            {
                PoSBase_Path = Application.StartupPath;
            }
            PoSManage_Path = Path.Combine(PoSBase_Path, "PoSManage");
            PoSSave_Path = Path.Combine(PoSManage_Path, "PoSData");

            if (!Directory.Exists(PoSManage_Path))
                return;
            if (!Directory.Exists(PoSSave_Path))
                return;

            fname = Path.Combine(PoSSave_Path, "NotifyList.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    NotifyList = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

        public void CheckAndSendNotificationIfActive()
        {
            decimal ntfyHours, freqHrs;
            decimal ReactTime = 0;
            ArrayList ReactRet;
            DateTime cts;
            TimeSpan diff;

            if (mailSendErr)
                return;

            cts = DateTime.Now;

            foreach (New_POS p in PlugInData.PDL.Designs.Values)
            {
                foreach (PosNotify pn in PlugInData.NL.NotifyList)
                {
                    if (mailSendErr)
                        return;

                    if (p.Name == pn.Tower)
                    {
                        if (pn.Frequency == "Days")
                        {
                            freqHrs = pn.FreqQty * 24;
                        }
                        else
                        {
                            freqHrs = pn.FreqQty;
                        }
                        if (pn.Initial == "Days")
                        {
                            ntfyHours = pn.InitQty * 24;
                        }
                        else
                        {
                            ntfyHours = pn.InitQty;
                        }

                        if (pn.Type == "Fuel Level")
                        {
                            if (pn.Notify_Active)
                            {
                                if (ntfyHours > p.PosTower.Data["F_RunTime"])
                                {
                                    diff = cts.Subtract(pn.Notify_Sent);
                                    if (diff.Hours >= freqHrs)
                                    {
                                        if (SendNotification(pn, p))
                                            pn.Notify_Sent = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                            else
                            {
                                // Check to see if tower fuel run time is less than notification time
                                if (ntfyHours >= p.PosTower.Data["F_RunTime"])
                                {
                                    pn.Notify_Active = true;
                                    if (SendNotification(pn, p))
                                        pn.Notify_Sent = DateTime.Now;
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                        }
                        else if (pn.Type == "Silo Level")
                        {
                            ReactRet = PlugInData.GetLongestSiloRunTime(p);
                            ReactTime = (decimal)ReactRet[0];

                            if (pn.Notify_Active)
                            {
                                if (ntfyHours > ReactTime)
                                {
                                    diff = cts.Subtract(pn.Notify_Sent);
                                    if (diff.Hours >= freqHrs)
                                    {
                                        if (SendNotification(pn, p))
                                            pn.Notify_Sent = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                            else
                            {
                                // Check to see if tower fuel run time is less than notification time
                                if (ntfyHours >= ReactTime)
                                {
                                    pn.Notify_Active = true;
                                    if (SendNotification(pn, p))
                                        pn.Notify_Sent = DateTime.Now;
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool SendNotification(PosNotify pn, New_POS p)
        {
            string eServe, eAddy, eUser, ePass, paddStr = "", mailMsg = "", subject;
            string paddLine = "";
            bool useSMTP;
            int ePort;
            decimal timeP;
            decimal totVol = 0, totCost = 0;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;
            decimal ReactTime = 0;
            ArrayList ReactRet;
            System.Net.Mail.SmtpClient Notify = new System.Net.Mail.SmtpClient();
            System.Net.Mail.MailMessage NtfMsg;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }

            eServe = EveHQ.Core.HQ.EveHQSettings.EMailServer;
            ePort = EveHQ.Core.HQ.EveHQSettings.EMailPort;
            eAddy = EveHQ.Core.HQ.EveHQSettings.EMailAddress;
            useSMTP = EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth;
            eUser = EveHQ.Core.HQ.EveHQSettings.EMailUsername;
            ePass = EveHQ.Core.HQ.EveHQSettings.EMailPassword;

            Notify.Port = ePort;
            Notify.Host = eServe;
            Notify.EnableSsl = EveHQ.Core.HQ.EveHQSettings.UseSSL;

            if (useSMTP)
            {
                System.Net.NetworkCredential netCred = new System.Net.NetworkCredential();
                netCred.Password = ePass;
                netCred.UserName = eUser;
                Notify.Credentials = netCred;
            }

            if (pn.Type == "Fuel Level")
            {
                timeP = p.ComputePosFuelUsageForFillTracking(4, 0, PlugInData.Config.data.FuelCosts);
                p.PosTower.T_Fuel.SetCurrentFuelVolumes();
                p.PosTower.T_Fuel.SetCurrentFuelCosts(PlugInData.Config.data.FuelCosts);
                fVal = p.PosTower.T_Fuel.GetFuelBayTotals();

                mailMsg += "Tower   : " + p.Name + "\n";
                mailMsg += "Location: " + p.Moon + "\n";
                mailMsg += "Fuel Run Time Is: " + PlugInData.ConvertHoursToTextDisplay(p.PosTower.Data["F_RunTime"]) + "\n";
                mailMsg += "\nThe Tower Needs the Following Fuel for MAX Run Time:\n";
                mailMsg += "----------------------------------------------------\n\n";

                for (int x = 0; x < 3; x++)
                {
                    if ((!p.UseChart) && (x == 1))
                        continue;

                    if (Convert.ToDecimal(fVal[x, 1]) > 0)
                    {
                        paddStr = fVal[x, 0].PadRight(17, ' ');
                        paddLine = " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])).PadLeft(7) + " ]";
                        paddStr += paddLine;
                        paddLine = "[ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])).PadLeft(7) + "m3 ]";
                        paddStr += paddLine;
                        paddLine = "[ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])).PadLeft(11) + "isk ]\n";
                        paddStr += paddLine;
                        totVol += Convert.ToDecimal(fVal[x, 2]);
                        totCost += Convert.ToDecimal(fVal[x, 3]);
                        tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                        tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                        tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                        mailMsg += paddStr;
                    }
                }
                mailMsg += "Transport Volume  [ " + String.Format("{0:#,0.#}", totVol).PadLeft(12) + "m3 ]\n";
                mailMsg += "Total Fuel Cost   [ " + String.Format("{0:#,0.#}", totCost).PadLeft(11) + "isk ]\n";
                //Clipboard.SetText(mailMsg);
            }
            else if (pn.Type == "Silo Level")
            {
                ReactRet = PlugInData.GetLongestSiloRunTime(p);
                ReactTime = (decimal)ReactRet[0];

                mailMsg += "Tower   : " + p.Name + "\n";
                mailMsg += "Location: " + p.Moon + "\n";
                mailMsg += "\nThe Tower Needs the Silo's Dealt With!\n";
                mailMsg += "----------------------------------------------------\n\n";

                foreach (Module m in p.Modules)
                {
                    paddLine = "";
                    switch (Convert.ToInt64(m.ModType))
                    {
                        case 2:
                        case 8:
                        case 9:
                        case 11:
                        case 12:
                        case 13:
                            paddLine = m.selMineral.name + " Silo Needs attention in [ " + PlugInData.ConvertHoursToTextDisplay(m.FillEmptyTime) + " ]\n\n";
                            break;
                        default:
                            break;
                    }
                    mailMsg += paddLine;
                }
            }
            else
                return false;  // Unknown Type

            NtfMsg = new System.Net.Mail.MailMessage();
            NtfMsg.From = new System.Net.Mail.MailAddress(eAddy);
            foreach (Player pl in pn.PList.Players)
            {
                foreach (Player ply in PlugInData.PL.Players)
                {
                    if (pl.Name == ply.Name)
                    {
                        if (ply.Email1 != "")
                        {
                            NtfMsg.To.Add(ply.Email1);
                        }
                        if (ply.Email2 != "")
                        {
                            NtfMsg.To.Add(ply.Email2);
                        }
                        if (ply.Email3 != "")
                        {
                            NtfMsg.To.Add(ply.Email3);
                        }
                    }
                }
            }
            subject = "EveHQ PoSManager: " + pn.Type + " for '" + p.Name + "'";
            NtfMsg.Subject = subject;
            NtfMsg.Body = mailMsg;
            try
            {
                Notify.Send(NtfMsg);
            }
            catch
            {
                MessageBox.Show("Error during attempt to Send Mail\nCheck to make sure the Mail configuration for EveHQ\nis correct and that you have a good connection.\n\nYou will need to do a Manual Send (button)\nOr restart manager to re-enable Notification Mails.", "Mail Send Error");
                mailSendErr = true;
                return false;
            }
            return true;
        }


    }
}
