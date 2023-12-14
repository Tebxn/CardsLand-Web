namespace CardsLand_Web.Interfaces
{
    public interface ITools
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
        void AddError(string errorMessage);
    }
}
