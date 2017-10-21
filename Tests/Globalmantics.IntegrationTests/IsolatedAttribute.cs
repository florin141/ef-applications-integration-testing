using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Transactions;

namespace Globalmantics.IntegrationTests
{
	public class IsolatedAttribute : Attribute, ITestAction
	{
		public ActionTargets Targets => ActionTargets.Test;

		private TransactionScope _transactionScope;

		public void AfterTest(ITest test)
		{
			if (_transactionScope != null)
				_transactionScope.Dispose();
			_transactionScope = null;
		}

		public void BeforeTest(ITest test)
		{
			_transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
		}
	}
}
