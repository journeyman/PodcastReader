using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Magellan.WP.Controls
{
    /// <summary>
    /// Represents a zone used in a <see cref="Layout"/>.
    /// </summary>
    [ContentProperty("Content")]
    public class Zone : DependencyObject
    {
        public static readonly DependencyProperty ZonePlaceHolderNameProperty = DependencyProperty.Register("ZonePlaceHolderName", typeof(string), typeof(Zone), new PropertyMetadata(string.Empty, ZoneNameSet));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(Zone), new PropertyMetadata(null));


        public string ZonePlaceHolderName
        {
            get { return (string)GetValue(ZonePlaceHolderNameProperty); }
            set { SetValue(ZonePlaceHolderNameProperty, value); }
        }

        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }
        
        [DebuggerNonUserCode]
        private static void ZoneNameSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.OldValue))
            {
                throw new InvalidOperationException("The ZonePlaceHolderName has already been set. Once set, it cannot be changed.");
            }
        }
    }
}