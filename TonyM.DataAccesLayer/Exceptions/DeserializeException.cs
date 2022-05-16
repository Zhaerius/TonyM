namespace TonyM.DAL.Exceptions
{
    public class DeserializeException : Exception
    {
        public DeserializeException()
        {
        }

        public DeserializeException(string reference) : base($"{reference} n'existe pas")
        {
        }
    }
}
