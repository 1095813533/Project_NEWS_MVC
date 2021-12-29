namespace NEWS_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userid { get; set; }

        [Key]
        [StringLength(20)]
        public string name { get; set; }

        [Required]
        [StringLength(20)]
        public string password { get; set; }

        public int type { get; set; }

        public DateTime regtime { get; set; }

        [Required]
        [StringLength(20)]
        public string email { get; set; }
    }
}
