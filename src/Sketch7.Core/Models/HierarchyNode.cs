using System.Collections.Generic;

namespace Sketch7.Core.Models
{
	/// <summary>
	/// 	Hierarchy node class which contains a nested collection of hierarchy nodes
	/// </summary>
	/// <typeparam name="T"> Entity </typeparam>
	public class HierarchyNode<T> where T : class
	{
		/// <summary>
		/// 	Gets or sets the Entity of your model.
		/// </summary>
		public T Entity { get; set; }

		/// <summary>
		/// 	Gets or sets the ChildNodes list relative to the hierarchy.
		/// </summary>
		public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }

		/// <summary>
		/// 	Gets or sets the Depth of the hierarchy.
		/// </summary>
		public int Depth { get; set; }

		/// <summary>
		/// 	Gets or sets the Parent relative to the <see cref="Entity" /> .
		/// </summary>
		public T Parent { get; set; }
	}
}