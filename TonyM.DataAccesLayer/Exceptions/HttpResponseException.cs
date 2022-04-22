namespace TonyM.DAL.Exceptions
{
    public class HttpResponseException : System.Exception
    {
        public HttpResponseException()
        {
        }

        public HttpResponseException(string reference) : base($"La Requête HTTP sur le produit {reference} a échoué, maj impossible")
        {
        }
    }
}
