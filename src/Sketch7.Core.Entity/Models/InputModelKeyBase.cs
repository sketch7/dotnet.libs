// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines an Entity base extended by property, which defaults the primary key as <see cref="int" /> .
	/// </summary>
	public abstract class InputModelKeyBase : InputModelKeyBase<int>
	{
		//Use ViewModelKeyBase<TKey> instead.
	}

	/// <summary>
	/// 	Defines an Entity base extended by <see cref="Id" /> property which contains a single Key.
	/// </summary>
	public abstract class InputModelKeyBase<TKey> : InputModelBase
		where TKey : struct
	{
		protected InputModelKeyBase()
		{
		}

		/// <summary>
		/// 	Gets or sets the primary key for the entity.
		/// </summary>
		public TKey Id { get; set; }
	}
}