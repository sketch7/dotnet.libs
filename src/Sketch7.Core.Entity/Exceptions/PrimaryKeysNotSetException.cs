using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity
{
	[Serializable]
	public class PrimaryKeysNotSetException : Sketch7Exception
	{
		public PrimaryKeysNotSetException()
			: base("Primary key(s) are not set.")
		{
		}
	}
}