using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Domain
{
	public class CartItem
	{
		public int CartItemId { get; set; }

		public Cart Cart { get; set; }
		public int CartId { get; set; }

		public CatalogItem CatalogItem { get; set; }
		public int CatalogItemId { get; set; }

		public int Quantity { get; set; }
	}
}
