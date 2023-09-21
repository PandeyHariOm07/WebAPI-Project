using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.WebAPI.CustomFilter
{
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private UserDetails _userDetails = null;
        public BasicAuthenticationAttribute()
        {
            _userDetails = new UserDetails();
        }
        public bool AllowMultiple => throw new NotImplementedException();

        public async Task<bool> IsValidUser(string emailId, string password)
        {
            return await _userDetails.ValidateUser(emailId, password)!=null;

        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authorization = context.Request.Headers.Authorization?.ToString();
            if(!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Basic"))
            {
                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Substring(6)));//username:password
                string[] data = credentials.Split(':');
                string username = data[0];
                string password = data[1];
                if(await IsValidUser(username, password))
                {
                    context.Principal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new System.Net.Http.Headers.AuthenticationHeaderValue[0], context.Request);
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new System.Net.Http.Headers.AuthenticationHeaderValue[0], context.Request);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}