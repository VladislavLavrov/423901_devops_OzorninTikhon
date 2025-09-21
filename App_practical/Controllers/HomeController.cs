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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(null);
        }

        [HttpPost]
        public IActionResult Index(string value1, string value2, string operation)
        {
            Console.WriteLine($"\"{value1}\" \"{value2}\"");
            if (!double.TryParse(value1.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double value1_double)
                || !double.TryParse(value2.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double value2_double))
            {
                return ShowError("Было введено неверное число.");
            }

            double result = 0.0;

            switch (operation)
            {
                case "Сложить (+)":
                    result = value1_double + value2_double;
                    break;
                case "Вычесть (-)":
                    result = value1_double - value2_double;
                    break;
                case "Умножить (*)":
                    result = value1_double * value2_double;
                    break;
                case "Разделить (/)":
                    if (value2_double == 0.0)
                    {
                        return ShowError("Деление на ноль.");
                    }
                    result = value1_double / value2_double;
                    break;
                case "Возвести в степень (^)":
                    result = Math.Pow(value1_double, value2_double);
                    break;
            }

            return View(new DataViewModel() { Result = Math.Round(result, 4) });
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
