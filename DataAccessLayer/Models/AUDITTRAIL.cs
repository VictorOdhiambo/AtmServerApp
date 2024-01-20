using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("AUDITTRAIL")]
    public partial class AUDITTRAIL
    {
        [Key]
        public int AUDITTRAILID { get; set; }
        public int? USERID { get; set; }
        [Unicode(false)]
        public string AUDITACTION { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime AUDITDATE { get; set; }

        [ForeignKey(nameof(USERID))]
        [InverseProperty("AUDITTRAILs")]
        public virtual USER? USER { get; set; }
    }
}
