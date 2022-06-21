using Microsoft.EntityFrameworkCore;
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
    public partial class Main_page : Page
    {
        MainWindow window1;
        Application_Context db;
        Add_page add_Page;
        string def_search = "поиск";
        string sort_name = "sort";
        const int searchFor_name = 0;
        const int searchFor_number = 1;
        const int searchFor_location = 3;
        const int searchFor_count = 2;
        public Main_page(MainWindow _window1)
        {
            InitializeComponent();
            window1 = _window1;
            db = new Application_Context();
            Update_list(false,null);
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
            add_Page.Clear();
            window1.frame.Navigate(add_Page);
        }

        public void Update_list(bool last,List<Thing> things)
        {
            List<Thing> all_things;
            if (things != null)
                all_things = things;
            else
                all_things = db.Things.Include(t=>t.Tags).ToList();

            if (!last)
            {
                foreach (Thing thing in all_things)
                {
                    Grid grid = new Grid();
                    grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
                    grid.Style = FindResource("GridStyle") as Style;

                    ColumnDefinition column = new ColumnDefinition();
                    Label label = new Label();
                   
                    label.BorderThickness = new Thickness(0, 0, 1, 1);
                    if (thing.date != null)
                        label.Style = (Style)Resources["LabelStyle2.2"];

                    else
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
                                    foreach (Thing t in item.Things)
                                    {
                                        if (t.id == thing.id)
                                        {
                                            label.Content = item.name;
                                            break;
                                        }
                                    }
                                }
                                break;
                            case 4:
                                column.Width = new GridLength(0.8, GridUnitType.Star);
                                foreach (Tag tag in thing.Tags)
                                {
                                    label.Content += tag.name + "; ";
                                }
                                break;
                        }
                        label.BorderThickness = new Thickness(0, 0, 1, 1);
                       
                        grid.ColumnDefinitions.Add(column);
                        column = new ColumnDefinition();
                        grid.Children.Add(label);
                        Grid.SetColumn(label, i);
                        label = new Label();
                        if (thing.date != null)
                            label.Style = (Style)Resources["LabelStyle2.2"];
                        else
                            label.Style = (Style)Resources["LabelStyle2"];
                    }
                    list.Children.Add(grid);
                }
            }
            else
            {
                Thing thing = db.Things.LastOrDefault();
                Grid grid = new Grid();
                grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
                grid.Style = FindResource("GridStyle") as Style;

                ColumnDefinition column = new ColumnDefinition();
                Label label = new Label();
                if (thing.date != null)
                    label.Style = (Style)Resources["LabelStyle2.2"];
                else
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
                                foreach (Thing t in item.Things)
                                {
                                    if (t.id == thing.id)
                                        label.Content = item.name;
                                    break;
                                }
                            }
                            break;
                        case 4:
                            column.Width = new GridLength(0.8, GridUnitType.Star);
                            foreach (Tag tag in thing.Tags)
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
                    if (thing.date != null)
                        label.Style = (Style)Resources["LabelStyle2.2"];
                    else
                        label.Style = (Style)Resources["LabelStyle2"];
                }
                list.Children.Add(grid);

            }
        }

        public void Update_thing(Grid grid,Thing thing)
        {
            if (thing.date != null)
            {
                foreach (Label item in grid.Children)
                {
                    item.Style = (Style)Resources["LabelStyle2.2"];
                }
            }
            ((Label)grid.Children[0]).Content = thing.name;
            ((Label)grid.Children[1]).Content = thing.number;
            ((Label)grid.Children[2]).Content = thing.count;
            foreach (Location location in db.Locations)
            {
                foreach (Thing t in location.Things)
                {
                    if (t.id == thing.id)
                        ((Label)grid.Children[3]).Content = location.name;
                    break;
                }
            }
            ((Label)grid.Children[4]).Content = "";
            foreach (Tag tag in thing.Tags)
            {
                ((Label)grid.Children[4]).Content += tag.name + "; ";
            }

        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount >= 2)
            {
                Grid grid = sender as Grid;
                foreach (Thing item in db.Things)
                {
                    if (item.name == ((Label)grid.Children[0]).Content && item.number == ((Label)grid.Children[1]).Content && item.count.ToString() == ((Label)grid.Children[2]).Content.ToString())
                    {
                        add_Page.Clear();
                        add_Page.Edit_thing(item, grid);
                        window1.frame.Navigate(add_Page);
                        break;
                    }
                }
            }
           
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
            if(comboBox.Name == sort_name)
                sort_sel.Content = "cортировать по";
            else
                search_sel.Content = "искать в";

        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(e.Key == Key.Enter && textBox.Text != null)
            {
                List<Thing> things;
                switch (search.SelectedIndex)
                {
                    case searchFor_name:
                        things = db.Things.Where(t => t!.name == textBox.Text).ToList();
                        if (things.Count == 0)
                            MessageBox.Show("Ничего не найдено");
                        else
                        {
                            list.Children.Clear();
                            Update_list(false, things);
                        }
                        break;

                    case searchFor_count:
                        try
                        {
                            int count = Convert.ToInt32(textBox.Text);
                            things = db.Things.Where(t => t!.count == count).ToList();
                            if (things.Count == 0)
                                MessageBox.Show("Ничего не найдено");
                            else
                            {
                                list.Children.Clear();
                                Update_list(false, things);
                            }
                        }
                        catch { }
                        break;

                    case searchFor_location:
                        bool find = true;
                        foreach (Location location in db.Locations)
                        {
                            if(location.name == textBox.Text)
                            {
                                find = true;
                                things = location.Things;
                                if (things.Count == 0)
                                    MessageBox.Show("Ничего не найдено");
                                else
                                {
                                    list.Children.Clear();
                                    Update_list(false, things);
                                }
                                break;
                            }
                            else
                                find = false;
                        }
                        if(!find)
                            MessageBox.Show("Ничего не найдено");
                        break;

                    case searchFor_number:
                        things = db.Things.Where(t => t!.number == textBox.Text).ToList();
                        if (things.Count == 0)
                            MessageBox.Show("Ничего не найдено");
                        else
                        {
                            list.Children.Clear();
                            Update_list(false, things);
                        }
                        break;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            list.Children.Clear();
            Update_list(false, null);
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
    }
}
