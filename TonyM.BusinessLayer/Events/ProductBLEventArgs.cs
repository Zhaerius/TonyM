namespace TonyM.BLL.Events
{
    public class ProductBLEventArgs : EventArgs
    {
        public ProductBLEventArgs(string link, string name)
        {
            this.Link = link;
            this.Name = name;
        }

        public string Link { get; set; }
        public string Name { get; set; }
    }
}
