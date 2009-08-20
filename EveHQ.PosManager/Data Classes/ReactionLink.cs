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
        public int LinkID;
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

        public ReactionLink(int lid, decimal iid, decimal oid, decimal xq, decimal xv, Color lc, string sn, string dn)
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
