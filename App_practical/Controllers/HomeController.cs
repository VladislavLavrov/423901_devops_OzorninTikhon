using App_practical.Data;
using App_practical.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace App_practical.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
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
        public IActionResult Calculate([FromForm] VariantDataViewModel dataViewModel)
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

            double result = 0.0;

            switch (dataViewModel.Operation)
            {
                case "+":
                    result = value1_double + value2_double;
                    break;
                case "-":
                    result = value1_double - value2_double;
                    break;
                case "*":
                    result = value1_double * value2_double;
                    break;
                case "/":
                    if (value2_double == 0.0)
                    {
                        return ShowError("Деление на ноль.");
                    }
                    result = value1_double / value2_double;
                    break;
                case "^":
                    result = Math.Pow(value1_double, value2_double);
                    break;
                default:
                    return ShowError("Неподдерживаемая операция.");
            }

            Variant? variant;

            if (dataViewModel.Id == 0)
            {
                variant = new()
                {
                    Name = dataViewModel.VariantName,
                    Value1 = value1_double,
                    Value2 = value2_double,
                    Operation = dataViewModel.Operation[0],
                };

                _context.Variants.Add(variant);
                _context.SaveChanges();
            }
            else
            {
                variant = _context.Variants.Where(x => x.Id == dataViewModel.Id).FirstOrDefault();
                if (variant != null)
                {
                    variant.Name = dataViewModel.VariantName;
                    variant.Value1 = value1_double;
                    variant.Value2 = value2_double;
                    variant.Operation = dataViewModel.Operation[0];
                    _context.SaveChanges();
                }
            }

            return View(new DataViewModel() { Result = Math.Round(result, 4), Variant = variant });
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
    }
}
