using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace thing_list
{
    public partial class Add_page : Page
    {
        Application_Context db;
        public MainWindow window1;
        Main_page main_Page;
        public int tag_col = 0;
        public int tag_row = 1;
        public Grid editing_grid;
        public Thing editing_thing;
        const string tagBtn_name = "add_tagBtn";
        const string locBtn_name = "add_locBtn";
        const string emplBtn_name = "add_emplBtn";
        const string addBtn = "Добавить запись";
        const string editBtn = "Изменить запись";

        public Add_page(MainWindow _window1, Application_Context context, Main_page main)
        {
            InitializeComponent();
            db = context; ;
            window1 = _window1;
            main_Page = main;
        }

        public void Clear()
        {
            name.Clear();
            number.Clear();
            ComboBox_employee.Text = "";
            ComboBox_location.Text = "";
            count_things.Text = "0";
            date.Text = "";
            select_tags.Children.RemoveRange(0, select_tags.Children.Count);
            Add_btn.Content = "Добавить запись";
            tag_col = 0;
            tag_row = 1;
        }
        public void Edit_thing(Thing thing, Grid grid)
        {
            editing_thing = thing;
            editing_grid = grid;
            name.Text = thing.name;
            number.Text = thing.number;
            foreach (Location location in db.Locations)
            {
                foreach (Thing item in location.Things)
                {
                    if (item.id == thing.id)
                    {
                        ComboBox_location.Text = location.name;
                        break;
                    }
                }
            }
            count_things.Text = thing.count.ToString();
            foreach (Tag tag in thing.Tags)
            {
                Add_selected_tag(tag.name);
            }
            foreach (Employee employee in db.Employees)
            {
                foreach (Thing thing_emp in employee.Things)
                {
                    if (thing_emp.id == thing.id)
                        ComboBox_employee.Text = $"{employee.name} {employee.surname} {employee.patronymic}";
                }

            }

            if (thing.date == null)
                date.SelectedDate = DateTime.Now;
            else
                date.Text = thing.date;

            Add_btn.Content = "Изменить запись";
        }

        public void Update_ListTags(bool last)
        {
            List<Tag> tags = db.Tags.ToList();
            if (!last)
            {
                foreach (Tag tag in tags)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                    item.HorizontalContentAlignment = HorizontalAlignment.Center;
                    item.Foreground = Brushes.White;
                    item.Content = tag.name;
                    ComboBox_tags.Items.Add(item);
                }
            }
            else
            {

                ComboBoxItem item = new ComboBoxItem();
                item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                item.HorizontalContentAlignment = HorizontalAlignment.Center;
                item.Foreground = Brushes.White;
                item.Content = tags.Last().name;
                ComboBox_tags.Items.Add(item);
            }
        }

        public void Update_ListLocations(bool last)
        {
            List<Location> locations = db.Locations.ToList();
            if (!last)
            {
                foreach (Location loc in locations)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                    item.HorizontalContentAlignment = HorizontalAlignment.Center;
                    item.Foreground = Brushes.White;
                    item.Content = loc.name;
                    ComboBox_location.Items.Add(item);
                }
            }
            else
            {

                ComboBoxItem item = new ComboBoxItem();
                item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                item.HorizontalContentAlignment = HorizontalAlignment.Center;
                item.Foreground = Brushes.White;
                item.Content = locations.Last().name;
                ComboBox_location.Items.Add(item);
            }
        }

        public void Update_ListEmployees(bool last)
        {
            List<Employee> employees = db.Employees.ToList();
            if (!last)
            {
                foreach (Employee employee in employees)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                    item.HorizontalContentAlignment = HorizontalAlignment.Center;
                    item.Foreground = Brushes.White;
                    item.Content = $"{employee.surname} {employee.name} {employee.patronymic}";
                    ComboBox_employee.Items.Add(item);
                }
            }
            else
            {

                ComboBoxItem item = new ComboBoxItem();
                item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                item.HorizontalContentAlignment = HorizontalAlignment.Center;
                item.Foreground = Brushes.White;
                item.Content = $"{employees.Last().surname} {employees.Last().name} {employees.Last().patronymic}";
                ComboBox_employee.Items.Add(item);
            }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tag_row < 9)
            {
                try
                {
                    var item = ((ComboBox)sender).SelectedValue;
                    var name_tag = ((ComboBoxItem)item).Content;
                    Add_selected_tag((string)name_tag);
                }
                catch
                {
                    MessageBox.Show("Такой категории не существует");
                }
            }

        }

        private void Add_selected_tag(string name_tag)
        {
            try
            {
                foreach (Grid tag in select_tags.Children)
                {
                    var child = tag.Children;
                    Border name_border = (Border)child[0];
                    Label name = (Label)name_border.Child;
                    if ((string)name.Content == name_tag)
                    {
                        MessageBox.Show("Такая категория уже присвоена");
                        return;
                    }

                }
                Grid grid = new Grid();

                ColumnDefinition columnDefinition0 = new ColumnDefinition();
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                columnDefinition0.Width = new GridLength(4, GridUnitType.Star);
                grid.ColumnDefinitions.Add(columnDefinition0);
                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.Name = $"grid{tag_row}_{tag_col}";

                Label label = new Label();
                label.Foreground = Brushes.White;
                label.Content = name_tag;
                label.Background = Brushes.Transparent;
                label.FontSize = 10;

                Button button = new Button();
                button.Content = "×";
                button.Style = (Style)Resources["ButtonStyle1"];
                button.BorderThickness = new Thickness(0);
                button.Click += Del_tag;


                Border border0 = new Border();
                border0.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#606060"));
                border0.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
                border0.BorderThickness = new Thickness(1);
                border0.CornerRadius = new CornerRadius(5, 0, 0, 5);

                Border border1 = new Border();
                border1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#606060"));
                border1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3958FC"));
                border1.BorderThickness = new Thickness(1);
                border1.CornerRadius = new CornerRadius(0, 5, 5, 0);

                border0.Child = label;
                grid.Children.Add(border0);
                Grid.SetColumn(border0, 0);

                border1.Child = button;
                grid.Children.Add(border1);
                Grid.SetColumn(border1, 1);

                select_tags.Children.Add(grid);
                Grid.SetColumn(grid, tag_col);
                Grid.SetRow(grid, tag_row);

                tag_col += 2;
                if (tag_col > 4)
                {
                    tag_col = 0;
                    tag_row += 2;
                }

            }
            catch { }
        }

        private void Del_tag(object sender, RoutedEventArgs e)
        {
            var grid = ((Button)sender).Parent;
            grid = ((Border)grid).Parent;
            string name = ((Grid)grid).Name;

            foreach (Grid item in select_tags.Children)
            {
                if (item.Name == name)
                {
                    select_tags.Children.Remove(item);
                    break;
                }
            }
            tag_col = 0;
            tag_row = 1;

            foreach (Grid item in select_tags.Children)
            {

                Grid.SetColumn(item, tag_col);
                Grid.SetRow(item, tag_row);
                tag_col += 2;
                if (tag_col > 4)
                {
                    tag_col = 0;
                    tag_row += 2;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string old_text = ((TextBox)sender).Text;
            string pattern = "(?i)[a-zа-я]";
            try
            {
                int i = Convert.ToInt32(old_text);
            }
            catch
            {
                ((TextBox)sender).Text = Regex.Replace(old_text, pattern, String.Empty); ;
            }
        }

        private void Open_Add_0_Page(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case tagBtn_name:
                    Add_tag add_tag_window = new Add_tag(this, db, "tag");
                    add_tag_window.Show();
                    break;
                case locBtn_name:
                    Add_tag add_loc_window = new Add_tag(this, db, "loc");
                    add_loc_window.Show();
                    break;
                case emplBtn_name:
                    Add_employee add_empl_window = new Add_employee(this, db);
                    add_empl_window.Show();
                    break;

            }
        }

        private void Update_count(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int count = Convert.ToInt32(count_things.Text);
            switch (button.Name)
            {
                case "plus":
                    count++;
                    count_things.Text = Convert.ToString(count);
                    break;
                case "minus":
                    if (count != 0)
                    {
                        count--;
                        count_things.Text = Convert.ToString(count);
                    }
                    break;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            window1.frame.Navigate(main_Page);
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            List<Location> locations = db.Locations.ToList();
            List<Employee> employees = db.Employees.ToList();
            switch (Add_btn.Content)
            {
                case addBtn:
                    if (name.Text != "" && ComboBox_location.Text != "" && number.Text != "" && count_things.Text != "0" && select_tags.Children.Count > 0)
                    {
                        
                        foreach (Location loc in locations)
                        {
                            if (loc.name == ComboBox_location.Text)
                            {
                                Thing thing = new Thing(name.Text, number.Text, Convert.ToInt32(count_things.Text));
                                if (ComboBox_employee.Text != "" && date.Text != "")
                                {
                                    string[] fio = ComboBox_employee.Text.Split(' ');
                                    foreach (Employee item in employees)
                                    {
                                        if (item.surname == fio[0] && item.name == fio[1] && item.patronymic == fio[2])
                                        {
                                            item.Things.Add(thing);
                                            thing.date = date.Text;
                                            break;
                                        }
                                    }

                                }
                                db.Things.Add(thing);
                                db.SaveChanges();
                                break;
                            }
                        }

                        List<Thing> things = db.Things.ToList();
                        foreach (Grid item in select_tags.Children)
                        {
                            var borders = item.Children;
                            Border border = (Border)borders[0];
                            string tag_selected = (string)((Label)border.Child).Content;
                            foreach (Tag tag in db.Tags.ToList())
                            {
                                if (tag.name == tag_selected)
                                {
                                    things.Last().Tags.Add(tag);
                                    db.SaveChanges();
                                    break;
                                }
                            }

                        }
                    }
                    main_Page.Update_list(true,null);
                    break;

                case editBtn:
                    editing_thing.name = name.Text;
                    editing_thing.number = number.Text;
                    editing_thing.count = Convert.ToInt32(count_things.Text);
                    foreach (Location loc in locations)
                    {
                        if (loc.name == ComboBox_location.Text)
                        {
                            loc.Things.Add(editing_thing);
                            if (ComboBox_employee.Text != "" && date.Text != "")
                            {
                                
                                string[] fio = ComboBox_employee.Text.Split(' ');
                                foreach (Employee item in employees)
                                {
                                    if (item.surname == fio[0] && item.name == fio[1] && item.patronymic == fio[2])
                                    {
                                        item.Things.Add(editing_thing);
                                        db.Employees.Update(item);
                                        editing_thing.date = date.Text;
                                        break;
                                    }
                                }

                            }
                            else
                            {
                                foreach (Employee emp in employees)
                                {
                                    foreach (Thing thing in emp.Things)
                                    {
                                        if(thing.id == editing_thing.id)
                                        {
                                            emp.Things.Remove(editing_thing);
                                            break;
                                        }    
                                    }
                                }
                                editing_thing.date = null;
                            }
                        }
                    }
                    foreach (Grid item in select_tags.Children)
                    {
                        var borders = item.Children;
                        Border border = (Border)borders[0];
                        string tag_selected = (string)((Label)border.Child).Content;
                        foreach (Tag tag in db.Tags.ToList())
                        {
                            if (tag.name == tag_selected)
                            {
                                bool present = false;
                                foreach (Tag t in editing_thing.Tags)
                                {
                                    if(tag.id == t.id)
                                    {
                                        present = true;
                                        break;
                                    }         
                                }
                                if(!present)
                                    editing_thing.Tags.Add(tag);
                                break;
                            }
                        }

                    }
                    db.SaveChanges();
                    main_Page.Update_thing(editing_grid,editing_thing);
                    break;
            }
        }
    }
}
