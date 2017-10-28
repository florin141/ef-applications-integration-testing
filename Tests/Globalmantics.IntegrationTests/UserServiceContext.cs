using Globalmantics.Domain;
using Globalmantics.Logic;
using System;

namespace Globalmantics.IntegrationTests
{
	public class UserServiceContext : TestContext
	{
		public UserService UserService { get; }

		public string EmailAddress { get; }

		protected UserServiceContext()
		{
			UserService = new UserService(Repository);
			EmailAddress = $"test{Guid.NewGuid().ToString()}@globalmantics.com";
		}

		public static UserServiceContext GiveServices()
		{
			return new UserServiceContext();
		}

		public User WhenGetUserByEmail()
		{
			var user = UserService.GetUserByEmail(EmailAddress);

			Context.SaveChanges();

			return user;
		}
	}
}
