using System.Security.Cryptography;
using System.Text;

namespace _3DeanSlicer.Console.Xyz
{
    public class Xyz3wDecryptionService
    {
        private readonly string _key;
        public Xyz3wDecryptionService(string key)
        {
            using var sha256 = SHA256.Create();
            _key = key;
        }

        public string Encrypt(byte[] plainBytes)
        {
            plainBytes = ApplyPkcs7Padding(plainBytes);
            using var aes = Aes.Create();
            aes.Key = Encoding.ASCII.GetBytes(_key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;

            using var msEncrypt = new MemoryStream();
            using var encryptor = aes.CreateEncryptor();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(plainBytes, 0, plainBytes.Length);
            csEncrypt.FlushFinalBlock();
            var encryptedData = Convert.ToBase64String(msEncrypt.ToArray());
            return encryptedData;
        }


        public string Decrypt(byte[] cypherBytes)
        {
            if (cypherBytes.Length < 16)
                throw new AggregateException("Invalid cypher");

            using var aes = Aes.Create();
            aes.Key = Encoding.ASCII.GetBytes(_key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;

            byte[] iv = new byte[16];
            Array.Copy(cypherBytes, 0, iv, 0, 16);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(cypherBytes, 16, cypherBytes.Length - 16);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var decryptedData = srDecrypt.ReadToEnd();
            return decryptedData;
        }

        public byte[] ApplyPkcs7Padding(byte[] data)
        {
            int blockSize = 16; // AES block size
            int paddingLength = blockSize - (data.Length % blockSize);
            if (paddingLength == 0) paddingLength = blockSize;
            byte paddingByte = (byte)paddingLength;
            return data.Concat(Enumerable.Repeat(paddingByte, paddingLength)).ToArray();
        }

    }
}
