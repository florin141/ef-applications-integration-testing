using FluentAssertions;
using Globalmantics.DAL;
using Globalmantics.DAL.Entities;
using Globalmantics.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.IntegrationTests
{
	[TestFixture]
	public class UserServiceTests
	{
		[Test]
		public void Can_create_user()
		{
			var context = new GlobalmanticsContext();
			var userService = new UserService(context);

			User user = userService.GetUserByEmail("test@globalmantics.com");
			context.SaveChanges();

			user.UserId.Should().NotBe(0);
			user.Email.Should().Be("test@globalmantics.com");
		}
	}
}
