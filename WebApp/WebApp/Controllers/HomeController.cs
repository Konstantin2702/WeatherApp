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


        
        [Route("api/weather/SendFiles")]
        [HttpPost]
        public string SendFilesName(List<IFormFile> files)
        { 
            string status = "Загружено";
            List<StringBuilder> errors = new List<StringBuilder>();
            foreach(IFormFile file in files)
            {
                errors.Add(new StringBuilder(file.FileName));
            }
            if (files == null && !files[0].FileName.EndsWith("xlsx"))
            { 
                status = "Wrong type of file";
                return status;
             }
            else
            {           
                    int i = 0;
                    bool isException ;
                    foreach (IFormFile file in files)
                    {
                    using (var stream = new MemoryStream())
                    {
                        isException = false;
                        try
                        {
                            file.CopyTo(stream);
                            _workWithFiles.SaveWeatherInDB(_db, stream);
                        }
                        catch (Exception ex)
                        {
                            errors[i].Append("\tFailed");
                            isException = true;
                        }
                        if(!isException)
                        {
                            errors[i].Append("\tSucсessfully");
                        }
                        i++;
                    }
                }
                
            }
            StringBuilder errorsToSend = new StringBuilder();
            foreach(StringBuilder str in errors)
            {
                errorsToSend.Append(str + ";   ");
            }
            return JsonSerializer.Serialize(new {Text = errorsToSend.ToString()});
           

        }

        [Route("api/weather/GetWeather")]
        [HttpGet]
        public IEnumerable<WeatherJSON> GetWeather(int month, int year, int pageNumber, int countOFElementsOnPage)
        {
            int firstElementToShow = (pageNumber - 1) * countOFElementsOnPage;
            int lastElementToShow = firstElementToShow + countOFElementsOnPage - 1;
           return  _workWithFiles.GetFilteredWeather(month, year, _db, firstElementToShow, lastElementToShow);
        }

        [Route("api/weather/GetCountWeather")]
        [HttpGet]
        public int GetCountWeather(int month, int year)
        {
            return _workWithFiles.GetCountOfElements(month, year, _db);
        }
    }
}
