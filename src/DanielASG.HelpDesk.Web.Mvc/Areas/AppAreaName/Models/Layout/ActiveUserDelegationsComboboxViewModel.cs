using System.Collections.Generic;
using DanielASG.HelpDesk.Authorization.Delegation;
using DanielASG.HelpDesk.Authorization.Users.Delegation.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }

        public List<UserDelegationDto> UserDelegations { get; set; }

        public string CssClass { get; set; }
    }
}
