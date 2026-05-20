namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface IAesCryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherBase64);
    }
}
