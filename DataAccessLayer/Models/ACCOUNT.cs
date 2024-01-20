using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("ACCOUNT")]
    public partial class ACCOUNT
    {
        [Key]
        public int ACCOUNTID { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ACCOUNTNO { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BALANCE { get; set; }
        public int? USERID { get; set; }
        public int? STATUSID { get; set; }

        [ForeignKey(nameof(STATUSID))]
        [InverseProperty("ACCOUNTs")]
        public virtual STATUS? STATUS { get; set; }
        [ForeignKey(nameof(USERID))]
        [InverseProperty("ACCOUNTs")]
        public virtual USER? USER { get; set; }
    }
}
