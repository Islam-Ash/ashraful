using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
	public class Measurement : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public string MeasurementName { get; set; }
		public string Symbol {  get; set; }

		public ICollection<Item> Items { get; set; }
	}
}
