using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel.EPPlus;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Zadanie1_MVC.Models;

namespace Zadanie1_MVC.Controllers
{
    public class TestController : Controller
    {
        private readonly TestContext _dbContext;

        public TestController(TestContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var clients = _dbContext.Klienci.ToList();
            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Klient client)
        {
            if (ModelState.IsValid)
            {
                client.Płeć = client.Pesel[client.Pesel.Length - 2] % 2 == 0 ? 0 : 1;
                client.BirthYear = GetYearFromPesel(client.Pesel);

                _dbContext.Add(client);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
            
        }

        public IActionResult Edit(int id)
        {
            var client = _dbContext.Klienci.FirstOrDefault(x => x.Id == id);
            if(client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Klient klient)
        {
            if (ModelState.IsValid)
            {
                klient.Płeć = klient.Pesel[klient.Pesel.Length - 2] % 2 == 0 ? 0 : 1;
                klient.BirthYear = GetYearFromPesel(klient.Pesel);

                _dbContext.Update(klient);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(klient);
        }

        public IActionResult Delete(int id)
        {
            var client = _dbContext.Klienci.FirstOrDefault(x => x.Id == id);

            if(client == null)
            {
                return NotFound();
            }
            else
            {
                _dbContext.Klienci.Remove(client);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ImportFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFile(ImportFileForm file)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var clients = GetClientsFromFile(file.FormFile);

            foreach(var client in clients)
            {
                client.Id = 0;
            }

            _dbContext.AddRange(clients);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        private IEnumerable<Klient> GetClientsFromFile(IFormFile file)
        {
            if(file.ContentType == "text/csv")
            {
                using (var fileReader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(fileReader, CultureInfo.InvariantCulture))
                {
                    var clients = csv.GetRecords<Klient>();
                    return clients.ToList();
                }
            }
            else
            {   //XLSX Nie działa 
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                config.TrimOptions = TrimOptions.Trim;
                config.HasHeaderRecord = true;

                using (var fileReader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(fileReader, config))
                {
                    var clients = csv.GetRecords<Klient>().ToList();
                    return clients;
                }
            }
        }

        public IActionResult SaveClientsToCsv()
        {
            string fileName = $"clients-{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}";
            SaveClientsToCsv(fileName);

            var memory = DownloadSinghFile($"{fileName}.csv", @"wwwroot\\Files");
            return File(memory, "text/csv", $"{fileName}.csv");
        }

        public IActionResult SaveClientsToXlsx()
        {
            string fileName = $"clients-{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}";
            SaveClientsToXlsx(fileName);

            var memory = DownloadSinghFile($"{fileName}.xlsx", @"wwwroot\\Files");
            return File(memory, "text/xlsx", $"{fileName}.xlsx");
        }

        private void SaveClientsToXlsx(string fileName)
        {
            var clients = _dbContext.Klienci.ToList();
            using (var csv = new ExcelWriter(@$"wwwroot\\Files\{fileName}.xlsx"))
            {
                csv.WriteRecords(clients);
            }
        }

        private void SaveClientsToCsv(string fileName)
        {
            var clients = _dbContext.Klienci.ToList();
            using (var writer = new StreamWriter(@$"wwwroot\\Files\{fileName}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(clients);
            }
        }

        private MemoryStream DownloadSinghFile(string fileName, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, fileName);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }

            memory.Position = 0;
            return memory;
        }

        private int GetYearFromPesel(string pesel)
        {
            int yearBase = 1900;
            int centuryIndicator = int.Parse(pesel.Substring(2, 1));
            int yearDigit1 = int.Parse(pesel.Substring(0, 1));
            int yearDigit2 = int.Parse(pesel.Substring(1, 1));

            switch (centuryIndicator)
            {
                case 0:
                case 1:
                    yearBase += yearDigit1 * 10 + yearDigit2;
                    break;
                case 2:
                case 3:
                    yearBase += (yearDigit1 * 10 + yearDigit2) + 100;
                    break;
                case 4:
                case 5:
                    yearBase += (yearDigit1 * 10 + yearDigit2) + 200;
                    break;
                case 6:
                case 7:
                    yearBase += (yearDigit1 * 10 + yearDigit2) + 300;
                    break;
                case 8:
                case 9:
                    yearBase += (yearDigit1 * 10 + yearDigit2) + 400;
                    break;
            }

            return yearBase;
        }
    }
}
