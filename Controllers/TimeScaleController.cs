using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeScaleApi.DTO.Command;
using TimeScaleApi.Service;
using WebApi_TimeScale.Data;
using WebApi_TimeScale.Data.Entity;

namespace WebApi_TimeScale.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeScaleController: Controller
    {

        private readonly ICSVService _csvService;
        private readonly AppDbContext _db;

        public TimeScaleController(ICSVService csvService, AppDbContext db)
        {
            _csvService = csvService;
            _db = db;
        }

        [HttpPost("Read-CSV")]
        public async Task<IActionResult> ReadValueCSV([FromForm] IFormFileCollection file)
        {
             if (file == null)
                return BadRequest("Нету файла");

             string name = Path.GetFileNameWithoutExtension(file[0].FileName);
            
             bool filename = await _db.Values.AnyAsync(x => x.FileName == name);

             if (filename)
             {
                 var filevalue = _db.Values.FirstOrDefault(x => x.FileName == name);
                 var resultid = filevalue.ResultId;
                 var resultvalue = _db.Results.Where(x => x.Id == resultid);
                 _db.RemoveRange(resultvalue); //Каскадное удаление

                 var newvalue = _csvService.ReadCSV(file[0].OpenReadStream(), name);

                 if (!newvalue.IsSuccess)
                     return BadRequest($"{newvalue.Error}");

                 var newresult = _csvService.ReadResult(newvalue.Value);
                 await _db.Values.AddRangeAsync(newvalue.Value);
                 await _db.Results.AddAsync(newresult.Value);
                 await _db.SaveChangesAsync();
                 return Ok($"Перезаписан {file[0].FileName}");
             }
             else
             {
                 var value = _csvService.ReadCSV(file[0].OpenReadStream(), name);

                 if (!value.IsSuccess)
                        return BadRequest($"{value.Error}");

                 var result = _csvService.ReadResult(value.Value);
                 await _db.Values.AddRangeAsync(value.Value);
                 await _db.Results.AddAsync(result.Value);
                 await _db.SaveChangesAsync();

                 return Ok(value.Value);
             }               
        }

        [HttpGet("Get-Results")]
        public async Task<IActionResult> GetResultsCSV()
        {
            try
            {
                var results = _db.Results
                    .OrderBy(x => x.AverageTime).ToList();

                if (!results.Any())
                    return NotFound("Нету данных");

                return Ok(results);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Get-Value-By-Date")]
        public async Task<IActionResult> GetValueCSV([FromBody] FileNameRequest fileName)
        {
            try
            {
               var values = await _db.Values
                    .Where(x => x.FileName == fileName.FileName)
                    .OrderByDescending(d => d.Date)
                    .Take(10)
                    .ToListAsync();
                
                if (!values.Any())
                    return NotFound("Нету данных");

                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }

}
