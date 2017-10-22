using Globalmantics.Domain;
using System.Linq;
using Highway.Data;
using Globalmantics.Logic.Queries;

namespace Globalmantics.Logic
{
	public class UserService
	{
		private readonly IRepository _repository;

		public UserService(IRepository repository)
		{
			_repository = repository;
		}

		public User GetUserByEmail(string emailAddress)
		{
			var user = _repository.Find(new UserByEmail(emailAddress));

			if (user == null)
			{
				user = _repository.Context.Add(User.Create(emailAddress));
			}

			return user;
		}
	}
}
