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

namespace EveHQ.PosManager
{
    [Serializable]
    class ReactionLink
    {
        public long LinkID;
        public decimal InpID, OutID;
        public decimal XferQty;
        public decimal XferVol;
        public string srcNm, dstNm;
        public Color LinkColor;

        public ReactionLink()
        {
            LinkID = 0;
            InpID = 0;
            OutID = 0;
            XferQty = 0;
            XferVol = 0;
            srcNm = "";
            dstNm = "";
            LinkColor = SystemColors.Control;
        }

        public ReactionLink(ReactionLink rl)
        {
            LinkID = rl.LinkID;
            InpID = rl.InpID;
            OutID = rl.OutID;
            XferQty = rl.XferQty;
            XferVol = rl.XferVol;
            srcNm = rl.srcNm;
            dstNm = rl.dstNm;
            LinkColor = rl.LinkColor;
        }

        public ReactionLink(long lid, decimal iid, decimal oid, decimal xq, decimal xv, Color lc, string sn, string dn)
        {
            LinkID = lid;
            InpID = iid;
            OutID = oid;
            XferQty = xq;
            XferVol = xv;
            srcNm = sn;
            dstNm = dn;
            LinkColor = lc;
        }

    }
}
