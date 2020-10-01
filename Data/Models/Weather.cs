using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Weather
    {
        [Key]     
        public int Id { get; set; }
        public string dates { get; set; }
        public string names { get; set; }
        public string minTemperatures { get; set; }
        public string maxTemperatures { get; set; }
        public string descriptions { get; set; }
    }
}
