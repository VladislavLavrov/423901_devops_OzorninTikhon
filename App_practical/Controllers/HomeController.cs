using App_practical.Data;
using App_practical.Models;
using App_practical.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace App_practical.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        private readonly KafkaProducerService<Null,string> _producer;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, KafkaProducerService<Null,string> producer)
        {
            _logger = logger;
            _context = context;
            _producer = producer;
        }

        public IActionResult Index()
        {
            return View(_context.Variants.ToList());
        }

        [HttpGet]
        public IActionResult Calculate(int id)
        {
            Variant? variant = _context.Variants.Where(x => x.Id == id).FirstOrDefault();
            return View(variant);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromForm] VariantDataViewModel dataViewModel)
        {
            if (!double.TryParse(
                    dataViewModel.Value1.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double value1_double)
                || !double.TryParse(
                    dataViewModel.Value2.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double value2_double))
            {
                return ShowError("Было введено неверное число.");
            }

            Variant variant = new()
            {
                Name = dataViewModel.VariantName,
                Value1 = value1_double,
                Value2 = value2_double,
                Operation = dataViewModel.Operation[0],
            };

            // Отправка данных в Kafka
            await SendDataToKafka(variant);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Callback([FromBody] Variant variant)
        {
            _context.Variants.Add(variant);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Delete(int id)
        {
            Variant? variant = _context.Variants.Where(x => x.Id == id).FirstOrDefault();
            if (variant != null)
            {
                _context.Variants.Remove(variant);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        ViewResult ShowError(string message)
        {
            return View(new DataViewModel() { ErrorMessage = message });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task SendDataToKafka(Variant variant)
        {
            var json = JsonSerializer.Serialize(variant);
            await _producer.ProduceAsync("ozornin", new Message<Null, string>{ Value = json });
        }
    }
}
