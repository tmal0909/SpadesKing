using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PosSystem.CustomHelper
{
    public class MemberSelectListItem : SelectListItem
    {
        public object htmlAttributes { get; set; }

        public string Phone { get; set; }

        public string Integration { get; set; }

        public string FreeChangeAmount { get; set; }
    }
}