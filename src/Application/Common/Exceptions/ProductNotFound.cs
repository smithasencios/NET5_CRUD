﻿using System;

namespace Application.Common.Exceptions
{
	public class ProductNotFound : Exception
	{
		public ProductNotFound(string message) : base(message)
		{

		}
	}
}
