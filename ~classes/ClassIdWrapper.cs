namespace Ans.Net10.Common
{

	public class ClassIdWrapper<T>
	{
		public ClassIdWrapper(
			int id,
			T item)
		{
			Id = id;
			Item = item;
		}
		public int Id { get; }
		public T Item { get; }
	}

}
