using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        database database = new database();
        public MainWindow()
        {
            InitializeComponent();
            btnQuizBekijken.Click += BtnQuizBekijken_Click;
            btnQuizNakijken.Click += BtnQuizNakijken_Click;
            btnQuizSpelen.Click += BtnQuizSpelen_Click;
            MaximizeToSecondaryMonitor();
        }

        private void BtnQuizSpelen_Click(object sender, RoutedEventArgs e)
        {
            QuizSpelen QuizSpelen = new QuizSpelen();
            QuizSpelen.Show();
        }

        private void BtnQuizNakijken_Click(object sender, RoutedEventArgs e)
        {
            QuizNakijken QuizNakijken = new QuizNakijken();
            QuizNakijken.Show();
        }

        private void BtnQuizBekijken_Click(object sender, RoutedEventArgs e)
        {
            QuizMaken QuizMaken = new QuizMaken();
            QuizMaken.Show();
        }

        public void MaximizeToSecondaryMonitor()
        {
            var secondaryScreen = Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;

                if (this.IsLoaded)
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
        }
    }
}
