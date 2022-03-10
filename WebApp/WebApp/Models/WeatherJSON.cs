namespace WebApp.Models
{
    public class WeatherJSON
    {
        public string DateOfTaking { get; set; }

        public string TimeOfTaking { get; set; }


        public string Temperature { get; set; }
        //[Range(0, 100)]
        public string Humidity { get; set; }

        public string DewPoint { get; set; }

        //[Range(700, 800)]
        public string Pressure { get; set; }

        public string WindDirection { get; set; }

        //[Range(0, int.MaxValue)]
        public string WindSpeed { get; set; }

        // [Range(0, 100)]
        public string Cloudiness { get; set; }

        //[Range(0, int.MaxValue)]
        public string CloudBase { get; set; }

        // [Range(0, int.MaxValue)]
        public string HorizontalVisibility { get; set; }

        public string WeatherCondition { get; set; }
    }
}
