using System;
using System.IO;
using System.Linq;

namespace Zadanie2
{
    internal class Program
    {
        static bool ReplaceWord(ref string text)
        {
            var textList = text.Split(' ', ',','\n');

            if (textList.Count(w => w == "praca") == 5)
            {
                text = text.Replace("praca", "job");
                return true;
            }

            return false;
        }

        static void Main(string[] args)
        {
            string path = @"c:/test/test_adam_cieslak.txt";

            try
            {
                var document1Text = File.ReadAllText(path);

                if (ReplaceWord(ref document1Text))
                {
                    string oldFileName = Path.GetFileNameWithoutExtension(path);
                    string newPath = path.Replace(oldFileName, $"{oldFileName}_changed-{DateTime.Now.ToString("yyyy_MM_dd")}");

                    using (var newFile = File.CreateText(newPath))
                    {
                        newFile.Write(document1Text);
                    }
                    File.Delete(path);
                    Console.WriteLine("succes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
