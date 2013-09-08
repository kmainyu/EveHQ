// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemAttribData.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines data on an Eve item attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    /// <summary>
    ///  Defines data on an Eve item attribute.
    /// </summary>
    public class ItemAttribData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemAttribData"/> class.
        /// </summary>
        /// <param name="attId">
        /// The attribute ID.
        /// </param>
        /// <param name="attValue">
        /// The attribute value.
        /// </param>
        /// <param name="attDisplayName">
        /// The attribute display name.
        /// </param>
        /// <param name="attUnit">
        /// The attribute unit name.
        /// </param>
        public ItemAttribData(int attId, double attValue, string attDisplayName, string attUnit)
        {
            this.Id = attId;
            this.Value = attValue;
            this.DisplayName = attDisplayName;
            this.Unit = attUnit;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the display value.
        /// </summary>
        public string DisplayValue { get; set; } 
    }
}
