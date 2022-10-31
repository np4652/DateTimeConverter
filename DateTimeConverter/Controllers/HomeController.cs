using DateTimeConverter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DateFormat = DateTimeConverter.Models.DateFormat;

namespace DateTimeConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult ConvertDate(string date, string format)
        {
            string finalRes = "Oops!Unrecognized input format";
            string result = finalRes;
            if (!string.IsNullOrEmpty(date))
            {
                result = convert(date.Trim(), format?.Trim());
            }
            return Json(result ?? finalRes);
        }

        private string convert(string _date, string format = "dd MMM yyyy")
        {
            string result = null;
            try
            {
                var dateFormats = GetDateFormates();
                foreach (DateFormat df in dateFormats)
                {
                    if (Regex.IsMatch(_date, df.Regex))
                    {
                        if (df.Format=="dd-MMM-yy hh:mm:ss t.t.")
                        {
                            _date=_date.Replace(".", "");
                            df.Format="dd-MMM-yy hh:mm:ss tt";
                        }
                        DateTime dt = DateTime.Now.AddDays(1);
                        DateTime.TryParseExact(_date, df.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                        result = dt > DateTime.Now ? null : dt.ToString(format);
                        result=result=="01 Jan 0001" ? null : result;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                result = null;
            }
            return result;
        }

        private List<DateFormat> GetDateFormates()
        {
            return new List<DateFormat>{
                new DateFormat{
                    Format="d-M-yyyy",
                    Regex = @"([0-9])-([0-9])-\d{4}?$"
                },
                new DateFormat{
                    Format="d-MM-yyyy",
                    Regex = @"([0-9])-([0-1][0-9])-\d{4}$"
                },
                new DateFormat{
                    Format="dd-MM-yyyy",
                    Regex = @"([0-3][0-9])-([0-1][0-9])-\d{4}$"
                },
                new DateFormat{
                    Format="dd/MM/yyyy",
                    Regex = @"([0-3][0-9])/([0-1][0-9])/\d{4}?$"
                },
                new DateFormat{
                    Format="dd MMM yyyy",
                    Regex = @"([0-3][0-9]) ([a-zA-Z]{3}) \d{4}$"
                },
                new DateFormat{
                    Format="dd MMM yyyy hh:mm:ss",
                    Regex = @"([0-3][0-9]) ([a-zA-Z]{3}) \d{4} ([0-5][0-9]):([0-5][0-9]):([0-5][0-9])?$"
                },
                new DateFormat{
                    Format="dd MMM yyyy hh:mm:ss tt",
                    Regex = @"([0-3][0-9]) ([a-zA-Z]{3}) \d{4} ([0-5][0-9]):([0-5][0-9]):([0-5][0-9]) ([aA|pP])(m|M)?$"
                },
                new DateFormat{
                    Format="dd-MMM-yyyy hh:mm:ss t.t.",
                    Regex = @"([0-3][0-9])-([a-zA-Z]{3})-\d{4} ([0-5][0-9]):([0-5][0-9]):([0-5][0-9]) ([aA|pP]).(m|M).?$"
                },
                new DateFormat{
                    Format="dd-MMM-yy hh:mm:ss tt",
                    Regex = @"([0-3][0-9])-([a-zA-Z]{3})-\d{2} ([0-5][0-9]):([0-5][0-9]):([0-5][0-9]) ([aA|pP])(m|M)?$"
                },
                new DateFormat{
                    Format="dd-MMM-yy hh:mm:ss t.t.",
                    Regex = @"([0-3][0-9])-([a-zA-Z]{3})-\d{2} ([0-5][0-9]):([0-5][0-9]):([0-5][0-9]) ([aA|pP]).(m|M).?$"
                },
                new DateFormat{
                    Format="yyyy-MM-ddThh:mm:ss",
                    Regex = @"\d{4}-([0-1][0-9])-([0-3][0-9])T([0-5][0-9]):([0-5][0-9]):([0-5][0-9])?$"
                },
                new DateFormat{
                    Format="dd/MM/yyyy hh:mm",
                    Regex = @"([0-3][0-9])/([0-1][0-9])/\d{4} ([0-5][0-9]):([0-5][0-9])"
                },
                new DateFormat{
                    Format="hh:mm tt dd MMMM yyyy", //"01:51 PM 21 June 2022"
                    Regex = @"([0-5][0-9]):([0-5][0-9]) ([aA|pP])(m|M) \d{2} [a-zA-Z]+ \d{4}$"
        },
            };
        }
    }
}
