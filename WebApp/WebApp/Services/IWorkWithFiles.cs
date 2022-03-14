using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IWorkWithFiles
    {
        public void SaveWeatherInDB(WeatherContext db, MemoryStream stream);
        public IEnumerable<WeatherJSON> GetFilteredWeather(int month, int year, WeatherContext db, int first, int last);
        public int GetCountOfElements(int month, int year, WeatherContext db);
    }
}
