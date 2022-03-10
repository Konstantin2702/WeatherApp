using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IWorkWithFiles
    {
        public string[] GetFileNamesFromDir(string path);
        public void CheckDownloadedFile(FileInf fileInfo, WeatherContext db);
        public void SaveWeatherInDB(WeatherContext db, string fileName);
        public IEnumerable<FileInf> AddFilesNotInDB(string[] fileNames, WeatherContext db);
        public IEnumerable<WeatherJSON> GetFilteredWeather(int month, int year, WeatherContext db);
    }
}
