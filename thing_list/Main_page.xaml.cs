using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
        string def_search = "Поиск";
        List<Data_thing> things = new List<Data_thing>();
        public Main_page(MainWindow _window1)
        {
            InitializeComponent();
            window1 = _window1;
            db = new Application_Context();
            Update_addPage();
            Update_list(false);
        }


        public void Update_list(bool last)
        {
            if (!last)
            {
                foreach (Thing thing in db.Things.Include(l => l.Tags))
                {
                    Data_thing data_Thing = new Data_thing();

                    data_Thing.id = thing.id;
                    foreach (Location location in db.Locations)
                    {
                        if (location.Things.Where(t => t.id == thing.id).Count() != 0)
                        {
                            data_Thing.Location = location.name;
                            break;
                        }
                    }
                    data_Thing.Number = thing.number;
                    data_Thing.Name = thing.name;
                    data_Thing.Count = thing.count;

                    string tags = "";
                    int count = 1;
                    foreach (Tag tag in thing.Tags)
                    {
                        if (count == thing.Tags.Count)
                            tags += tag.name;
                        else
                            tags += tag.name + "; ";
                        count++;
                    }
                    data_Thing.Tag = tags;
                    things.Add(data_Thing);
                }

                list.ItemsSource = things;
            }
            else
            {
                Data_thing data_Thing = new Data_thing();
                Thing thing = db.Things.OrderBy(t => t.id).Last();
                data_Thing.id = thing.id;
                data_Thing.Name = thing.name;
                data_Thing.Number = thing.number;
                data_Thing.Count = thing.count;
                foreach (Location location in db.Locations)
                {
                    if (location.Things.Where(t => t.id == thing.id).Count() != 0)
                    {
                        data_Thing.Location = location.name;
                        break;
                    }
                }
                string tags = "";
                int count = 1;
                foreach (Tag tag in thing.Tags)
                {
                    if (count == thing.Tags.Count)
                        tags += tag.name;
                    else
                        tags += tag.name + "; ";
                }
                data_Thing.Tag = tags;
                things.Add(data_Thing);
                list.Items.Refresh();
            }
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
            Data_thing thing = list.SelectedValue as Data_thing;
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
            List<Taken_things> taken_s = db.Taken_Things.ToList();
            Data_thing thing = (Data_thing)e.Row.DataContext;
            Thing sel_t = db.Things.Find(thing.id);
            if (sel_t.Taken_Things.Count != 0)
                e.Row.Foreground = Brushes.LightBlue;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (search.Text != def_search && search.Text != "" && list != null)
            {
                var filter_name = things.Where(t => t.Name.ToLower().Contains(search.Text.ToLower()));
                var filter_number = things.Where(t => t.Number.ToLower().Contains(search.Text.ToLower()));
                var filter_count = things.Where(t => t.Count.ToString().ToLower().Contains(search.Text.ToLower()));
                var filter_location = things.Where(t => t.Location.ToLower().Contains(search.Text.ToLower()));
                var filter_tag = things.Where(t => t.Tag.ToLower().Contains(search.Text.ToLower()));

                var new_list = filter_name.Concat(filter_number).Concat(filter_location).Concat(filter_tag).Concat(filter_count);
                list.ItemsSource = new_list;
            }
            else
            {
                if (list != null)
                    list.ItemsSource = things;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("columnW.json");
                dynamic gridLayout = JsonConvert.DeserializeObject(sr.ReadLine());
                foreach (var column in gridLayout.columns)
                {
                    var gridColumn = list.Columns.FirstOrDefault(c => c.Header?.ToString() == column.header?.Value);
                    gridColumn.Width = column.width;
                }
            }
            catch { }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var gridLayout = new
                {
                    columns = list.Columns.Select(c => new { header = c.Header?.ToString(), width = c.Width }).ToArray()
                };
                StreamWriter sw = new StreamWriter("columnW.json");
                sw.WriteLine(JsonConvert.SerializeObject(gridLayout));
                sw.Close(); ;
            }
            catch { }
        }

        private void Open_settings(object sender, RoutedEventArgs e)
        {
            window1.frame.Navigate(new Settings());
        }
    }
}
