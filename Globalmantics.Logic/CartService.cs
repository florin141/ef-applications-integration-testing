﻿using Globalmantics.DAL;
using Globalmantics.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.Logic
{
	public class CartService
	{
		private readonly GlobalmanticsContext _context;

		public CartService(GlobalmanticsContext context)
		{
			_context = context;
		}

		public Cart GetCartForUser(User user)
		{
			var cart = _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefault(x => x.UserId == user.UserId);

			if (cart == null)
			{
				cart = _context.Carts.Add(Cart.Create(user.UserId));
			}

			return cart;
		}

		public void AddItemToCart(Cart cart, string sku, int quantity)
		{
			var catalogItem = _context.CatalogItems
				.FirstOrDefault(x => x.Sku == sku);

			cart.CartItems.Add(new CartItem
			{
				CatalogItem = catalogItem,
				Quantity = quantity
			});
		}
	}
}
