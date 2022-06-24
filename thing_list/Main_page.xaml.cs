using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace thing_list
{
    public partial class Main_page : Page
    {
        MainWindow window1;
        Application_Context db;
        Add_page add_Page;
        string def_search = "поиск";
        string sort_name = "sort";
        const int for_name = 0;
        const int for_number = 1;
        const int for_count = 2;
        const int for_location = 3;
        const int for_freedom = 4;
        public Main_page(MainWindow _window1)
        {
            InitializeComponent();
            window1 = _window1;
            db = new Application_Context();
            Update_addPage();
            List<Data_thing> things = new List<Data_thing>();

            foreach (Location location in db.Locations.Include(l => l.Things).ToList())
            {
                foreach (Thing thing in location.Things)
                {
                    Data_thing data_Thing = new Data_thing();
                    data_Thing.Date = thing.date;
                    //data_Thing.Comment = thing.comment;
                   
                    data_Thing.id = thing.id;
                    data_Thing.Location = location.name;
                    data_Thing.Number = thing.number;
                    data_Thing.Name = thing.name;
                    data_Thing.Count = thing.count;

                    string tags = "";
                    foreach (Tag tag in db.Tags.Include(t => t.Things))
                    {
                        foreach (Thing t in tag.Things)
                        {
                            if (t.id == thing.id)
                            {
                                tags += tag.name + ";";
                            }
                        }
                    }
                    data_Thing.Tag = tags;
                    things.Add(data_Thing);
                }
            }
            list.ItemsSource = things;
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
            add_Page.Clear();
            window1.frame.Navigate(add_Page);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Data_thing thing = list.SelectedItem as Data_thing;
            Thing item = db.Things.Find(thing.id);

            add_Page.Clear();
            add_Page.Edit_thing(item, thing);
            window1.frame.Navigate(add_Page);

        }
        private void Got_focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == def_search)
                box.Text = "";
        }

        private void Lost_focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "")
                box.Text = def_search;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.Name == sort_name)
                sort_sel.Content = "cортировать по";
            else
                search_sel.Content = "искать в";

        }

        private void Switch_sort(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            RotateTransform s = (RotateTransform)sort_method.LayoutTransform;
            if (s.Angle == 270)
            {
                RotateTransform rotate = new RotateTransform(90);
                sort_method.LayoutTransform = rotate;
            }

            else
            {
                RotateTransform rotate = new RotateTransform(270);
                sort_method.LayoutTransform = rotate;
            }

        }

        private void list_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Data_thing thing =  (Data_thing)e.Row.DataContext;
            Thing sel_t = db.Things.Find(thing.id);
            if (sel_t.date != null)
                e.Row.Foreground = new SolidColorBrush(Colors.Blue);
        }

        //private void Sort(object sender, RoutedEventArgs e)
        //{
        //    RotateTransform s = (RotateTransform)sort_method.LayoutTransform;
        //    List<Thing> things;
        //    if (s.Angle == 270)
        //    {
        //        switch (sort.SelectedIndex)
        //        {
        //            case for_count:
        //                things = db.Things.OrderByDescending(t => t.count).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_freedom:
        //                things = db.Things.OrderByDescending(t => t.date).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_location:
        //                things = new List<Thing>();
        //                foreach (Location location in db.Locations.OrderByDescending(l=>l.name))
        //                {
        //                    foreach (Thing thing in location.Things)
        //                    {
        //                        things.Add(thing);
        //                    }
        //                }
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_name:
        //                things = db.Things.OrderByDescending(t => t.name).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_number:
        //                things = db.Things.OrderByDescending(t => t.number).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //        }
        //    }

        //    else
        //    {
        //        switch (sort.SelectedIndex)
        //        {
        //            case for_count:
        //                things = db.Things.OrderBy(t => t.count).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_freedom:
        //                things = db.Things.OrderBy(t => t.date).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_location:
        //                things = new List<Thing>();
        //                foreach (Location location in db.Locations.OrderBy(l => l.name))
        //                {
        //                    foreach (Thing thing in location.Things)
        //                    {
        //                        things.Add(thing);
        //                    }
        //                }
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_name:
        //                things = db.Things.OrderBy(t => t.name).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //            case for_number:
        //                things = db.Things.OrderBy(t => t.number).ToList();
        //                list.Children.Clear();
        //                Update_list(false, things);
        //                break;
        //        }
        //    }
        //}
    }
}
