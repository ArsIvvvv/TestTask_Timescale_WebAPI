using System.ComponentModel.DataAnnotations;

namespace TimeScaleApi.DTO.Command
{
    public class FileNameRequest
    {
        [Required(ErrorMessage = "Название файла обязательно")]
        public string? FileName { get; set; }
    }
}
