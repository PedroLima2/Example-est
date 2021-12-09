using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examples.Charge.Application.Dtos
{
    public class PhoneNumberTypeDto
    {
        [Key]
        public int PhoneNumberTypeID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
