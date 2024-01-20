using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RequestPayload
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int AtmId { get; set; }
        public string AccountNoTo { get; set; }
    }
}
