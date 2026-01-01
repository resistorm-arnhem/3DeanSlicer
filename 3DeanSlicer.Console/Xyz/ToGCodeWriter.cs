using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _3DeanSlicer.Console.Xyz
{
    public static class ToGCodeWriter
    {


        public static void ExportDirectory(string path, string outputDir, bool exportHexDumps) { 
            foreach(string file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".3w"))
                {
                    string pathOut = Path.Combine(outputDir, Path.GetFileName(file) + ".gcode");
                    ExportAsGCode(file, pathOut);
                    if (exportHexDumps) {
                        ExportAsHexDump(File.ReadAllBytes(file), CreateHexDumpFileName(file, outputDir));
                    }
                }
            }
        }

        public static string CreateHexDumpFileName(string pathIn, string dirOut) { 
            string filename = Path.GetFileNameWithoutExtension(pathIn);
            return Path.Combine(dirOut, filename + "3w.hexdump");
        }


        public static void ExportAsHexDump(string pathIn) { 
            string filename = Path.GetFileNameWithoutExtension(pathIn);
            string folderName = Path.GetDirectoryName(filename);
            string outputFile = Path.Combine(folderName, filename + ".hexdump");
            ExportAsHexDump(File.ReadAllBytes(pathIn), outputFile);
        }

        public static void ExportAsHexDump(string pathIn, string pathOut) {
            ExportAsHexDump(File.ReadAllBytes(pathIn), pathOut);
        }

        public static void ExportAsHexDump(byte[] bytes, string pathOut)
        {
            string raw = "";
            string line = "";
            int count = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = "0x" + bytes[i].ToString("X2");
                line += hex + " ";
                count++;
                if (count == 16)
                {
                    raw += line + "\n\r";
                    line = "";
                    count = 0;
                }
            }
            File.WriteAllText(pathOut, raw);
        }


        public static string ToGCode(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Xyz3wDecryptionService service = new Xyz3wDecryptionService("@xyzprinting.com@xyzprinting.com");
            string result = service.Decrypt(bytes);
            return result;
        }


        public static string ToGCode(byte[] bytes)
        {
            Xyz3wDecryptionService service = new Xyz3wDecryptionService("@xyzprinting.com@xyzprinting.com");
            string result = service.Decrypt(bytes);
            return result;
        }

        public static void ExportAsGCode(string pathIn, string pathOut)
        {
            string result = ToGCode(pathIn);
            File.WriteAllText(pathOut, result);
        }

        public static void ExportAsGCode(string pathIn)
        {
            string pathOut = pathIn + ".gcode";
            ExportAsGCode(pathIn, pathOut);
        }

    }
}
