namespace TonyM.BLL.Events
{
    public class ProductBLEventArgs : EventArgs
    {
        public ProductBLEventArgs(string link)
        {
            this.link = link;
        }

        public string link { get; set; }
    }
}
