using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examples.Charge.Application.Dtos
{
    public class PersonPhoneDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe BusinessEntityID")]
        public int BusinessEntityID { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Por favor, informe PhoneNumberTypeID")]
        public int PhoneNumberTypeID { get; set; }


        public PhoneNumberTypeDto PhoneNumberType { get; set; }
    }
}
