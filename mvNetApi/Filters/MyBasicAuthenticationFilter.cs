using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace mvNetApi.Filters
{
    public class MyBasicAuthenticationFilter : BasicAuthenticationFilter
    {
        public MyBasicAuthenticationFilter()
        {
        }

        public MyBasicAuthenticationFilter(bool active)
            : base(active)
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
                base.OnAuthorization(actionContext);
        }

        protected override bool OnAuthorizeUser( string userName, string password, HttpActionContext actionContext)
        {
            //This verifies that the request has provided 3 valid parameters and that they match the Account, UserName and Password
            //from the configuration file.  Uses a set account/username/password combination because it is only called internally
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return false;
            var checkUserName = ConfigurationManager.AppSettings["UserName"];
            var checkPassword = ConfigurationManager.AppSettings["Password"];

            return (userName.Equals(checkUserName)) && (password.Equals(checkPassword));
        }

        private string ValidatePass( string userName, string password)
        {
            return  userName + ":" + password;
        }
    }
}