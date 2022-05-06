using System.Text.RegularExpressions;
using TonyM.BLL.Events;
using TonyM.BLL.Exceptions;

namespace TonyM.BLL.Models
{
    public class ProductBL
    {
        private string _reference;
        private string _localisation;
        private string _buyLink;

        public ProductBL(string reference, string localisation)
        {
            this.Reference = reference;
            this.Localisation = localisation;
        }

        public event EventHandler<ProductBLEventArgs> OnAvailable;
        public string Reference
        {
            get
            {
                return _reference;
            }
            init
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new EmptyFieldException();
                }
                _reference = value;
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
        public string? BuyLink
        {
            get
            {
                if (!string.IsNullOrEmpty(_buyLink))
                {
                    return Regex.Replace(_buyLink, @"(?<=html)[^\]]+", "");
                }
                return _buyLink;
            }
            set
            {
                _buyLink = value;
            }
        }

        public bool InStock { get; set; } = false;
        public string Localisation
        {
            get
            {
                return _localisation;
            }
            init
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new EmptyFieldException();
                }
                _localisation = value;
            }
        }
        public DateTime? LastDetected { get; set; }


        public void VerificationStock(string? oldLink)
        {
            if (this.InStock && !string.IsNullOrEmpty(this.BuyLink) && this.BuyLink != oldLink)
            {
                this.LastDetected = DateTime.Now;

                if (OnAvailable != null)
                {
                    OnAvailable(this, new ProductBLEventArgs(this.BuyLink, this.Name));
                }
            }
        }
    }
}