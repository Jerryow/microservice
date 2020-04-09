using System;
using System.Collections.Generic;
using System.Text;

namespace IService.Models
{
    public class User
    {
        public int PKID { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
