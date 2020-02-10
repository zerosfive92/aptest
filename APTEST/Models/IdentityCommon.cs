using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APTEST.Models
{
    public class IdentityCommon
    {
        public string code { get; set; }

        public string name { get; set; }
    }
    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

    }
    public class ResultWithPaging : ResultInfo
    {
        public int page { get; set; }

        public int toltalSize { get; set; }

        public int pageSize { get; set; }
    }
    public class DSUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Group { get; set; }
        public bool? IsActive { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UType { get; set; }
        public string Sex { get; set; }
        public string GroupName { get; set; }
    }
}