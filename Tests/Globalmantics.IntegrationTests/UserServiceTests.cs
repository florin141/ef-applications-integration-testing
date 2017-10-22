﻿using FluentAssertions;
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
			var configuration = new GlobalmanticsMappingConfiguration();
			var context = new DataContext("GlobalmanticsContext", configuration);
			var repository = new Repository(context);
			var userService = new UserService(repository);

			User user = userService.GetUserByEmail("test@globalmantics.com");
			context.SaveChanges();

			user.UserId.Should().NotBe(0);
			user.Email.Should().Be("test@globalmantics.com");
		}
	}
}
