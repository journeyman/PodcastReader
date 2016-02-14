namespace Pr.Phone8.Infrastructure.Converters
{
	public class BoolInverseConverter : ConverterBase<bool, bool>
	{
		public override bool ConvertSafe(bool value)
		{
			return !value;
		}
	}
}