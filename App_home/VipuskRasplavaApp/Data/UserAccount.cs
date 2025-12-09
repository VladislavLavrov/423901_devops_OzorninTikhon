using System.ComponentModel.DataAnnotations;

namespace RaspredeleniyeDutyaApp.Data
{
    public class UserAccount
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Свойство, определяющее, является ли пользователь администратором
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Хеш пароля пользователя
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Варианты пользователя
        /// </summary>
        public List<Variant> Variants { get; set; }

        public string GetFullName()
        {
            if (MiddleName.Length > 0)
                return $"{LastName} {FirstName} {MiddleName}";
            else
                return $"{LastName} {FirstName}";
        }
    }
}
