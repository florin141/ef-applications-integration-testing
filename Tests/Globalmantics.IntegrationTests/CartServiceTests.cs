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

		[Test]
		public void Can_load_cart_with_one_items()
		{
			var configuration = new GlobalmanticsMappingConfiguration();
			InitializeCartWithOneItem(configuration);

			var context = new DataContext("GlobalmanticsContext", configuration);

			var repository = new Repository(context);
			var userService = new UserService(repository);
			var cartService = new CartService(repository);

			var user = userService.GetUserByEmail("test@globalmantics.com");
			context.Commit();
			var cart = cartService.GetCartForUser(user);
			context.Commit();

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(2);
		}

		private void InitializeCartWithOneItem(IMappingConfiguration configuration)
		{
			var context = new DataContext("GlobalmanticsContext", configuration);
			var user = context.Add(User.Create("test@globalmantics.com"));
			context.Commit();
			var cart = context.Add(Cart.Create(user.UserId));
			var catalogItem = context.AsQueryable<CatalogItem>()
				.Single(x => x.Sku == "CAFE-314");
			cart.AddItem(catalogItem, 2);
			context.Commit();
		}
	}
}
