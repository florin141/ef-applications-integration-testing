﻿using Highway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Domain
{
	public class Cart : IIdentifiable<int>
	{
		int IIdentifiable<int>.Id
		{
			get { return CartId; }
			set { CartId = value; }
		}

		private Cart()
		{
			CartItems = new List<CartItem>();
		}

		public int CartId { get; private set; }

		public User User { get; private set; }
		public int UserId { get; private set; }

		public ICollection<CartItem> CartItems { get; }

		public DateTime CreatedAt { get; private set; }

		public static Cart Create(int userId)
		{
			return new Cart
			{
				UserId = userId,
				CreatedAt = DateTime.Now
			};
		}

		public void AddItem(CatalogItem catalogItem, int quantity)
		{
			CartItem cartItem = CartItems
				.Where(c => c.CatalogItem == catalogItem)
				.FirstOrDefault();

			if (cartItem == null)
			{
				cartItem = CartItem.Create(catalogItem);
				CartItems.Add(cartItem);
			}

			cartItem.IncreaseQuantity(quantity);
		}
	}
}
