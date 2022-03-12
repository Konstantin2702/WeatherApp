using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IWorkWithFiles
    {
        public string[] GetFileNamesFromDir(string path);
        public string CheckDownloadedFile(string fileName, WeatherContext db);
        public void SaveWeatherInDB(WeatherContext db, MemoryStream stream);
        public IEnumerable<FileInf> AddFilesNotInDB(string[] fileNames, WeatherContext db);
        public IEnumerable<WeatherJSON> GetFilteredWeather(int month, int year, WeatherContext db, int first, int last);
        public int GetCountOfElements(int month, int year, WeatherContext db);
    }
}
