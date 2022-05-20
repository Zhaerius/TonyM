namespace TonyM.Core.Exceptions
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException() : base("Un champ est vide, vérifier le UserSettings.json")
        {

        }
    }
}
