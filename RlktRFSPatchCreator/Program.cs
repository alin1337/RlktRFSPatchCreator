using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace RlktRFSPatchCreator
{
   
    class Program
    {
        public static string GetMD5FromFile(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        static void InstallContextMenu()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\.lua", "", "RlktLua", RegistryValueKind.String);
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\RlktRFS\\shell\\RlktCreatePatch", "", "Create Patch", RegistryValueKind.String);
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\RlktRFS\\shell\\RlktCreatePatch\\command", "", string.Format("\"{0}\" \"%1\"", path), RegistryValueKind.String);
        }

        static void Main(string[] args)
        {
            InstallContextMenu();
            if (args.Length != 1)
                return;

            string PatchNumber = Microsoft.VisualBasic.Interaction.InputBox("Patch Number (e.g. 7)", "Create Patch", "");

            string file = args[0];
            string filename = Path.GetFileNameWithoutExtension(file);

            string outfile = string.Format("Patch{0,8:D8}", int.Parse(PatchNumber));

            File.WriteAllText(outfile + ".txt", "");
            File.WriteAllText(outfile + ".MD5", GetMD5FromFile(file));
            File.Copy(file , outfile + ".RFS");
        }
    }
}
