﻿using System.Collections.Generic;

namespace DanielASG.HelpDesk.Authorization.Users.Profile.Dto
{
    public class UpdateGoogleAuthenticatorKeyOutput
    {
        public IEnumerable<string> RecoveryCodes { get; set; }
    }
}
