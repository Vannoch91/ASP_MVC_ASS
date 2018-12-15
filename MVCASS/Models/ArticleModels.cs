using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVCASS.Models
{
    [Table("tbl_article")]
    public class Articles
    {
            [Key]
            public int id { get; set; }
           [Required]
            public string title { get; set; }
           [Required]
        public string description { get; set; }

            public DateTime created_at { get; set; }
            public int author { get; set; }
     
    }
}