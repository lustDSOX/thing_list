using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace thing_list
{
    public partial class Add_page : Page
    {
        Application_Context db;
        public MainWindow window1;
        Main_page main_Page;
        public int tag_col = 0;
        public Data_thing editing_item;
        public Thing editing_thing;
        const string tagBtn_name = "add_tagBtn";
        const string locBtn_name = "add_locBtn";
        const string emplBtn_name = "add_emplBtn";
        const string addBtn = "Добавить запись";
        const string editBtn = "Изменить запись";
        public ObservableCollection<Thing_employees> thing_s = new ObservableCollection<Thing_employees>();
        public Add_page(MainWindow _window1, Application_Context context, Main_page main)
        {
            InitializeComponent();
            db = context; ;
            window1 = _window1;
            main_Page = main;
            list.ItemsSource = thing_s;
        }

        public class Thing_employees
        {
            public Thing_employees()
            {
                foreach (Employee item in db.Employees)
                {
                    Name.Add($"{item.surname} {item.name} {item.patronymic}");
                }
            }
            public string Count { get; set; }
            public string SelectedName { get; set; }
            public List<string> Name { get; set; } = new();
            Application_Context db = new Application_Context();

            public DateTime? Date { get; set; }
            public string? Comm { get; set; }
        }

        public void Clear()
        {
            name.Clear();
            number.Clear();
            ComboBox_location.Text = "";
            count_things.Text = "0";
            select_tags.Children.RemoveRange(0, select_tags.Children.Count);
            Add_btn.Content = addBtn;
            ComboBox_tags.Text = "";
            tag_col = 0;
            thing_s.Clear();
            list.Items.Refresh();

        }
        public void Edit_thing(Thing thing, Data_thing item_list)
        {
            editing_thing = thing;
            editing_item = item_list;
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
            foreach (Taken_things item in thing.Taken_Things)
            {
                Employee employee = db.Employees.Where(e => e.id == item.id_employee).FirstOrDefault();
                Thing_employees thing_Employees = new Thing_employees();
                thing_Employees.SelectedName = $"{employee.surname} {employee.name} {employee.patronymic}";
                thing_Employees.Count = item.count;
                thing_Employees.Date = item.date;
                thing_Employees.Comm = item.comm;
                thing_s.Add(thing_Employees);
            }


            Add_btn.Content = editBtn;
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

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tag_col < 6)
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
                columnDefinition0.Width = new GridLength(3, GridUnitType.Star);
                grid.ColumnDefinitions.Add(columnDefinition0);
                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.Name = $"grid_{tag_col}";

                Label label = new Label();
                label.Foreground = Brushes.White;
                label.Content = name_tag;
                label.Background = Brushes.Transparent;
                label.FontSize = 10;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.Padding = new Thickness(0, 0, 5, 0);

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

                tag_col += 2;

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

            foreach (Grid item in select_tags.Children)
            {

                Grid.SetColumn(item, tag_col);
                tag_col += 2;
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
            var parent3 = button.Parent;
            var parent2 = ((Grid)parent3).Parent;
            var parent1 = ((Border)parent2).Parent;
            var box = ((Grid)parent1).Children.OfType<TextBox>().FirstOrDefault();
            int count = 0;
            try
            {
                count = Convert.ToInt32(((TextBox)box).Text);
            }
            catch { }

            switch (button.Name)
            {
                case "plus":
                    count++;
                    ((TextBox)box).Text = Convert.ToString(count);
                    break;
                case "minus":
                    if (count != 0)
                    {
                        count--;
                        ((TextBox)box).Text = Convert.ToString(count);
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
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Duration = TimeSpan.FromSeconds(2);
            switch (Add_btn.Content)
            {
                case addBtn:
                    if (name.Text != "" && ComboBox_location.Text != "" && number.Text != "" && count_things.Text != "0" && select_tags.Children.Count > 0)
                    {
                        Thing thing = new Thing();

                        thing.count = Convert.ToInt32(count_things.Text);
                        thing.name = name.Text;
                        thing.number = number.Text;

                        foreach (Grid item in select_tags.Children)
                        {
                            var borders = item.Children;
                            Border border = (Border)borders[0];
                            string tag_selected = (string)((Label)border.Child).Content;
                            thing.Tags.Add(db.Tags.Where(t => t.name == tag_selected).FirstOrDefault());
                        }

                        try
                        {
                            foreach (Thing_employees item in thing_s)
                            {
                                if (item.Count != null && item.Date != null)
                                {
                                    string[] fio = item.SelectedName.Split(' ');
                                    Employee employee = db.Employees.Where(e => e.surname == fio[0]).Where(e => e.name == fio[1]).Where(e => e.patronymic == fio[2]).FirstOrDefault();
                                    Taken_things taken_Things = new Taken_things();
                                    taken_Things.employee = employee;
                                    taken_Things.thing = editing_thing;
                                    taken_Things.count = item.Count;
                                    taken_Things.date = item.Date;
                                    taken_Things.comm = item.Comm;
                                    thing.Taken_Things.Add(taken_Things);

                                }

                            }
                        }
                        catch { }

                        Location location = db.Locations.Where(l => l.name == ComboBox_location.Text).FirstOrDefault();
                        location.Things.Add(thing);
                        db.Things.Add(thing);
                        db.SaveChanges();
                        main_Page.Update_list(true);
                    }
                    save_img.Opacity = 1;
                    save_img.BeginAnimation(Image.OpacityProperty, animation);
                    break;

                case editBtn:
                    editing_thing.name = name.Text;
                    editing_thing.number = number.Text;
                    editing_thing.count = Convert.ToInt32(count_things.Text);
                    editing_thing.Tags.Clear();
                    foreach (Grid item in select_tags.Children)
                    {
                        var borders = item.Children;
                        Border border = (Border)borders[0];
                        string tag_selected = (string)((Label)border.Child).Content;
                        editing_thing.Tags.Add(db.Tags.Where(t => t.name == tag_selected).FirstOrDefault());
                    }

                    editing_thing.Taken_Things.Clear();

                    foreach (Thing_employees item in thing_s)
                    {
                        if (item.Count != null && item.Date != null)
                        {
                            string[] fio = item.SelectedName.Split(' ');
                            Employee employee = db.Employees.Where(e => e.surname == fio[0]).Where(e => e.name == fio[1]).Where(e => e.patronymic == fio[2]).FirstOrDefault();
                            Taken_things taken_Things = new Taken_things();
                            taken_Things.employee = employee;
                            taken_Things.thing = editing_thing;
                            taken_Things.count = item.Count;
                            taken_Things.date = item.Date;
                            taken_Things.comm = item.Comm;
                            editing_thing.Taken_Things.Add(taken_Things);
                        }

                    }


                    foreach (Location loc in db.Locations)
                    {
                        if (loc.Things.Where(t => t.id == editing_thing.id).Count() != 0)
                        {
                            loc.Things.Remove(editing_thing);
                            break;
                        }
                    }

                    Location location_add = db.Locations.Where(l => l.name == ComboBox_location.Text).FirstOrDefault();
                    location_add.Things.Add(editing_thing);
                    db.SaveChanges();

                    editing_item.Name = editing_thing.name;
                    editing_item.Number = editing_thing.number;
                    editing_item.Location = ComboBox_location.Text;
                    editing_item.Count = editing_thing.count;
                    string tags = "";
                    int count = 1;
                    foreach (Tag tag in editing_thing.Tags)
                    {
                        if (count == editing_thing.Tags.Count)
                            tags += tag.name;
                        else
                            tags += tag.name + "; ";
                    }
                    editing_item.Tag = tags;
                    main_Page.list.Items.Refresh();
                    save_img.Opacity = 1;
                    save_img.BeginAnimation(Image.OpacityProperty, animation);
                    break;
            }
        }

        private void Del_thing(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Действительно удалить данную деталь?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                db.Things.Remove(editing_thing);
                db.SaveChanges();
                main_Page.things.Clear();
                main_Page.Update_list(false);
                window1.frame.Navigate(main_Page);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("columnW_add.json");
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
                StreamWriter sw = new StreamWriter("columnW_add.json");
                sw.WriteLine(JsonConvert.SerializeObject(gridLayout));
                sw.Close(); ;
            }
            catch { }
        }
        private void Del_tEmployee(object sender, RoutedEventArgs e)
        {
            var item = list.SelectedItem;
            try
            {
                Thing_employees employee = (Thing_employees)item;
                MessageBoxResult result = MessageBox.Show("Действительно удалить данного сотрудника?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    thing_s.Remove(employee);
                }
            }
            catch
            {
                MessageBox.Show("Значение пусто");
            }
            
        }

        private void del_location_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_location.Text == "")
            {
                MessageBox.Show("Значение пусто");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Действительно удалить данное расположение?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Location location = db.Locations.Where(l => l.name == ComboBox_location.Text).FirstOrDefault();
                    db.Locations.Remove(location);
                    db.SaveChanges();
                    ComboBox_location.Items.Clear();
                    Update_ListLocations(false);

                }
            }
           
        }
    }
}
