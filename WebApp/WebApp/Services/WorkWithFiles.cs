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


        public void SaveWeatherInDB(WeatherContext db, MemoryStream stream)
        {
            List<WeatherInfo> weatherInfos = new List<WeatherInfo>();
            IWorkbook hssfwb;

            hssfwb = WorkbookFactory.Create(stream);


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

                        var checkDate = db.WeatherInfos.Where(w => w.DateOfTaking == weatherInfo.DateOfTaking && w.TimeOfTaking == weatherInfo.TimeOfTaking);
                        if(checkDate.Count() > 0)
                        {
                            continue;
                        }
                        else
                        {
                            db.WeatherInfos.Add(weatherInfo);
                        }
                    }
                }
            }
            db.SaveChanges();
        }


        public IEnumerable<WeatherJSON> GetFilteredWeather(int month, int year, WeatherContext db, int first, int last)
        {
            List<WeatherJSON> weatherToSend = new List<WeatherJSON>();
            if (month != 0 && year != 0)
            {
                List<WeatherJSON> weather = db.WeatherInfos
                   .Where(w => w.DateOfTaking.Year == year && w.DateOfTaking.Month == month)
                   .OrderBy(w => w.DateOfTaking.Day)
                   .ThenBy(w => w.TimeOfTaking)
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
                for (int i = 0; i < weather.Count(); i++)
                {
                    if (i >= first && i <= last)
                    {
                        weatherToSend.Add(weather[i]);
                    }
                    else if (i > last)
                        break;

                }
                return weatherToSend;
            }
            else
            {
                return null;
            }
        }

        public int GetCountOfElements(int month, int year, WeatherContext db)
        {
            if (month != 0 && year != 0)
            {
                List<WeatherJSON> weather = db.WeatherInfos
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

                return weather.Count();
            }
            else
                return 0;
        }

    }
}
