using Newtonsoft.Json;
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

namespace prak2_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<user> dates = new List<user>();
        public MainWindow()
        {
            InitializeComponent();
            dates = save.Des<List<user>>("jsonka.json") ?? new List<user>();
            data.SelectedDate = DateTime.Now;
            изменение();
        }
        void изменение()
        {
            txt1.Text = "";
            txt2.Text = "";
            if (dates.Count > 0)
            {
                lb.Items.Clear();
                foreach (var a in dates)
                {
                    if (a.date == data.SelectedDate.Value)
                    {
                        lb.Items.Add(a.title);
                    }
                }
            }
            save.Ser("jsonka.json", dates);
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            изменение();
        }
        bool есть_ли(string titles)
        {
            foreach (var a in dates)
            {
                if (titles == a.title)
                    return true;
            }
            return false;
        }
        void добавление()
        {
            dates.Add(new user(txt1.Text, txt2.Text, data.SelectedDate.Value));
            изменение();
        }
        void удаление()
        {
            foreach (var a in dates)
            {
                if (a.title == lb.SelectedItem.ToString())
                {
                    dates.Remove(a); 
                    break;
                }
            }
            изменение();
        }
        void изменить()
        {
            foreach (var a in dates)
            {
                if (a.title == lb.SelectedItem.ToString())
                {
                    a.title = txt1.Text;
                    a.summ = txt2.Text;
                }
            }
            изменение();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txt1.Text == "" || txt2.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            добавление();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lb.SelectedIndex == -1 || lb.SelectedIndex == lb.Items.Count)
            {
                MessageBox.Show("Пункт из списка не выбран!");
                return;
            }
            if (txt1.Text == "" || txt2.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            if (есть_ли(txt1.Text))
            {
                MessageBox.Show("Такая заметка уже есть!");
                return;
            }
            изменить();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (lb.SelectedIndex == -1 || lb.SelectedIndex == lb.Items.Count)
            {
                MessageBox.Show("Пункт из списка не выбран!");
                return;
            }
            удаление();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb.SelectedItem != null)
            {
                if (lb.SelectedIndex != -1 || lb.SelectedIndex != lb.Items.Count)
                {
                    foreach (var a in dates)
                    {
                        if (a.title == lb.SelectedItem.ToString())
                        {
                            txt1.Text = a.title;
                            txt2.Text = a.summ;
                        }
                    }
                }
            }
        }
    }
    class user
    {
        public string title;
        public string summ;
        public DateTime? date;

        public user(string text1, string text2, DateTime? selectedDate)
        {
            title = text1;
            summ = text2;
            date = selectedDate;
        }
    }
    class save
    {
        public static void Ser<T>(string path, T ato)
        {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(ato));
        }
        public static T Des<T>(string path)
        {
            if (!System.IO.File.Exists(path))
                System.IO.File.WriteAllText(path, "");
            return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
        }
    }
}
