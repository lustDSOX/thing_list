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
    /// <summary>
    /// Логика взаимодействия для Add_employee.xaml
    /// </summary>
    public partial class Add_employee : Window
    {
        string def_surname = "Фамилия";
        string def_name = "Имя";
        string def_patronymic = "Отчество";
        Application_Context db;
        public Add_page add_Page;
        public Add_employee(Add_page _add_Page, Application_Context context)
        {
            InitializeComponent();
            db = context;
            add_Page = _add_Page;
        }

        private void Got_focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Name == surname.Name && surname.Text == def_surname)
                surname.Text = "";
            if (box.Name == name.Name && name.Text == def_name)
                name.Text = "";
            if (box.Name == patronymic.Name && patronymic.Text == def_patronymic)
                patronymic.Text = "";
        }

        private void Lost_focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Name == surname.Name && surname.Text == "")
                surname.Text = def_surname;
            if (box.Name == name.Name && name.Text == "")
                name.Text = def_name;
            if (box.Name == patronymic.Name && patronymic.Text == "")
                patronymic.Text = def_patronymic;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (surname.Text != def_surname && name.Text != def_name && patronymic.Text != def_patronymic)
            {
                Employee employee = new Employee(surname.Text, name.Text, patronymic.Text);
                db.Employees.Add(employee);
                db.SaveChanges();
                add_Page.Update_ListEmployees(true);
                Close();
            }
        }
    }
}
