using FluentAssertions;
using Globalmantics.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.UnitTests
{
	[TestFixture]
    public class CartTests
    {
		[Test]
		public void Cart_is_initially_empty()
		{
			var cart = Cart.Create(0);

			cart.CartItems.Count().Should().Be(0);
		}

		[Test]
		public void Can_add_item_to_cart()
		{
			var cart = Cart.Create(0);
			var catalogItem = CatalogItem.Create(
				sku: "CAFE-314",
				description: "1 Pound Guatemalan Coffee Beans",
				unitPrice: 18.80m);

			cart.AddItem(catalogItem, 2);

			cart.CartItems.Count().Should().Be(1);
		}

		[Test]
		public void Group_items_of_same_kind()
		{
			var cart = Cart.Create(0);
			var catalogItem = CatalogItem.Create(
				sku: "CAFE-314",
				description: "1 Pound Guatemalan Coffee Beans",
				unitPrice: 18.80m);

			cart.AddItem(catalogItem, 2);
			cart.AddItem(catalogItem, 2);

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(4);
		}
	}
}
