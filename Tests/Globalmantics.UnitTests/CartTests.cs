using FluentAssertions;
using Globalmantics.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.UnitTests
{
	[TestFixture]
    public class CartTests
    {
		[Test]
		public void Cart_is_initially_empty()
		{
			var cart = Cart.Create(0);

			cart.CartItems.Count().Should().Be(0);
		}
    }
}
