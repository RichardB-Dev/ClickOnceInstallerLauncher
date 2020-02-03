using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace ClickOnce_Installer_Launcher
{
    class Program
    {
        static void Main(string[] args)
        {                       
            string ProjectName = "ExampleProject";

            // Add Click Once installer files to a zip folder named Setup + Change resource to Embdedded
            // var ResourceList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string resourceLocation = "ClickOnce_Installer_Launcher.Resources.Setup.zip";

            string folderLocation = AppDomain.CurrentDomain.BaseDirectory;
            string outputDirectory = folderLocation + @"Setup_" + ProjectName + ".zip";
            string extractedDirectory = folderLocation + @"Setup_" + ProjectName;
            
            //Copy resource Zip folder to current location
            using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocation))
            {
                using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(outputDirectory), System.IO.FileMode.Create))
                {
                    for (int i = 0; i < stream.Length; i++)
                    {
                        fileStream.WriteByte((byte)stream.ReadByte());
                    }
                    fileStream.Close();
                }
            }

            if (Directory.Exists(extractedDirectory))
            {
                DeleteDirectory(extractedDirectory);
            }
            //Extract Zip file
            ZipFile.ExtractToDirectory(outputDirectory, extractedDirectory);

            //Delete Zip File
            File.Delete(outputDirectory);
            
            //Run Click Once installer
            Process.Start(extractedDirectory + "\\setup.exe");

        }

        public static void DeleteDirectory(string deleteDirectory)
        {
            string[] files = Directory.GetFiles(deleteDirectory);
            string[] dirs = Directory.GetDirectories(deleteDirectory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(deleteDirectory, false);
        }
    }
}
