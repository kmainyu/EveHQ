﻿using System;
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
    public class PlayerList
    {
        public ArrayList Players;

        public PlayerList()
        {
            Players = new ArrayList();
        }

        public PlayerList(PlayerList pl)
        {
            Players = new ArrayList();
            foreach (Player p in pl.Players)
                Players.Add(p);
        }

        public void SavePlayerList()
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

            fname = Path.Combine(PoSSave_Path, "PlayerList.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, Players);
            pStream.Close();
        }

        public void LoadPlayerList()
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

            fname = Path.Combine(PoSSave_Path, "PlayerList.bin");
            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    Players = (ArrayList)myBf.Deserialize(cStr);
                    cStr.Close();
                }
                catch
                {
                    cStr.Close();
                }
            }
        }

    }
}
