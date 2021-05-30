using System;

namespace Application.Common.Exceptions
{
	public class InvalidParameter : Exception
	{
		public InvalidParameter(string message): base(message)
		{

		}
	}
}
