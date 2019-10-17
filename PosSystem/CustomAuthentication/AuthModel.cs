using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosSystem.CustomAuthentication
{
    public class AuthModel
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string AuthLevel { get; set; }
    }
}