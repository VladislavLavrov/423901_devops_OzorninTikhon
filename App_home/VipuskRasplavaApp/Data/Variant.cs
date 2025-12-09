using RaspredeleniyeDutyaFormulas;
using System.ComponentModel.DataAnnotations;

namespace RaspredeleniyeDutyaApp.Data
{
    public class Variant
    {
        /// <summary>
        /// ID варианта
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название варианта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID пользователя, который создал данный вариант
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Входные данные по фурмам
        /// </summary>
        public InitialData Data { get; set; }

        /// <summary>
        /// Информация о создателе варианта
        /// </summary>
        public UserAccount? Owner {  get; set; }
    }
}
