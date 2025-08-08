using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi_TimeScale.Data.Entity
{
    public class Value
    {       
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double LeadTime { get; set; }
        public double Indicator { get; set; }
        public string FileName { get; set; }

        // навигационные свойства
        public int ResultId { get; set; }

        [JsonIgnore]
        public Result Result { get; set; }
    }
}
