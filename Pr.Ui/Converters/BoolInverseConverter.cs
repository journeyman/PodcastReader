namespace Pr.Ui.Converters
{
	public class BoolInverseConverter : ConverterBase<bool, bool>
	{
		public override bool ConvertSafe(bool value)
		{
			return !value;
		}
	}
}