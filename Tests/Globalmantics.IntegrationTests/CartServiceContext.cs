using Globalmantics.Logic;
using Highway.Data;

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
	}
}