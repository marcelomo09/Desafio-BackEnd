using System.Security.Cryptography;
using System.Text;

public static class CryptoSHA256
{
    public static string ConvertStringToSHA256(string value)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            
            StringBuilder builder = new StringBuilder();
            
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}