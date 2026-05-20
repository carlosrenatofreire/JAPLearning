using Microsoft.Extensions.Options;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Models.Settings;
using JAPLearning.Helper.StaticVariables;
using System.Security.Cryptography;
using System.Text;

namespace JAPLearning.Business.Services.Internals.Shareds
{
    public class AesCryptoService : IAesCryptoService
    {
        private readonly byte[] _aesKey;
        private readonly byte[] _hmacKey;

        public AesCryptoService(IOptions<AesCryptoSetting> options)
        {
            var aesKeyBase64 = options.Value.AES_KEY_BASE64;
            var hmacKeyBase64 = options.Value.HMAC_KEY_BASE64;

            _aesKey = Convert.FromBase64String(aesKeyBase64);
            _hmacKey = Convert.FromBase64String(hmacKeyBase64);

            if (_aesKey.Length != GeneralVariableHelper.AesKeyRequiredBytes)
                throw new ArgumentException(MessageHelper.AesKeyInvalid());

            if (_hmacKey.Length < GeneralVariableHelper.HmacKeyMinBytes)
                throw new ArgumentException(MessageHelper.HmacKeyInvalid());
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            using var encryptor = aes.CreateEncryptor();
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var ivAndCipher = aes.IV.Concat(cipherBytes).ToArray();

            using var hmac = new HMACSHA256(_hmacKey);
            var tag = hmac.ComputeHash(ivAndCipher);

            var full = ivAndCipher.Concat(tag).ToArray();
            return Convert.ToBase64String(full);
        }

        public string Decrypt(string encryptedBase64)
        {
            var fullBytes = Convert.FromBase64String(encryptedBase64);

            if (fullBytes.Length < GeneralVariableHelper.AesIvBytes + GeneralVariableHelper.HmacTagBytes)
                throw new CryptographicException(MessageHelper.EncryptedDataInvalid());

            var iv = fullBytes[..16];
            var tag = fullBytes[^32..];
            var cipherBytes = fullBytes[16..^32];

            using var hmac = new HMACSHA256(_hmacKey);
            var computedTag = hmac.ComputeHash(iv.Concat(cipherBytes).ToArray());

            if (!CryptographicOperations.FixedTimeEquals(tag, computedTag))
                throw new CryptographicException(MessageHelper.HmacValidationFailed());

            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }

}
