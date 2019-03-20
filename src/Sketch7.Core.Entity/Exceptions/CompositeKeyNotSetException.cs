using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity
{
	[Serializable]
	public class CompositeKeyNotSetException : Sketch7Exception
	{
		public CompositeKeyNotSetException() : base("Composite keys are not set.")
		{
		}
	}
}