using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace KnockoutSample.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}
