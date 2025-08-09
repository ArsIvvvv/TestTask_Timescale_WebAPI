using CsvHelper.Configuration;
using WebApi_TimeScale.Data.Entity;

namespace TimeScaleApi.Map
{
    public class ValueMap : ClassMap<Value>
    {
        public ValueMap()
        {
            Map(m => m.Date).Name("Date");          
            Map(m => m.LeadTime).Name("ExecutionTime");
            Map(m => m.Indicator).Name("Value");
        }
    }
}
