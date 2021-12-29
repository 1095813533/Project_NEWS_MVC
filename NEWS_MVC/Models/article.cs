namespace NEWS_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("article")]
    public partial class article
    {
        public int articleid { get; set; }

        public int userid { get; set; }

        [Required]
        [StringLength(40)]
        public string name { get; set; }

        public DateTime newstime { get; set; }

        [Required]
        [StringLength(10)]
        public string type { get; set; }

        public int priority { get; set; }

        [StringLength(400)]
        public string fengmian { get; set; }

        [Required]
        [StringLength(20)]
        public string username { get; set; }
    }
}
