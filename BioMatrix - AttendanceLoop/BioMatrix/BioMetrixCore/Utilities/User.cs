using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Utilities
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public int role_id { get; set; }
        public bool activated { get; set; }
        public bool enabled { get; set; }
    }
}
