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
			CartServiceContext services = GivenServices();

			User user = GivenUser(services.Context, services.UserService, 
				services.EmailAddress);

			var cart = services.CartService.GetCartForUser(user);
			services.Context.SaveChanges();

			cart.CartItems.Count().Should().Be(0);
		}

		[Test]
		public void Can_add_item_to_cart()
		{
			CartServiceContext services = GivenServices();

			Cart cart = WhenLoadCart(services);

			services.CartService.AddItemToCart(cart, "CAFE-314", 2);
			services.Context.SaveChanges();

			cart.CartItems.Count().Should().Be(1);
		}

		[Test]
		public void Group_items_of_same_type()
		{
			CartServiceContext services = GivenServices();

			Cart cart = WhenLoadCart(services);

			services.CartService.AddItemToCart(cart, "CAFE-314", 2);
			services.CartService.AddItemToCart(cart, "CAFE-314", 1);
			services.Context.SaveChanges();

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(3);
		}

		[Test]
		public void Can_load_cart_with_one_item()
		{
			var services = GivenServices();
			InitializeCartWithOneItem(services.EmailAddress);

			Cart cart = WhenLoadCart(services);

			cart.CartItems.Count().Should().Be(1);
			cart.CartItems.Single().Quantity.Should().Be(2);
		}

		private static Cart WhenLoadCart(CartServiceContext services)
		{
			User user = GivenUser(services.Context, services.UserService, 
				services.EmailAddress);

			var cart = services.CartService.GetCartForUser(user);
			services.Context.SaveChanges();
			return cart;
		}

		private static CartServiceContext GivenServices()
		{
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var repository = new Repository(context);
			var userService = new UserService(repository);
			CartService cartService = new CartService(repository, new MockLog());

			var services = new CartServiceContext
			{
				Context = context,
				UserService = userService,
				CartService = cartService
			};

			return services;
		}

		private static User GivenUser(IUnitOfWork unitOfWork, UserService userService, string emailAddress)
		{
			var user = userService.GetUserByEmail(emailAddress);
			unitOfWork.Commit();
			return user;
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
