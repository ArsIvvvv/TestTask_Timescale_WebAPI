using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using TimeScaleApi.Common;
using TimeScaleApi.DTO;
using TimeScaleApi.Map;
using WebApi_TimeScale.Controllers;
using WebApi_TimeScale.Data.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace TimeScaleApi.Service
{
    public class CSVService: ICSVService
    {
        private readonly int Max = 10000; // Макс количество строк

        // Метод для чтения и валидации CSV-файла
        public ResultException<List<Value>> ReadCSV(Stream file, string filename)
        {
            var values = new List<Value>();

            using (var reader = new StreamReader(file))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "yyyy-MM-dd HH:mm:ss.ffff" };
                    csv.Context.RegisterClassMap<ValueMap>();

                    var allErrors = new List<string>();

                    var valueDto = csv.GetRecords<ValueDto>().ToList();



                    if (valueDto.Count == 0 || valueDto.Count > Max)
                        allErrors.Add("Количество превышает 1000 строк или их нету");

                    foreach (var value in valueDto)
                    {

                        bool success = DateTime.TryParseExact(value.Date, "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out DateTime parsedDate);

                        if (!success)
                        {
                            allErrors.Add($"Неверный формат даты");
                        }
                        else
                        {
                            DateTime minDate = new DateTime(2000, 1, 1);
                            DateTime maxDate = DateTime.UtcNow;

                            if (parsedDate < minDate || parsedDate > maxDate)
                            {
                                allErrors.Add($"Дата должна быть между {minDate:yyyy-MM-dd} и {maxDate:yyyy-MM-dd}.");
                            }
                        }


                        if (Valid(value, out var errors))
                        {
                            values.Add(new Value
                            {
                                Date = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc),
                                LeadTime = value.ExecutionTime.Value,
                                Indicator = value.Value.Value,
                                FileName = filename
                            });
                        }
                        else
                        {
                            allErrors.AddRange(errors);
                        }

                    }

                    allErrors = allErrors.Distinct().ToList(); // убирает повторы

                    if (allErrors.Any())
                    {
                        return ResultException<List<Value>>.Failure("Ошибки:\n" + string.Join("\n", allErrors));
                    }
                }
            }

             return ResultException<List<Value>>.Success(values);
        }

        // Метод для рассчета значения для таблицы Result
        public ResultException<Result> ReadResult(List<Value> values)
        {
            var timedelta = (values.Max(x => x.Date) - values.Min(x => x.Date)).TotalSeconds;
            var mindata = values.Min(x => x.Date);
            var averagetime = values.Average(x => x.LeadTime);
            var averagevalue = values.Average(x => x.Indicator);
            var medianvalue = values
                .OrderBy(x => x.Indicator)
                .Select(x => x.Indicator)
                .Skip((values.Count / 2) - (values.Count % 2 == 0 ? 1 : 0))
                .Take(values.Count % 2 == 0 ? 2 : 1).Average();
            var maxvalue = values.Max(x =>x.Indicator);
            var minvalue = values.Min(x =>x.Indicator);

            var result = new Result
            {
                TimeDelta = timedelta,
                MinData = mindata,
                AverageTime = averagetime,
                AverageValue = averagevalue,
                MedianValue = medianvalue,
                MaxValue = maxvalue,
                MinValue = minvalue,
            };

            result.ListValue = values;

            return ResultException<Result>.Success(result);

        }

        // Метод для валидации DTO
        public bool Valid(ValueDto dto, out List<string> errors)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(dto, context, results, true);

            errors = results.Select(r => r.ErrorMessage).ToList();

            return valid;
        }





    }
}
