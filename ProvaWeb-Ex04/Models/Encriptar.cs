using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProvaWeb_Ex04.Models
{
    public class Encriptar
    {
        [Key]
        public int MyProperty { get; set; }

        [Required(ErrorMessage = "Por favor Prencha o Campo")]
        public string Txt { get; set; }
    }
}