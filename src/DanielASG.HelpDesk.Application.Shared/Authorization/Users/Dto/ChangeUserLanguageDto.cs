using System.ComponentModel.DataAnnotations;

namespace DanielASG.HelpDesk.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
