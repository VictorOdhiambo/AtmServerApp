using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDetail
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AccountNo { get; set; }
        public bool IsActive { get; set; }  
        public string AccountBal { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public int AtmId { get; set; }
    }
}
