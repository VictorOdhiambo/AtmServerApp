using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("USER")]
    public partial class USER
    {
        public USER()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            AUDITTRAILs = new HashSet<AUDITTRAIL>();
        }

        [Key]
        public int USERID { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string USERNAME { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string PASSWORD { get; set; } = null!;
        public int? FAILEDATTEMPTS { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LASTLOGIN { get; set; }
        public int? STATUSID { get; set; }

        [ForeignKey(nameof(STATUSID))]
        [InverseProperty("USERs")]
        public virtual STATUS? STATUS { get; set; }
        [InverseProperty(nameof(ACCOUNT.USER))]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }
        [InverseProperty(nameof(AUDITTRAIL.USER))]
        public virtual ICollection<AUDITTRAIL> AUDITTRAILs { get; set; }
    }
}
