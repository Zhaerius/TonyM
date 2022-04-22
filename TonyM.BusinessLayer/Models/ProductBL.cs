using TonyM.BLL.Events;
using TonyM.BLL.Exceptions;

namespace TonyM.BLL
{
    public class ProductBL
    {
        private string _Reference;
        private string _Localisation;

        public ProductBL(string reference, string localisation)
        {
            this.Reference = reference;
            this.Localisation = localisation;   
        }

        public event EventHandler<ProductBLEventArgs> OnAvailable;
        public string Reference {
            get
            {
                return _Reference;
            }
            init
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new EmptyFieldException();
                }
                _Reference = value;
            }
        }
        public string? Name 
        {
            get
            {
                if (this.Reference.StartsWith("NVGFT"))
                {
                    return this.Reference.Replace("NVGFT", "RTX 3").Replace("0T", "0 Ti");
                }
                return this.Reference;
            }
        }
        public string? BuyLink { get; set; }
        public bool InStock { get; set; } = false;
        public string Localisation {
            get
            {
                return _Localisation;
            }
            init
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new EmptyFieldException();
                }
                _Localisation = value;
            }
        }
        public DateTime? LastDetected { get; set; }


        public void VerificationStock(string? oldLink)
        {
            if (this.InStock && this.BuyLink != null && this.BuyLink != oldLink)
            {
                this.LastDetected = DateTime.Now;

                if (OnAvailable != null)
                {                
                    OnAvailable(this, new ProductBLEventArgs(this.BuyLink));
                }
            }
        }
    }
}