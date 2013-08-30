using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Magellan.WP.Controls
{
    /// <summary>
    /// Presents a shared layout in the control tree, and merges the <see cref="Zone">zones</see> into the
    /// <see cref="ZonePlaceHolder">zone place holders</see> specified in the layout.
    /// </summary>
    [StyleTypedProperty(Property="ItemContainerStyle", StyleTargetType=typeof(Zone))]
    [ContentProperty("Zones")]
    public class Layout : Control
    {
        private bool sourceLoaded;

        /// <summary>
        /// Initializes the <see cref="Layout"/> class.
        /// </summary>
        public Layout()
        {
            DefaultStyleKey = typeof (Layout);
        
            Loaded += MasterLoaded;
            Zones = new ZoneCollection();
        }

        /// <summary>
        /// Dependency propety for the <see cref="Zones"/> property.
        /// </summary>
        public static readonly DependencyProperty ZonesProperty = DependencyProperty.Register("Zones", typeof(ZoneCollection), typeof(Layout), new PropertyMetadata(null));
        
        /// <summary>
        /// Dependency propety for the <see cref="Source"/> property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(Layout), new PropertyMetadata(string.Empty, SourcePropertySet));

        /// <summary>
        /// Dependency property for the <see cref="Content"/> property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(Layout), new PropertyMetadata(null, ContentChanged));

        /// <summary>
        /// Gets or sets the zones.
        /// </summary>
        /// <value>The zones.</value>
        public ZoneCollection Zones
        {
            get { return (ZoneCollection)GetValue(ZonesProperty); }
            set { SetValue(ZonesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path to the XAML file containing the layout template to use.
        /// </summary>
        /// <value>The source.</value>
        [Category("Appearance")]
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private static void SourcePropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Layout)d;

            @this.sourceLoaded = false;
            @this.LoadLayoutFromSource();
        }
 
        private void MasterLoaded(object sender, RoutedEventArgs e)
        {
            LoadLayoutFromSource();
            ReloadZones();
        }

        private void LoadLayoutFromSource()
        {
            if (!string.IsNullOrEmpty(this.Source) && !this.sourceLoaded)
            {
                this.sourceLoaded = true;
                using (var stream = Application.GetResourceStream(new Uri(this.Source, UriKind.Relative)).Stream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var content = XamlReader.Load(reader.ReadToEnd());
                        this.Content = (FrameworkElement)content;
                    }
                }
            }
        }

        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var layout = (Layout)d;
            layout.ReloadZones();
            layout.UpdateLayout();
        }

        private void ReloadZones()
        {
            if (this.Content != null)
            {
                var list = new List<KeyValuePair<ZonePlaceHolder, Zone>>();
                foreach (var zone in this.Zones)
                {
                    object placeHolder = this.Content.FindName(zone.ZonePlaceHolderName);
                    if (placeHolder == null)
                    {
                        throw new InvalidOperationException(string.Format("A ZonePlaceHolder by the name of '{0}' does not exist on the layout '{1}'.", zone.ZonePlaceHolderName, this.Content));
                    }
                    var holder = placeHolder as ZonePlaceHolder;
                    if (holder == null)
                    {
                        throw new InvalidOperationException(string.Format("The control '{0}' in layout '{1}' is not a ZonePlaceHolder.", zone.ZonePlaceHolderName, this.Content));
                    }
                    list.Add(new KeyValuePair<ZonePlaceHolder, Zone>(holder, zone));
                }

                foreach (var pair in list)
                {
                    pair.Key.Content = pair.Value.Content;
                }
            }
        }
    }
}


