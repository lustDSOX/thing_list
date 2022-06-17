using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace thing_list
{
    /// <summary>
    /// Логика взаимодействия для Main_page.xaml
    /// </summary>
    public partial class Main_page : Page
    {
        public MainWindow window1;
        Application_Context db;
        Add_page add_Page;
        public Main_page(MainWindow _window1)
        {
            InitializeComponent();
            window1 = _window1;
            db = new Application_Context();
            // Update_list(false);
            Update_addPage();
        }

        private void Update_addPage()
        {
            add_Page = new Add_page(window1, db, this);
            add_Page.Update_ListTags(false);
            add_Page.Update_ListLocations(false);
            add_Page.Update_ListEmployees(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window1.frame.Navigate(add_Page);
        }

        public void Update_list(bool last)
        {
            List<Thing> things = db.Things.ToList();
            if (!last)
            {
                foreach (Thing thing in things)
                {
                    Grid grid = new Grid();

                    ColumnDefinition column = new ColumnDefinition();
                    Label label = new Label();
                    label.Style = (Style)Resources["LabelStyle2"];
                    for (int i = 0; i < 5; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                label.Content = thing.name;
                                break;
                            case 1:
                                label.Content = thing.number;
                                column.Width = new GridLength(0.5, GridUnitType.Star);
                                break;
                            case 2:
                                label.Content = thing.count;
                                column.Width = new GridLength(0.1, GridUnitType.Star);
                                break;
                            case 3:
                                List<Location> locations = db.Locations.ToList();
                                foreach (Location item in locations)
                                {
                                    if (thing.id_location == item.id)
                                        label.Content = item.name;
                                    break;
                                }
                                break;
                            case 4:
                                column.Width = new GridLength(0.8, GridUnitType.Star);
                                var tags = thing.Tags.ToArray();
                                foreach (var tag in tags)
                                {
                                    label.Content += tag.name + "; ";
                                }
                                break;
                        }
                        grid.ColumnDefinitions.Add(column);
                        column = new ColumnDefinition();
                        grid.Children.Add(label);
                        Grid.SetColumn(label, i);
                        label = new Label();
                        label.Style = (Style)Resources["LabelStyle2"];
                    }
                    list.Children.Add(grid);
                }
            }
            else
            {


            }
        }
    }
}
