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
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for QuizMaken.xaml
    /// </summary>
    public partial class QuizMaken : Window
    {
        private database db = new database();
        private int choosenQuiz = 0;
        private List<string> listID = new List<string>();
        private List<string> listTitel = new List<string>();
        private List<List<SaveQuestions>> questions = new List<List<SaveQuestions>>();
        public QuizMaken()
        {
            InitializeComponent();
            btnMaken1.Click += BtnMaken_Click;
            btnMaken2.Click += BtnMaken_Click;
            btnMaken3.Click += BtnMaken_Click;
            btnMaken4.Click += BtnMaken_Click;
            btnMaken5.Click += BtnMaken_Click;
            btnMaken6.Click += BtnMaken_Click;

           /* MySqlDataReader quizVragen = db.SelectQuizVragen("1");

            while (quizVragen.Read())
            {
                string neegeenJson = quizVragen[0].ToString();
                JArray data = JArray.Parse(neegeenJson);
                foreach (JArray obj in data)
                {
                    foreach (JObject ob in obj)
                    {
                        List<SaveQuestions> question = new List<SaveQuestions>
            {
                        new SaveQuestions
                        {
                            question = ob["question"].ToString(),
                            answer1 = ob["answer1"].ToString(),
                            answer2 = ob["answer2"].ToString(),
                            answer3 = ob["answer3"].ToString(),
                            answer4 = ob["answer4"].ToString(),
                            rightAnswer = Convert.ToInt32(ob["rightanswer"]),
                            image = ob["image"].ToString(),
                            timer = Convert.ToInt32(ob["timer"])
                        },
                    };
                        questions.Add(question);
                    }
                }
            }*/

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
                    btnMaken1.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    break;
                case 2:
                    btnMaken1.Content = "Quiz bewerken";
                    btnMaken2.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    break;
                case 3:
                    btnMaken1.Content = "Quiz bewerken";
                    btnMaken2.Content = "Quiz bewerken";
                    btnMaken3.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    break;
                case 4:
                    btnMaken1.Content = "Quiz bewerken";
                    btnMaken2.Content = "Quiz bewerken";
                    btnMaken3.Content = "Quiz bewerken";
                    btnMaken4.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    break;
                case 5:
                    btnMaken1.Content = "Quiz bewerken";
                    btnMaken2.Content = "Quiz bewerken";
                    btnMaken3.Content = "Quiz bewerken";
                    btnMaken4.Content = "Quiz bewerken";
                    btnMaken5.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    txbQuizTitel5.Text = listTitel[4];
                    break;
                case 6:
                    btnMaken1.Content = "Quiz bewerken";
                    btnMaken2.Content = "Quiz bewerken";
                    btnMaken3.Content = "Quiz bewerken";
                    btnMaken4.Content = "Quiz bewerken";
                    btnMaken5.Content = "Quiz bewerken";
                    btnMaken6.Content = "Quiz bewerken";
                    txbQuizTitel1.Text = listTitel[0];
                    txbQuizTitel2.Text = listTitel[1];
                    txbQuizTitel3.Text = listTitel[2];
                    txbQuizTitel4.Text = listTitel[3];
                    txbQuizTitel5.Text = listTitel[4];
                    txbQuizTitel6.Text = listTitel[5];
                    break;

            }
    }

    private void BtnMaken_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton == null)
                return;

            if (clickedButton.Name == "btnMaken1" && txbQuizTitel1.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[0]);
            }
            else if (clickedButton.Name == "btnMaken2" && txbQuizTitel2.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[1]); ;
            }
            else if (clickedButton.Name == "btnMaken3" && txbQuizTitel3.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[2]); ;
            }
            else if (clickedButton.Name == "btnMaken4" && txbQuizTitel4.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[3]); ;
            }
            else if (clickedButton.Name == "btnMaken5" && txbQuizTitel5.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[4]); ;
            }
            else if (clickedButton.Name == "btnMaken6" && txbQuizTitel6.Text != "")
            {
                choosenQuiz = Convert.ToInt32(listID[5]); ;
            }

            if (choosenQuiz > 0)
            {
                QuizBewerken QuizBewerken = new QuizBewerken(choosenQuiz);
                QuizBewerken.Show();
                this.Close();
            }
            else
            {
                QuizAanmaken QuizAanmaken = new QuizAanmaken();
                QuizAanmaken.Show();
                this.Close();
            }
        }
    }
}
