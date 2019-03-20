using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity
{
	[Serializable]
	public class DbDeleteHasChildrenException : Sketch7Exception
	{
		public DbDeleteHasChildrenException()
			: base("Entity contains relational children.")
		{
		}
	}
}