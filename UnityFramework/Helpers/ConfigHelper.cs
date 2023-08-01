using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnityFramework.Helpers
{
    class ConfigHelper
    {

        private static string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HackName\\");
        private static string GetConfigPath(string name = "Default")
        {
            return ConfigPath + name + ".cfg";
        }
        private static readonly string Hash = "Randomstring";
        private static string DecryptStatic(string text)
        {

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(text);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Hash, new byte[] { 0xfd, 0xef, 0x32, 0x4f, 0xfa, 0x66, 0xa7, 0x42, 0x57, 0x95, 0x64, 0x75, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        text = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return text;
        }
        private static string EncryptStatic(string text)
        {


            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Hash, new byte[] { 0xfd, 0xef, 0x32, 0x4f, 0xfa, 0x66, 0xa7, 0x42, 0x57, 0x95, 0x64, 0x75, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }
        public static void SaveConfig(string name = "Default")
        {
            string path = GetConfigPath(name);
            string json = JsonConvert.SerializeObject(Globals.Config, Formatting.Indented); // serialize it to the config format
            File.WriteAllText(path, EncryptStatic(json)); // save it encrypted by base64, helps against sigs

        }
        public static void LoadConfig(string name = "Default")
        {
            if (File.Exists(GetConfigPath(name)))
            {
                string json = DecryptStatic(File.ReadAllText(GetConfigPath(name))); // decrypt the config and read the contents
                Configs.Config s = JsonConvert.DeserializeObject<Configs.Config>(json);  // deserialize and cast it
                Globals.Config = s;

            }
        }
        public static void SetUp()
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }
            if (!File.Exists(GetConfigPath()))
                SaveConfig();
            else
                LoadConfig();
        }
    }
}
