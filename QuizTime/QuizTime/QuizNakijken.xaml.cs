using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for QuizNakijken.xaml
    /// </summary>
    public partial class QuizNakijken : Window
    {
        private database db = new database();
        private int choosenQuiz = 0;
        private List<string> listID = new List<string>();
        private List<string> listTitel = new List<string>();
        public QuizNakijken()
        {
            InitializeComponent();
            btnNakijken1.Click += btnNakijken_Click;
            btnNakijken2.Click += btnNakijken_Click;
            btnNakijken3.Click += btnNakijken_Click;
            btnNakijken4.Click += btnNakijken_Click;
            btnNakijken5.Click += btnNakijken_Click;
            btnNakijken6.Click += btnNakijken_Click;

            MaximizeToSecondaryMonitor();

            MySqlDataReader quizID = db.SelectQuizID();
            while (quizID.Read())
            {
                listID.Add(quizID[0].ToString());

            }

            MySqlDataReader quizTitel = db.SelectQuizTitel();
            while (quizTitel.Read())
            {
                listTitel.Add(quizTitel[0].ToString());

            }

            switch (listID.Count)
            {
                case 1:
                    btnNakijken1.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    break;
                case 2:
                    btnNakijken1.Content = "Quiz nakijken";
                    btnNakijken2.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    break;
                case 3:
                    btnNakijken1.Content = "Quiz nakijken";
                    btnNakijken2.Content = "Quiz nakijken";
                    btnNakijken3.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    break;
                case 4:
                    btnNakijken1.Content = "Quiz nakijken";
                    btnNakijken2.Content = "Quiz nakijken";
                    btnNakijken3.Content = "Quiz nakijken";
                    btnNakijken4.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    break;
                case 5:
                    btnNakijken1.Content = "Quiz nakijken";
                    btnNakijken2.Content = "Quiz nakijken";
                    btnNakijken3.Content = "Quiz nakijken";
                    btnNakijken4.Content = "Quiz nakijken";
                    btnNakijken5.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    txbQuizTitel5.Text = listTitel[4];
                    break;
                case 6:
                    btnNakijken1.Content = "Quiz nakijken";
                    btnNakijken2.Content = "Quiz nakijken";
                    btnNakijken3.Content = "Quiz nakijken";
                    btnNakijken4.Content = "Quiz nakijken";
                    btnNakijken5.Content = "Quiz nakijken";
                    btnNakijken6.Content = "Quiz nakijken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    txbQuizTitel5.Text = listTitel[4];
                    txbQuizTitel6.Text = listTitel[5];
                    break;

            }
        }

        private void btnNakijken_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button clickedButton = sender as System.Windows.Controls.Button;

            if (clickedButton == null)
                return;

            if (clickedButton.Name == "btnNakijken1" && txbQuizTitel1.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[0]);
            }
            else if (clickedButton.Name == "btnNakijken2" && txbQuizTitel2.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[1]); ;
            }
            else if (clickedButton.Name == "btnNakijken3" && txbQuizTitel3.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[2]); ;
            }
            else if (clickedButton.Name == "btnNakijken4" && txbQuizTitel4.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[3]); ;
            }
            else if (clickedButton.Name == "btnNakijken5" && txbQuizTitel5.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[4]); ;
            }
            else if (clickedButton.Name == "btnNakijken6" && txbQuizTitel6.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[5]); ;
            }

            if (choosenQuiz > 0)
            {
                QuizSpelenAdmin QuizSpelenAdmin = new QuizSpelenAdmin(choosenQuiz, "Nakijken");
                QuizSpelenAdmin.Show();
                this.Close();
            }
            else
            {
                QuizAanmaken QuizAanmaken = new QuizAanmaken();
                QuizAanmaken.Show();
                this.Close();
            }
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
