using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace mvNetApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticationFilter : AuthorizationFilterAttribute
    {
        private readonly bool _active = true;

        public BasicAuthenticationFilter()
        {
        }

        public BasicAuthenticationFilter(bool active)
        {
            _active = active;
        }

        private static void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{host}\"");
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_active) return;
            var identity = ParseAuthorizationHeader(actionContext);
            if (identity == null)
            {
                Challenge(actionContext); //return authentication error
                return;
            }
            if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
            {
                Challenge(actionContext); //return authenication error - no challenge
                return;
            }
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            base.OnAuthorization(actionContext);
        }

        protected virtual bool OnAuthorizeUser(string userName, string password, HttpActionContext actionContext)
        {
            return false;
        }

        protected virtual bool OnAuthorizeUser(string acctnum, string username, string password, HttpActionContext actionContext)
        {
            return !string.IsNullOrEmpty(acctnum) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
            {
                authHeader = auth.Parameter;
            }
            if (string.IsNullOrEmpty(authHeader))
            {
                return null;
            }
            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));
            var tokens = authHeader.Split(new char[] { ':' });
            return (int)tokens.Length < 2 ? null : new BasicAuthenticationIdentity( tokens[0], tokens[1]);
        }
    }
}