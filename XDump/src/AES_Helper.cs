using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace XDump.src
{
    public class AES
    {
        
        public static object DecryptAES(byte[] key, string data)
        {
            // Create cipher object
            RijndaelManaged AesCipher = new RijndaelManaged();
            
            // Set the key and mode
            AesCipher.Key  = key;
            AesCipher.Mode = CipherMode.ECB;
            
            // Create decryption object
            ICryptoTransform Decryptor = AesCipher.CreateDecryptor();
            
            // Attempt decryption and return cipher-text if decryption fails
            try
            {
                    byte[] DecodedBase64 = Convert.FromBase64String(data);
                    return AES.ConvertToString(Decryptor.TransformFinalBlock(DecodedBase64, 0, DecodedBase64.Length));
            }
            catch (Exception ex) 
            {
                return data;
            }
        }

        private static string ConvertToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] ConvertToBytes(string data)
        { 
            return Encoding.UTF8.GetBytes(data);
        }
    }
}
