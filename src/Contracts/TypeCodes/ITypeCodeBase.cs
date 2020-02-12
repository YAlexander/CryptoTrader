namespace Contracts.TypeCodes
{
	/// <summary>
	/// Base interface for type codes. Please don't forget to modify baseAsm.DefinedTypes if you create codes with Code other than int or string
	/// </summary>
	/// <typeparam name="T">Type of code property</typeparam>
	public interface ITypeCodeBase<out T>
	{
		/// <summary>
		/// Human readable description of the item
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Unique item code
		/// </summary>
		T Code { get; }
	}
}