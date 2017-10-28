using FluentAssertions;
using Globalmantics.DAL;
using Globalmantics.Domain;
using Globalmantics.Logic;
using Highway.Data;
using NUnit.Framework;
using System.Linq;

namespace Globalmantics.IntegrationTests
{
	//[Isolated]
	[TestFixture]
	public class CartServiceTests
	{
		[Test]
		public void Cart_is_initially_empty()
		{
			var services = CartServiceContext.GivenServices();

			var cart = services.WhenLoadCart();

			cart.CartItems.Count().Should().Be(0);
		}

		[Test]
		public void Can_add_item_to_cart()
		{
			var services = CartServiceContext.GivenServices();

			var cart = services.WhenLoadCart();

			services.WhenAddItemToCart(cart);

			cart.CartItems.Count().Should().Be(1);
		}

		[Test]
		public void Group_items_of_same_type()
		{
			var services = CartServiceContext.GivenServices();

			var cart = services.WhenLoadCart();

			services.WhenAddItemToCart(cart, quantity: 2);
			services.WhenAddItemToCart(cart, quantity: 1);

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(3);
		}

		[Test]
		public void Can_load_cart_with_one_item()
		{
			var services = CartServiceContext.GivenServices();
			InitializeCartWithOneItem(services.EmailAddress);

			Cart cart = services.WhenLoadCart();

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(2);
		}

		private void InitializeCartWithOneItem(string emailAddress)
		{
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var user = context.Add(User.Create(emailAddress));

			context.Commit();
			var cart = context.Add(Cart.Create(user.UserId));
			var catalogItem = context.AsQueryable<CatalogItem>()
				.Single(x => x.Sku == "CAFE-314");
			cart.AddItem(catalogItem, 2);
			context.Commit();
		}
	}
}
