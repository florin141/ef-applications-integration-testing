using Globalmantics.Domain;
using Highway.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Logic.Queries
{
	public class CartForUser : Scalar<Cart>
	{
		public CartForUser(int userId)
		{
			ContextQuery = context => context.AsQueryable<Cart>()
				.Include(c => c.CartItems)
				.FirstOrDefault(x => x.UserId == userId);
		}
	}
}
