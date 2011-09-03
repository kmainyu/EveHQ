// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
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

namespace EveHQ.RouteMap
{
    [Serializable]
    public class MapColors
    {
        public ArrayList SecurityColors;

        public MapColors()
        {
            SecurityColors = new ArrayList();
            SetSecurityColors();
        }

        public void SetSecurityColors()
        {
            SecurityColors.Add(Color.FromArgb(139, 0, 0));
            SecurityColors.Add(Color.FromArgb(209, 134, 0));
            SecurityColors.Add(Color.FromArgb(255, 140, 0));
            SecurityColors.Add(Color.FromArgb(255, 185, 0));
            SecurityColors.Add(Color.FromArgb(255, 215, 0));
            SecurityColors.Add(Color.FromArgb(255, 255, 0));
            SecurityColors.Add(Color.FromArgb(215, 255, 0));
            SecurityColors.Add(Color.FromArgb(150, 255, 0));
            SecurityColors.Add(Color.FromArgb(100, 255, 0));
            SecurityColors.Add(Color.FromArgb(135, 206, 250));
            SecurityColors.Add(Color.FromArgb(0, 255, 255));
        }

    }
}
