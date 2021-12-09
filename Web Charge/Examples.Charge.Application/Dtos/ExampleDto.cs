using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examples.Charge.Application.Dtos
{
    public class ExampleDto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
