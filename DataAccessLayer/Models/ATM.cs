using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("ATM")]
    public partial class ATM
    {
        [Key]
        public int ATMID { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BALANCE { get; set; }
        public int? STATUSID { get; set; }

        [ForeignKey(nameof(STATUSID))]
        [InverseProperty("ATMs")]
        public virtual STATUS? STATUS { get; set; }
    }
}
