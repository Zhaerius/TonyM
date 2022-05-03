namespace TonyM.DAL.Exceptions
{
    public class DeserializeException : Exception
    {
        public DeserializeException()
        {
        }

        public DeserializeException(string reference) : base($"Impossible de déserialiser la référence {reference}, elle n'existe pas chez Nvidia")
        {
        }
    }
}
