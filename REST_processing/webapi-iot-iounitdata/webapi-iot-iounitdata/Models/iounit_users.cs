using System;
using System.Collections.Generic;

namespace webapi_iot_growdata5.Models
{

    public partial class iounit_users
    {
        public int id { get; set; }

        public string username { get; set; }

        public string pw_hash { get; set; }

        public int id_ur { get; set; }

        public DateTime? last_modify { get; set; }
    }
}
