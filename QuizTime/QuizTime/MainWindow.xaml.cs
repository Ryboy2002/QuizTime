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
            /* btnGegevens.Click += BtnGegevens_Click;*/
            btnQuizBekijken.Click += BtnQuizBekijken_Click;
            btnQuizNakijken.Click += BtnQuizNakijken_Click;
            btnQuizSpelen.Click += BtnQuizSpelen_Click;
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

        /*private void BtnGegevens_Click(object sender, RoutedEventArgs e)
        {
            MySqlDataReader gegevens = database.SelectQuiz();
            while (gegevens.Read())
            {
                MessageBox.Show(gegevens[0].ToString());
                txblockGegevens.Text = gegevens[0].ToString();
            }
        }*/


    }
}
