using System.ComponentModel.DataAnnotations;

namespace DanielASG.HelpDesk.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}