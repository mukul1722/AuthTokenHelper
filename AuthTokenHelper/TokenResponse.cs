using System;
using System.Collections.Generic;
using System.Text;

namespace AuthTokenHelper
{
    public class TokenResponse
    {
        
        public string accessToken { get; set; }
        public string expiresIn { get; set; }
        public bool isSuccess { get; set; }
    }
}
