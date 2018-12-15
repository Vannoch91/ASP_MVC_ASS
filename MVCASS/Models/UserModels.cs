using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCASS.Models
{
    [Table("tbl_user")]
    public class User
    {
        
        [Key]
        public int id { get; set; }
        public string username { get; set; }
       
        public string email { get; set; }
      
        public string password { get; set; }
    }
}