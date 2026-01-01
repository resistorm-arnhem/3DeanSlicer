using _3DeanSlicer.Console.Xyz;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace _3DeanSlicer.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string path = "C:\\Users\\muld0324.SKYNET\\3D Prints\\_Slicer Project\\Test Models\\Phone stand\\3w Original Files";
            string path = "C:\\Users\\muld0324.SKYNET\\3D Prints\\_Slicer Project\\Test Models\\Phone stand\\3w Original Files\\Converted Gcode\\";
            //ToGCodeWriter.ExportDirectory(path, dirOut, false);

            string path2 = "C:\\Users\\muld0324.SKYNET\\3D Prints\\_Slicer Project\\Test Models\\Phone stand\\3w Original Files";

            Dictionary<string, string> fileHeaders = new();
            Dictionary<string, int> lines3w = new();
            Dictionary<string, int> linesGcode = new();


            //foreach (string file in Directory.GetFiles(path))
            //{
            //    if (file.EndsWith(".gcode"))
            //    {
            //        string[] lines = File.ReadAllLines(file);
            //        string key = Path.GetFileNameWithoutExtension(file);
            //        linesGcode[key] = lines.Length;
            //    }
            //}

            string search = "print_time";
            foreach (string file in Directory.GetFiles(path2))
            {
                if (file.EndsWith(".3w"))
                {
                    string[] lines = File.ReadAllLines(file);
                    int count = 0;
                    while (count < lines.Length) {
                        string line = lines[count];
                        Debug.WriteLine($"{lines[count]}");
                        if (line.Contains(search))
                        {
                            Debug.WriteLine($"offset @ {count}");
                            break;
                        }
                        count++;
                    }
                    string key = Path.GetFileNameWithoutExtension(file); 
                    lines3w[key] = lines.Length;
                }
            }

            


        }


        static byte[] DecompressZLib(byte[] data)
        {
            using var input = new MemoryStream(data, 2, data.Length - 2);
            using var deflate = new DeflateStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();

            deflate.CopyTo(output);
            return output.ToArray();
        }
    }
}
