using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public class WorkWithFiles : IWorkWithFiles
    {
        public void CheckDownloadedFile(FileInf fileInfo, WeatherContext db)
        {
            SavedFiles savedFile = new SavedFiles { FileName = fileInfo.FileNames };
            var checkFile = db.SavedFiles.Where(f => f.FileName.Equals(fileInfo.FileNames));
            if (checkFile.Count() == 0)
            {
                 db.SavedFiles.Add(savedFile);
                 db.SaveChanges();
            }
        }

        public string[] GetFileNamesFromDir(string path)
        {
           string[] fileNames = Directory.GetFiles(path);
           List<string> filesToReturn = new List<string>();

            foreach(string str in fileNames)
            {
                string tempString = str.Split('\\')[1];
                filesToReturn.Add(tempString);
            }
           return filesToReturn.ToArray();
        }

        public  void SaveWeatherInDB(WeatherContext db, string fileName)
        {
            List<WeatherInfo> weatherInfos = new List<WeatherInfo>();
            IWorkbook hssfwb;
            using (FileStream file = new FileStream(
                   $"Source/{fileName}",
                   FileMode.Open, FileAccess.Read))
            {
                hssfwb = WorkbookFactory.Create(file);
            }

            foreach (ISheet sheet in hssfwb)
            {
                for (int row = 4; row <= sheet.LastRowNum; row++)
                {

                    var sheetRow = sheet.GetRow(row);

                    WeatherCondition weatherCondition;
                    WeatherInfo weatherInfo = new WeatherInfo();
                    if (sheetRow.GetCell(11) != null)
                    {
                        weatherCondition = new WeatherCondition { Text = sheetRow.GetCell(11).ToString() };
                    }
                    else
                    {
                        weatherCondition = new WeatherCondition { Text = "" };
                    }



                    var tempCondition = db.WeatherConditions.Where(w => w.Text.Equals(weatherCondition.Text));
                    if (tempCondition.Count() == 0)
                    {
                        db.WeatherConditions.Add(weatherCondition);
                        db.SaveChanges();
                        weatherInfo.WeatherCondition = weatherCondition;

                    }
                    else
                    {
                        weatherInfo.WeatherCondition = tempCondition.ToList()[0];
                    }

                    if (sheetRow != null)
                    {
                        weatherInfo.DateOfTaking = DateTime.Parse(sheetRow.GetCell(0).ToString());
                        weatherInfo.TimeOfTaking = DateTime.Parse(sheetRow.GetCell(1).ToString()).TimeOfDay;
                        weatherInfo.Temperature = float.Parse(sheetRow.GetCell(2).ToString());
                        weatherInfo.Humidity = float.Parse(sheetRow.GetCell(3).ToString());
                        weatherInfo.DewPoint = float.Parse(sheetRow.GetCell(4).ToString());
                        weatherInfo.Pressure = int.Parse(sheetRow.GetCell(5).ToString());

                        weatherInfo.WindDirection = sheetRow.GetCell(6) != null ? sheetRow.GetCell(6).ToString() : "";
                        weatherInfo.WindDirection = sheetRow.GetCell(6).ToString();
                        weatherInfo.WindSpeed = int.TryParse(sheetRow.GetCell(7).ToString(), out var w) ? w : 0;
                        weatherInfo.Cloudiness = int.TryParse(sheetRow.GetCell(8).ToString(), out var c) ? c : 0;
                        weatherInfo.CloudBase = int.TryParse(sheetRow.GetCell(9).ToString(), out var cb) ? cb : 0;
                        weatherInfo.HorizontalVisibility = int.TryParse(sheetRow.GetCell(10).ToString(), out var h) ? h : 0;

                        weatherInfos.Add(weatherInfo);
                        
                    }
                }
            }
           db.WeatherInfos.AddRange(weatherInfos);
           db.SaveChanges();
        }

        public IEnumerable<FileInf> AddFilesNotInDB(string[] fileNames, WeatherContext db)
        {
            List<FileInf> files = new List<FileInf>();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in fileNames)
            {
                var file = db.SavedFiles.Where(f => f.FileName.Equals(str));
                if (file.Count() == 0)
                {
                    files.Add(new FileInf { FileNames = str, Status = "" });
                }
            }
            return files;
        }

        public IEnumerable<WeatherJSON> GetFilteredWeather(int month, int year, WeatherContext db)
        {
            if (month != 0 && year != 0)
            {
                IEnumerable<WeatherJSON> weather = db.WeatherInfos
                   .Where(w => w.DateOfTaking.Year == year && w.DateOfTaking.Month == month)
                   .Select(w => new WeatherJSON
                   {
                       DateOfTaking = w.DateOfTaking.ToShortDateString(),
                       TimeOfTaking = w.TimeOfTaking.ToString().Substring(0, 8),
                       WindSpeed = w.WindSpeed.ToString(),
                       CloudBase = w.CloudBase.ToString(),
                       DewPoint = w.DewPoint.ToString(),
                       WindDirection = w.WindDirection.ToString(),
                       Cloudiness = w.Cloudiness.ToString(),
                       HorizontalVisibility = w.HorizontalVisibility.ToString(),
                       Humidity = w.Humidity.ToString(),
                       Pressure = w.Pressure.ToString(),
                       Temperature = w.Temperature.ToString(),
                       WeatherCondition = w.WeatherCondition.Text
                   })
                   .ToList();
            return weather;
        }
            else
            {
                return null;
            }
        }
    }
}
