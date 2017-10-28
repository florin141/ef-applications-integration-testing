using Globalmantics.DAL;
using Globalmantics.Domain;
using Globalmantics.Logic;
using Highway.Data;
using System;

namespace Globalmantics.IntegrationTests
{
	public class CartServiceContext
	{
		public CartServiceContext()
		{
		}

		public DataContext Context { get; set; }
		public UserService UserService { get; set; }
		public CartService CartService { get; set; }
		public string EmailAddress { get; } = $"test{Guid.NewGuid().ToString()}@globalmantics.com";

		public static CartServiceContext GivenServices()
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

		public Cart WhenLoadCart()
		{
			User user = UserService.GetUserByEmail(EmailAddress);
			Context.Commit();

			var cart = CartService.GetCartForUser(user);
			Context.SaveChanges();
			return cart;
		}

		public void WhenAddItemToCart(Cart cart, int quantity = 1)
		{
			CartService.AddItemToCart(cart, "CAFE-314", quantity);
			Context.SaveChanges();
		}
	}
}