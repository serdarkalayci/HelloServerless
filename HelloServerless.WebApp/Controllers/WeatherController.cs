using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace HelloServerless.WebApp.Controllers
{
    public class WeatherController : Controller
    {
        // 
        // GET: /Weather/

        public string Index()
        {
            return "Weather will be displayed here...";
        }

        // 
        // GET: /Weather/Welcome/ 

        public string Welcome(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }
    }
}