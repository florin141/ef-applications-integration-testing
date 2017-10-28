using FluentAssertions;
using Globalmantics.DAL;
using Globalmantics.Domain;
using Globalmantics.Logic;
using Highway.Data;
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
			UserServiceContext service = UserServiceContext.GiveServices();

			User user = service.WhenGetUserByEmail();

			user.UserId.Should().NotBe(0);
			user.Email.Should().Be(service.EmailAddress);
		}
	}
}
