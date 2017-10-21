using Globalmantics.DAL;
using Globalmantics.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Logic
{
	public class UserService
	{
		private readonly GlobalmanticsContext _context;

		public UserService(GlobalmanticsContext context)
		{
			_context = context;
		}

		public User GetUserByEmail(string emailAddress)
		{
			var user = _context.Users.Add(new User
			{
				Email = emailAddress
			});

			return user;
		}
	}
}
