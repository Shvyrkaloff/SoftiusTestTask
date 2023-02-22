using Microsoft.AspNetCore.Mvc;
using SoftiusTest.Models;
using System.Diagnostics;

namespace SoftiusTest.Controllers
{
    /// <summary>Class HomeController.
    /// Implements the <see cref="Controller" /></summary>
    public class HomeController : Controller
    {
        int currentLine = 1; // переменная для отслеживания строки
        List<int[]> answers = new List<int[]>();
        List<int[,]> arrays = new List<int[,]>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// Обработчик нажатия на кнопку
        /// <summary>Indexes the specified data.</summary>
        /// <param name="data">The data.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        public IActionResult Index(string data)
        {
            try 
            {
                string[] lines = data.Split('\n');
                InputData(lines);
                Solutions();
                ViewBag.Answer = answers;
            }
            catch 
            {
                ViewBag.Error = "Проверьте введенные данные";
            }
            return View();
        }
        /// Обработка входных данных
        /// <summary>Inputs the data.</summary>
        /// <param name="lines">The lines.</param>
        private void InputData(string[] lines)
        {
            try 
            {
                int n = int.Parse(lines[0]); // задаю, что первая строка - всегда количество массивов
                if (n >= 1 && n <= 10000)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (currentLine < lines.Length && string.IsNullOrWhiteSpace(lines[currentLine]))
                        {
                            currentLine++; // пропуск пустой строки
                        }
                        int x = int.Parse(lines[currentLine++]); // первая строка после пропуска - количество строк массива
                        if (x >= 1 && x <= 200000)
                        {
                            int[,] array = new int[x, 2];
                            for (int j = 0; j < x; j++)
                            {
                                string[] values = lines[currentLine++].Split(' ');
                                array[j, 0] = int.Parse(values[0]);
                                array[j, 1] = int.Parse(values[1]);
                            }
                            arrays.Add(array);
                            if (currentLine < lines.Length && string.IsNullOrWhiteSpace(lines[currentLine])) // обработка пустой строки, разделение массивов
                            {
                                currentLine++;
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Количество заданий, которые могут поступить на сервер должно быть не меньше одного и не больще 200000";
                        }

                    }
                }
                else
                {
                    ViewBag.Error = "Количество наборов данных должно быть не меньше одного и не больше 10000";
                }
                
            }
            catch
            {
                ViewBag.Error = "Проверьте введенные данные";
            }
            
        }
        /// Решение задачи
        /// <summary>Solutionses this instance.</summary>
        private void Solutions()
        {
            foreach (int[,] array in arrays) // цикл для подсчета каждого массива
            {
                int sum = 0;
                List<int> rowValues = new List<int>();
                for (int i = 0; i < array.GetLength(0); i++) // цикл по строкам массива
                {
                    if (i == 0) // проверка на певую строку, где всега будет t + d
                    {
                        rowValues.Add(array[i, 0] + array[i, 1]);
                        sum = array[i, 0] + array[i, 1];
                    }
                    else
                    {
                        if (sum < array[i, 0]) // если сумма предыдущей строки меньше, чем t нынешней, то t + d
                        {
                            rowValues.Add(array[i, 0] + array[i, 1]);
                            sum = array[i, 0] + array[i, 1];
                        }
                        else // иначе сумма(е) + d
                        {
                            rowValues.Add(sum + array[i, 1]);
                            sum = sum + array[i, 1];
                        }
                    }
                }
                answers.Add(rowValues.ToArray()); // добавление ответа в зубчатый массив
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}