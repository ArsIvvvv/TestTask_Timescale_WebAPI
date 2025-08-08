using System.Collections.Generic;
using TimeScaleApi.Common;
using WebApi_TimeScale.Data.Entity;

namespace TimeScaleApi.Service
{
    public interface ICSVService
    {

        /// <summary>
        /// Возвращает список объектов Value и заполняет из прочитаного CSV-файла.
        /// Валидарует параметры CSV-файла.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filename"></param>
        /// <returns>List<Value></returns>
        public ResultException<List<Value>> ReadCSV(Stream file, string filename);

        /// <summary>
        /// Рассчитывает параметры и возвращает объект Result для добавления в бд.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Result</returns>
        public ResultException<Result> ReadResult(List<Value> values);

    }
}
