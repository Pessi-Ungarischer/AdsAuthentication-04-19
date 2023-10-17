using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsAuthentication.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public int ListerId { get; set; }
        public string ListerName { get; set; }
        public string PhoneNum { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
