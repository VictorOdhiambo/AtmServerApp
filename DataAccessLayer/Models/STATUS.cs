using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("STATUS")]
    public partial class STATUS
    {
        public STATUS()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            ATMs = new HashSet<ATM>();
            USERs = new HashSet<USER>();
        }

        [Key]
        public int STATUSID { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string DESCRIPTION { get; set; } = null!;

        [InverseProperty(nameof(ACCOUNT.STATUS))]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }
        [InverseProperty(nameof(ATM.STATUS))]
        public virtual ICollection<ATM> ATMs { get; set; }
        [InverseProperty(nameof(USER.STATUS))]
        public virtual ICollection<USER> USERs { get; set; }
    }
}
