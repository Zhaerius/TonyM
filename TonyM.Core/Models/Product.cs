using System.Text.RegularExpressions;
using TonyM.Core.Events;
using TonyM.Core.Exceptions;

namespace TonyM.Core.Models
{
    public class Product
    {
        private string _reference;
        private string _localisation;
        private string _buyLink;

        public Product(string reference, string localisation)
        {
            this.Reference = reference;
            this.Localisation = localisation;   
        }

        public event EventHandler<ProductEventArgs> OnAvailable;
        public string Reference {
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
        public string? BuyLink { 
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
        public string Localisation {
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
                    OnAvailable(this, new ProductEventArgs(this.BuyLink, this.Name));
                }
            }
        }

    }
}