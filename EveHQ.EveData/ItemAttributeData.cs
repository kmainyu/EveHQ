namespace EveHQ.EveData
{
    public class ItemAttributeData
    {
        public int ID { get; set; }
        public double Value { get; set; }
        public string DisplayName { get; set; }
        public string Unit { get; set; }
        public string DisplayValue { get; set; }
        
        public ItemAttributeData(int attID, double attValue, string attDisplayName, string attUnit)
        {
            ID = attID;
            Value = attValue;
            DisplayName = attDisplayName;
            Unit = attUnit;
        }
    }
}
