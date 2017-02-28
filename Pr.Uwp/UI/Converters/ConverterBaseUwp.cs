using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Pr.Uwp.UI.Converters
{
    public abstract class ConverterBase<TIn, TOut> : DependencyObject, IValueConverter
    {
        protected struct ConverterContext
        {
            public Type TargetType;

            public object Parameter;

            public string Language;
        }

        protected ConverterContext Context { get; set; }

        public abstract TOut ConvertCore(TIn value);

        public virtual TIn ConvertBackCore(TOut value)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Context = new ConverterContext { Language = language, Parameter = parameter, TargetType = targetType };
            return ConvertCore((TIn)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Context = new ConverterContext { Language = language, Parameter = parameter, TargetType = targetType };
            return ConvertBackCore((TOut)value);
        }
    }
}
