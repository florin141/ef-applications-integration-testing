using FluentAssertions;
using Globalmantics.DAL;
using Globalmantics.Domain;
using Globalmantics.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.IntegrationTests
{
	//[Isolated]
	[TestFixture]
	public class CartServiceTests
	{
		[Test]
		public void Cart_is_initially_empty()
		{
			var context = new GlobalmanticsContext();
			var userService = new UserService(context);
			var cartService = new CartService(context);

			User user = GivenUser(context, userService);

			var cart = cartService.GetCartForUser(user);
			context.SaveChanges();

			cart.CartItems.Count().Should().Be(0);
		}

		private static User GivenUser(GlobalmanticsContext context, UserService userService)
		{
			var user = userService.GetUserByEmail($"test{Guid.NewGuid().ToString()}@globalmantics.com");
			context.SaveChanges();
			return user;
		}

		[Test]
		public void Can_add_item_to_cart()
		{
			var context = new GlobalmanticsContext();
			var userService = new UserService(context);
			var cartService = new CartService(context);

			User user = GivenUser(context, userService);

			var cart = cartService.GetCartForUser(user);
			context.SaveChanges();

			cartService.AddItemToCart(cart, "CAFE-314", 2);
			context.SaveChanges();

			cart.CartItems.Count().Should().Be(1);
		}

		[Test]
		public void Group_items_of_same_type()
		{
			var context = new GlobalmanticsContext();
			var userService = new UserService(context);
			var cartService = new CartService(context);

			User user = GivenUser(context, userService);

			var cart = cartService.GetCartForUser(user);
			context.SaveChanges();

			cartService.AddItemToCart(cart, "CAFE-314", 2);
			cartService.AddItemToCart(cart, "CAFE-314", 1);
			context.SaveChanges();

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(3);
		}
	}
}
