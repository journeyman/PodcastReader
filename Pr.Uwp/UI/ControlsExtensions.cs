using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Pr.Phone8.Ui
{
	using System.Windows.Input;

	public static class Ext
	{
		public static readonly DependencyProperty TapCommandProperty = DependencyProperty.RegisterAttached("TapCommand", typeof(ICommand), typeof(Ext), new PropertyMetadata(null, OnTapCommandChanged));

		public static readonly DependencyProperty TapCommandParameterProperty = DependencyProperty.RegisterAttached("TapCommandParameter", typeof(object), typeof(Ext), new PropertyMetadata(null));

		public static ICommand GetTapCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(TapCommandProperty);
		}

		public static void SetTapCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(TapCommandProperty, value);
		}

		public static object GetTapCommandParameter(DependencyObject obj)
		{
			return obj.GetValue(TapCommandParameterProperty);
		}

		public static void SetTapCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(TapCommandParameterProperty, value);
		}

		private static void OnTapCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var frameworkElement = (FrameworkElement)obj;
			frameworkElement.Tapped -= FrameworkElementOnTapped;
			frameworkElement.Tapped += FrameworkElementOnTapped;
		}

		private static void FrameworkElementOnTapped(object sender, TappedRoutedEventArgs e)
		{
			var obj = sender as DependencyObject;

			var command = GetTapCommand(obj);
			var param = GetTapCommandParameter(obj);
			if (command != null && command.CanExecute(param))
			{
				command.Execute(param);

				e.Handled = true;
			}
		}
	}
}
