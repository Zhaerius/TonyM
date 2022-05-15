namespace TonyM.DAL.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException()
        {
        }

        public HttpResponseException(string statutCode, string reference) : base($"{reference} : Statut code {statutCode}")
        {
        }
    }
}
