using Globalmantics.Domain;
using Globalmantics.Logic;

namespace Globalmantics.IntegrationTests
{
	public class CartServiceContext : UserServiceContext
	{
		public CartService CartService { get; set; }

		protected CartServiceContext()
		{
			CartService = new CartService(Repository, new MockLog());
		}

		public static CartServiceContext GivenServices()
		{
			return new CartServiceContext();
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