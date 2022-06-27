using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace thing_list
{
    public partial class Main_page : Page
    {
        MainWindow window1;
        Application_Context db;
        Add_page add_Page;
        string def_search = "поиск";
        List<Data_thing> things = new List<Data_thing>();
        List<Data_thing> lower_things = new List<Data_thing>();
        public Main_page(MainWindow _window1)
        {
            InitializeComponent();
            window1 = _window1;
            db = new Application_Context();
            Update_addPage();

            
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

        private void list_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Data_thing thing =  (Data_thing)e.Row.DataContext;
            Thing sel_t = db.Things.Find(thing.id);
            if (sel_t.date != null)
                e.Row.Foreground = new SolidColorBrush(Colors.Blue);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(list != null)
            {

                var filter_name = things.Where(t => t.Name.ToLower().Contains(search.Text.ToLower()));
                var filter_number = things.Where(t => t.Number.ToLower().Contains(search.Text.ToLower()));
                var filter_count = things.Where(t => t.Count.ToString().ToLower().Contains(search.Text.ToLower()));
                var filter_location = things.Where(t => t.Location.ToLower().Contains(search.Text.ToLower()));
                var filter_tag = things.Where(t => t.Tag.ToLower().Contains(search.Text.ToLower()));

                var new_list = filter_name.Concat(filter_number).Concat(filter_location).Concat(filter_tag).Concat(filter_count);
                list.ItemsSource = new_list;
            }
            
        }
    }
}
