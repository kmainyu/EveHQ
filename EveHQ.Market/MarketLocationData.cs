// -----------------------------------------------------------------------
// <copyright file="MarketLocationData.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.Market
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MarketLocationData
    {
        public int LocationId { get; set; }
        public DateTimeOffset Freshness { get; set; }
        public string PathFromRoot { get; set; }
    }
}
