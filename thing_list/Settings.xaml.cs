using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorHelper;
using ColorConverter = ColorHelper.ColorConverter;

namespace thing_list
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        
        public Settings()
        {
            InitializeComponent();
            List<Text_border> text_Border = new List<Text_border>();
            text_Border.Add(new Text_border());
            text_Border.Add(new Text_border());
            text_Border.Add(new Text_border());
            text_Border.Add(new Text_border());
            text_Border.Add(new Text_border());
            border.ItemsSource = text_Border;
            All_settings settings = new All_settings();
            this.DataContext = settings;
        }

        class Text_border
        {
            public string Text { get; set; } = "Текст";
        }
        private void border_color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            border.HorizontalGridLinesBrush = new SolidColorBrush((Color)border_color.SelectedColor);
            border.VerticalGridLinesBrush = new SolidColorBrush((Color)border_color.SelectedColor);
            Style style = new Style();
            Trigger trigger = new Trigger();
            trigger.Property = UIElement.IsMouseOverProperty;
            trigger.Value = true;
            Setter setter = new Setter();
            setter.Property = BackgroundProperty;
            setter.Value = new SolidColorBrush((Color)border_color.SelectedColor);
            trigger.Setters.Add(setter);
            style.Triggers.Add(trigger);
            Setter setter1 = new Setter();
            setter1.Property = BackgroundProperty;
            setter1.Value = Brushes.Transparent;
            style.Setters.Add(setter1);
            border.RowStyle = style;
        }

        private void background_color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            border.Background = new SolidColorBrush((Color)background_color.SelectedColor);
        }

        private void ui_color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ui_element.Background = new SolidColorBrush((Color)ui_color.SelectedColor);
        }
    }
}
