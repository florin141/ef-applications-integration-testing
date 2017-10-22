using FluentAssertions;
using Globalmantics.DAL;
using Globalmantics.Domain;
using Globalmantics.Logic;
using Highway.Data;
using NUnit.Framework;
using System;
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
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var repository = new Repository(context);
			var userService = new UserService(repository);
			var cartService = new CartService(repository);

			User user = GivenUser(context, userService);

			var cart = cartService.GetCartForUser(user);
			context.SaveChanges();

			cart.CartItems.Count().Should().Be(0);
		}

		private static User GivenUser(IUnitOfWork unitOfWork, UserService userService)
		{
			var user = userService.GetUserByEmail($"test{Guid.NewGuid().ToString()}@globalmantics.com");
			unitOfWork.Commit();
			return user;
		}

		[Test]
		public void Can_add_item_to_cart()
		{
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var repository = new Repository(context);
			var userService = new UserService(repository);
			var cartService = new CartService(repository);

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
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var repository = new Repository(context);
			var userService = new UserService(repository);
			var cartService = new CartService(repository);

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
