namespace WebApi_TimeScale.Data.Entity
{
    public class Result
    {
        public int Id {  get; set; }
        public double TimeDelta { get; set; }
        public DateTime MinData { get; set; }
        public double AverageTime { get; set; }
        public double AverageValue { get; set; }    
        public double MedianValue {  get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        // Связь один-ко-многим
        public List<Value> ListValue = new List<Value>();

    }
}
