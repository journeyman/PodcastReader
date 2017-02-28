namespace Pr.Uwp.UI.Converters
{
	public class BoolInverseConverter : ConverterBase<bool, bool>
	{
	    public override bool ConvertCore(bool value)
	    {
			return !value;
        }
    }
}