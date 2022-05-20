namespace TonyM.Core.Events
{
    public class ProductEventArgs : EventArgs
    {
        public ProductEventArgs(string link, string name)
        {
            this.Link = link;
            this.Name = name;
        }

        public string Link { get; set; }
        public string Name { get; set; }
    }
}
