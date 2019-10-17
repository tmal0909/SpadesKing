using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PosSystem.CustomAuthentication
{
    public class RepositoryAuthModel
    {
        public static AuthModel GetAuthModel(FormsIdentity Identity)
        {
            return JsonConvert.DeserializeObject<AuthModel>(Identity.Ticket.UserData);
        }

        public static string GetAuthLevel(FormsIdentity Identity)
        {
            return JsonConvert.DeserializeObject<AuthModel>(Identity.Ticket.UserData).AuthLevel;
        }
    }
}