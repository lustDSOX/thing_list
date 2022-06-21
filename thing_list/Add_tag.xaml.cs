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
using System.Windows.Shapes;

namespace thing_list
{
    public partial class Add_tag : Window
    {
        Application_Context db;
        public Add_page add_Page;
        public Add_tag(Add_page _add_Page, Application_Context context, string type)
        {
            add_Page = _add_Page;
            InitializeComponent();
            db = context;
            switch (type)
            {
                case "tag":
                    btn.Content = "Добавить категорию";
                    break;
                case "loc":
                    btn.Content = "Добавить расположение";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(((Button)sender).Content) == "Добавить категорию")
            {
                var t = db.Tags.Where(p => p.name == name.Text);
                if (t.Count() == 0)
                {
                    Tag tag = new Tag(name.Text);
                    db.Tags.Add(tag);
                    db.SaveChanges();
                    add_Page.Update_ListTags(true);
                    Close();
                }
                    
            }
            else
            {
                Location location = new Location(name.Text);
                db.Locations.Add(location);
                db.SaveChanges();
                add_Page.Update_ListLocations(true);
                Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
