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

namespace EveHQ.PI
{
    [Serializable]
    public class PINFacility
    {
        public CommandCenter Command;
        public SortedList<string, Extractor> Extractors;
        public SortedList<string, ExtControlUnit> ECUs;
        public SortedList<string, Launchpad> LaunchPad;
        public SortedList<string, Processor> Processors;  // Key = Processor Name
        public SortedList<string, StorageFacility> Storage;
        public ArrayList Links;
        public string Name;
        public string SystemName;
        public string PlanetName;
        public string PlanetType;
        public int SystemID;
        public int PlanetID;
        public double CPU;
        public double Power;
        public double StoreCap;
        public bool inOverview;
        public decimal AvgLinkLength;
        public decimal NumLinks;
        public int numMods, OVQty;
        public bool Converted = false;

        public PINFacility()
        {
            Command = new CommandCenter();
            Extractors = new SortedList<string, Extractor>();
            ECUs = new SortedList<string, ExtControlUnit>();
            LaunchPad = new SortedList<string, Launchpad>();
            Processors = new SortedList<string, Processor>();
            Storage = new SortedList<string, StorageFacility>();
            Links = new ArrayList();
            Name = "";
            SystemName = "";
            PlanetName = "";
            PlanetType = "";
            SystemID = 0;
            PlanetID = 0;
            CPU = 0;
            Power = 0;
            StoreCap = 0;
            inOverview = false;
            AvgLinkLength = 0;
            NumLinks = 0;
            numMods = 0;
            OVQty = 1;
            Converted = false;
        }

        public PINFacility(PINFacility p)
        {
            Command = new CommandCenter(p.Command);
            LaunchPad = new SortedList<string, Launchpad>();
            foreach (var lp in p.LaunchPad)
                LaunchPad.Add(lp.Key, new Launchpad(lp.Value));

            Extractors = new SortedList<string, Extractor>();
            foreach (var ex in p.Extractors)
                Extractors.Add(ex.Key, new Extractor(ex.Value));

            ECUs = new SortedList<string, ExtControlUnit>();
            if (p.ECUs != null)
            {
                foreach (var ecu in p.ECUs)
                    ECUs.Add(ecu.Key, new ExtControlUnit(ecu.Value));
            }

            Processors = new SortedList<string, Processor>();
            if (p.Processors != null)
                foreach (var pr in p.Processors)
                    Processors.Add(pr.Key, new Processor(pr.Value));

            Storage = new SortedList<string, StorageFacility>();
            foreach (var sf in p.Storage)
                Storage.Add(sf.Key, new StorageFacility(sf.Value));

            Links = new ArrayList();
            foreach (Link l in p.Links)
                Links.Add(new Link(l));

            Name = p.Name;
            SystemName = p.SystemName;
            PlanetName = p.PlanetName;
            PlanetType = p.PlanetType;
            SystemID = p.SystemID;
            PlanetID = p.PlanetID;
            CPU = p.CPU;
            Power = p.Power;
            StoreCap = p.StoreCap;
            inOverview = p.inOverview;
            AvgLinkLength = p.AvgLinkLength;
            NumLinks = p.NumLinks;
            numMods = p.numMods;
            OVQty = p.OVQty;
            Converted = p.Converted;
        }

    }
}
