// rev 2026-09-18

namespace Ans.Net10.Common
{

	/// <summary>
	/// Представляет легковесный контейнер-обертку для связывания целочисленного идентификатора и объекта.
	/// </summary>
	/// <typeparam name="T">Тип инкапсулируемого объекта.</typeparam>
	public readonly record struct ClassIdWrapper<T>(
		int Id,
		T? Item)
	{
	}

	//public class ClassIdWrapper<T>
	//{
	//	public ClassIdWrapper(
	//		int id,
	//		T item)
	//	{
	//		Id = id;
	//		Item = item;
	//	}

	//	public int Id { get; }
	//	public T Item { get; }
	//}

}
