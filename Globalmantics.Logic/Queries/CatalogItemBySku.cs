using Globalmantics.Domain;
using Highway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Logic.Queries
{
	public class CatalogItemBySku : Scalar<CatalogItem>
	{
		public CatalogItemBySku(string sku)
		{
			ContextQuery = context => context.AsQueryable<CatalogItem>()
				.FirstOrDefault(x => x.Sku == sku);
		}
	}
}
