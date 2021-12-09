using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examples.Charge.Application.Dtos
{
    public class PersonDto
    {
        [Key]
        public int BusinessEntityID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<PersonPhoneDto> Phones { get; set; }
    }
}
