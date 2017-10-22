using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Domain
{
	public class CatalogItem
	{
		public int CatalogItemId { get; set; }

		public string Sku { get; set; }

		public string Description { get; set; }

		public decimal UnitPrice { get; set; }
	}
}