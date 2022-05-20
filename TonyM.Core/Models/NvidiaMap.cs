namespace TonyM.Core.Models
{
    public class Root
    {
        public bool success { get; set; }
        public List<ListMap> listMap { get; set; }
    }

    public class ListMap
    {
        public string is_active { get; set; }
        public string product_url { get; set; }
        public string price { get; set; }
        public string fe_sku { get; set; }
        public string locale { get; set; }
    }


}
