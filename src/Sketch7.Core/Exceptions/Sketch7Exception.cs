using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	[Serializable]
	public class Sketch7Exception : Exception
	{
		public Sketch7Exception()
		{
		}

		public Sketch7Exception(string message) : base(message)
		{
		}

		public Sketch7Exception(string message, Exception inner) : base(message, inner)
		{
		}

		protected Sketch7Exception(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}