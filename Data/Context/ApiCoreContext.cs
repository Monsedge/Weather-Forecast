using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using System.Linq;

namespace Data.Context
{    
    public class ApiCoreContext : DbContext //наследуется от стандартного класс DbContext
    {        
        public DbSet<Weather> Weathers { get; set; }
        public ApiCoreContext(DbContextOptions<ApiCoreContext> options) : base(options)
        {
            
        }
        public string Find(string name, string date)
        {
            var requestedInstance = Weathers.FirstOrDefault(e => e.names == name && e.dates == date);
            if (requestedInstance != null)
                return $"днем {requestedInstance.maxTemperatures}, ночью {requestedInstance.minTemperatures}\n{requestedInstance.descriptions}";
            else 
                return "ERROR: request input is invalid or database is empty";
            
        }
        public string GetDateList()
        {
            var dateCollection =  Weathers.Select(e=>e.dates).Distinct();            
            if (dateCollection.Count() > 0)            
                return string.Join(",", dateCollection);
            else 
                return "ERROR: datelist was requested from an empty database";
        }
        public string GetCityList()
        {
            var cityCollection = Weathers.Select(e => e.names).Distinct();
            if (cityCollection.Count() > 0)
                return string.Join(",", cityCollection);
            else 
                return "ERROR: citylist was requested from an empty database";
        }
    }
}
