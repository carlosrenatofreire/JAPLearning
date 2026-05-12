namespace MundoDev.Helper.StaticVariables
{
    public static class MessageHelper
    {
        // ─── Erros de Criptografia (AesCryptoService) ─────────────────────
        public static string AesKeyInvalid()
            => $"AES key must be {GeneralVariableHelper.AesKeyRequiredBytes} bytes (256 bits).";

        public static string HmacKeyInvalid()
            => $"HMAC key must be at least {GeneralVariableHelper.HmacKeyMinBytes} bytes.";

        public static string EncryptedDataInvalid()
            => "Invalid encrypted data format.";

        public static string HmacValidationFailed()
            => "HMAC validation failed.";

    }
}
