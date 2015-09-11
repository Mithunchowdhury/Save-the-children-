using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSMS
{
    public class Employee
    {
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Stafcode { get; set; }
        public string GroupId { get; set; }
        public string Locationid { get; set; }
        public bool  Activeid { get; set; }
        //public string LocationName { get; set; }

    }
}