using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using XDump.src;

namespace XDump
{
    internal class XDump
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Properties.Resources.Banner);

            // Check program argument length
            if (args.Length != 1 && args.Length != 2)
            {
                Console.WriteLine($"Too many arguments or not enough provided.\n{Properties.Resources.Usage}\n");
                Environment.Exit(2);
            }

            // Load the target assembly from the filepath argument provided 
            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(args[0]);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error on Assembly Load: {ex.Message}");
                Environment.Exit(3);
            }

            // Begin key derivation by retrieving mutex.
            //
            string mutex = null;
            
            // 2nd argument is a mutex, set mutex to user argument else retrieve mutex from expected location
            if (args.Length == 2)
            {
                mutex = args[1];
            }
            else if (args.Length == 1)
            {
                mutex = src.MetadataParser.GetMutex(args[0], assembly);
                if (mutex == null)
                {
                    Console.WriteLine("Failed to capture Mutex. Quitting.");
                    Environment.Exit(0);
                }
            }

            // Init Key array and create MD5 hash from Mutex
            MD5CryptoServiceProvider MD5Provider = new MD5CryptoServiceProvider();
            byte[] AesDecryptionKey = new byte[32];
            byte[] MutexMD5Hash = MD5Provider.ComputeHash(AES.ConvertToBytes(mutex));

            // Copy the hash into the key byte array
            Array.Copy(MutexMD5Hash, 0, AesDecryptionKey, 0, 16);
            Array.Copy(MutexMD5Hash, 0, AesDecryptionKey, 15, 16);

            // Loop through config enum and dump configuration
            foreach (XWormConfiguration setting in Enum.GetValues(typeof(XWormConfiguration))) {
                
                int token = (int)setting;
                string name = setting.ToString();

                if (name != "MUTEX")
                {
                    //Console.WriteLine($"Config Name: {name}");
#if DEBUG
                    Console.WriteLine($"Token: 0x{token.ToString("X")}");

#endif
                    var FieldInfo = src.MetadataParser.GetFieldData(args[0], (uint)setting, assembly);
                    if (FieldInfo != null)
                    {
                        Console.WriteLine($"Decrypted Data: {AES.DecryptAES(AesDecryptionKey, FieldInfo)}\n");
                    }
                }
            }
        }
    }
}
