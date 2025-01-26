using System.ComponentModel.DataAnnotations;

namespace DanielASG.HelpDesk.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}