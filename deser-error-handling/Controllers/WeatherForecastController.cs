using Microsoft.AspNetCore.Mvc;

namespace deser_error_handling.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpPost(Name = "GetWeatherForecast")]
    public WeatherForecast Post(WeatherForecast body)
    {
        return body;
    }
}
