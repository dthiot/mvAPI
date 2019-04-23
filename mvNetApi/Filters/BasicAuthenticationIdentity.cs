using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace mvNetApi.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public string Password { get; set; }

        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {            
            Password = password;
        }
    }
}