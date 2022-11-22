using System.Text;
using Microsoft.AspNetCore.Mvc;
using POC.Outbox.WebAPI.Models.DB;

namespace POC.Outbox.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpPost(Name = "GetWeatherForecast")]
        public IActionResult Post(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            try
            {
                using var context = new PocOutboxContext();
                using var transaction = context.Database.BeginTransaction();
                context.Orders.Add(new Order { WriteTime = DateTime.UtcNow, Name = input });
                context.SaveChanges();

                context.Outboxes.Add(new Models.DB.Outbox { WriteTime = DateTimeOffset.UtcNow, Data = bytes });
                context.SaveChanges();

                transaction.Commit();

                //poll the database in this service?
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"Received {input}");
            return Created("bla bla", null);
        }
    }
}