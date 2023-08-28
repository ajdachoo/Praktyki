using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadanie3
{
    public class Person
    {
        public int Lp { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int YearOfBirth { get; set; }

        public override string ToString()
        {
            return $"{Lp} {Name} {Surname} {YearOfBirth}";
        }
    }
    internal class Program
    {
        static List<Person> GeneratePeople()
        {
            string[] names = { "Ania", "Kasia", "Basia", "Zosia" };
            string[] surnames = { "Kowalska", "Nowak" };
            var result = new List<Person>();
            Random random = new Random();

            for(int i = 0; i < 100; i++)
            {
                result.Add(new Person { Lp = i + 1, Name = names[random.Next(names.Length)], Surname = surnames[random.Next(surnames.Length)], YearOfBirth = random.Next(1990, 2000) });
            }

            return result;
        }
        static void Main(string[] args)
        {
            var people = GeneratePeople();

            using (var writer = new StreamWriter(@$"c:/test/users-{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(people);
            }
        }
    }
}
