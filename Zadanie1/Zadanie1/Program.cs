using System;
using System.IO;
using System.Text;

namespace Zadanie1
{
    internal class Program
    {
        static int Counter(string text, char c)
        {
            int result = 0;
            for(int i = 0; i < text.Length; i++)
            {
                if (text[i] == c)
                {
                    result++;
                }
            }

            return result;
        }
        static void Main(string[] args)
        {
            string path = @"c:/test/test_adam_cieslak.txt";

            try
            {
                var document1 = File.ReadAllText(path);
                Console.WriteLine(Counter(document1, 'a'));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
