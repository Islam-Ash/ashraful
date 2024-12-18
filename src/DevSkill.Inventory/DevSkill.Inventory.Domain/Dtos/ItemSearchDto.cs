using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
	public class ItemSearchDto
	{
		public string? Name { get; set; }
		public string? Barcode { get; set; }
        public string? PriceStartFrom { get; set; }
        public string? PriceStartTo { get; set; }
       
        public string? CategoryId { get; set; }
    }
}
