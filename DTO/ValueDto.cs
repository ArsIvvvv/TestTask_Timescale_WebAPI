using System.ComponentModel.DataAnnotations;

namespace TimeScaleApi.DTO
{
    public class ValueDto
    {

        [Required(ErrorMessage = "Дата не может быть пустым")]
        public string? Date { get; set; }

        [Required(ErrorMessage = "Время выполнения не может быть пустым")]
        [Range(0, int.MaxValue, ErrorMessage = "Время выполнение не может быть отрицательным")]
        public double? ExecutionTime { get; set; }

        [Required(ErrorMessage = "Показатель не может быть пустым")]
        [Range(0, double.MaxValue, ErrorMessage = "Значение не может быть отрицательным")]
        public double? Value { get; set; }
    }
}
