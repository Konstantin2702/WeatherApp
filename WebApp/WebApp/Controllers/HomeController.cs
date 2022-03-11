using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;


namespace WebApp.Controllers
{
    [ApiController] 
    public class HomeController : ControllerBase
    {
        private readonly IWorkWithFiles _workWithFiles;
        private WeatherContext _db;
        public HomeController(IWorkWithFiles workWithFiles, WeatherContext db)
        {
            this._workWithFiles = workWithFiles;
            _db = db;
        }

        [Route("api/weather")]
        [HttpGet]
        public IEnumerable<FileInf> Get()
        {
            List<Product> products = new List<Product>();
            string path = "Source";
            string[] fileNames = _workWithFiles.GetFileNamesFromDir(path);
            IEnumerable<FileInf> files = _workWithFiles.AddFilesNotInDB(fileNames, _db);
            return files;
        }

        [Route("api/weather")]
        [HttpPost]
        public FileInf Post(FileInf fileInfo)
        {
            string status = "Загружено";
            try
            {                       
                _workWithFiles.SaveWeatherInDB(_db, fileInfo.FileNames);
                _workWithFiles.CheckDownloadedFile(fileInfo, _db);
               
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            fileInfo.Status = status;
            return fileInfo;
        }

        [Route("api/weather/GetWeather")]
        [HttpGet]
        public IEnumerable<WeatherJSON> GetWeather(int month, int year)
        {
           return  _workWithFiles.GetFilteredWeather(month, year, _db);
        }
    }
}
