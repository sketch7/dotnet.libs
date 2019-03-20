namespace Sketch7.Core.Models
{
	public class KeyValue
	{
		public int Key { get; set; }
		public string Value { get; set; }
	}

	public class KeyValue<TKey, TValue>
	{
		public TKey Key { get; set; }
		public TValue Value { get; set; }
	}
}