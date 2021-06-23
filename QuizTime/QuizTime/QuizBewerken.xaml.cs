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
    /// Interaction logic for QuizBewerken.xaml
    /// </summary>
    public partial class QuizBewerken : Window
    {
        private database db = new database();
        private List<List<SaveQuestions>> listQuestions = new List<List<SaveQuestions>>();
        private List<string> listTitel = new List<string>();
        private string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private int _questionNumber = 1;
        private int _listNumber;
        private int _rightAnswer;
        private int _numberOfQuestions;
        public QuizBewerken(int choosenQuiz)
        {
            InitializeComponent();

            btnQuit.Click += BtnQuit_Click;
            btnVolgende.Click += BtnVolgende_Click;
            btnVorige.Click += BtnVorige_Click;
            btnSave.Click += BtnSave_Click;

            PointCollection polygonColl = new PointCollection();
            polygonColl.Add(new Point(0, 80));
            polygonColl.Add(new Point(40, 10));
            polygonColl.Add(new Point(80, 80));
            polygonTriangle.Points = polygonColl;

            string quizID = choosenQuiz.ToString();

            MySqlDataReader quizVragen = db.SelectQuizVragen($"{quizID}");

            MySqlDataReader quizTitel = db.SelectedQuizTitel($"{quizID}");
            while (quizTitel.Read())
            {
                listTitel.Add(quizTitel[0].ToString());

            }

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
                            rightAnswer = Convert.ToInt32(ob["rightAnswer"]),
                            image = ob["image"].ToString(),
                            timer = Convert.ToInt32(ob["timer"])
                        },
                    };
                        listQuestions.Add(question);
                    }
                }
            }
            txbQuizTitel.Text = listTitel[0];
            txbAnswer1.Text = listQuestions[0][0].answer1;
            txbAnswer2.Text = listQuestions[0][0].answer2;
            txbAnswer3.Text = listQuestions[0][0].answer3;
            txbAnswer4.Text = listQuestions[0][0].answer4;
            txbQuizVraag.Text = listQuestions[0][0].question;
            txbTimer.Text = listQuestions[0][0].timer.ToString();
            lblQuestionNumber.Content = $"Vraag 1";
            MessageBox.Show(listQuestions[0][0].rightAnswer.ToString());
            if (listQuestions[0][0].rightAnswer == 1)
            {
                cboxCorrectAnswer1.IsChecked = true;
            }
            else if (listQuestions[0][0].rightAnswer == 2)
            {
                cboxCorrectAnswer2.IsChecked = true;
            }
            else if (listQuestions[0][0].rightAnswer == 3)
            {
                cboxCorrectAnswer3.IsChecked = true;
            }
            else if (listQuestions[0][0].rightAnswer == 4)
            {
                cboxCorrectAnswer4.IsChecked = true;
            }

            _numberOfQuestions = listQuestions.Count();
        }
        private void BtnVolgende_Click(object sender, RoutedEventArgs e)
        {
            if (txbAnswer1.Text == "" || txbAnswer2.Text == "")
            {
                MessageBox.Show("Error: Vul minimaal 2 antwoorden in");
            }
            else if ((bool)cboxCorrectAnswer1.IsChecked == false && (bool)cboxCorrectAnswer2.IsChecked == false && (bool)cboxCorrectAnswer3.IsChecked == false && (bool)cboxCorrectAnswer4.IsChecked == false)
            {
                MessageBox.Show("Error: Selecteer een juist antwoord");
            }
            else if (Convert.ToInt32(txbTimer.Text) > 120)
            {
                MessageBox.Show("Error: Maximale tijd is 120 secondes");
            }
            else if (txbQuizVraag.Text == "")
            {
                MessageBox.Show("Error: Vul de vraag in");
            }
            else
            {
                _questionNumber++;
            }

            if (_numberOfQuestions < _questionNumber - 1)
            {
                if (txbAnswer1.Text == "" || txbAnswer2.Text == "")
                {
                    MessageBox.Show("Error: Vul minimaal 2 antwoorden in");
                }
                else if ((bool)cboxCorrectAnswer1.IsChecked == false && (bool)cboxCorrectAnswer2.IsChecked == false && (bool)cboxCorrectAnswer3.IsChecked == false && (bool)cboxCorrectAnswer4.IsChecked == false)
                {
                    MessageBox.Show("Error: Selecteer een juist antwoord");
                }
                else if (Convert.ToInt32(txbTimer.Text) > 120)
                {
                    MessageBox.Show("Error: Maximale tijd is 120 secondes");
                } else if (txbQuizVraag.Text == ""){
                    MessageBox.Show("Error: Vul de vraag in");
                }
                else
                {
                    List<SaveQuestions> question = new List<SaveQuestions>
            {
                new SaveQuestions
                {
                    question = txbQuizVraag.Text,
                    answer1 = txbAnswer1.Text,
                    answer2 = txbAnswer2.Text,
                    answer3 = txbAnswer3.Text,
                    answer4 = txbAnswer4.Text,
                    rightAnswer = _rightAnswer,
                    image = "",
                    timer = Convert.ToInt32(txbTimer.Text)
                },
            };
                    listQuestions.Add(question);
                    _numberOfQuestions++;
                    lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                    txbAnswer1.Text = "";
                    txbAnswer2.Text = "";
                    txbAnswer3.Text = "";
                    txbAnswer4.Text = "";
                    txbQuizVraag.Text = "";
                    txbTimer.Text = "30";
                    cboxCorrectAnswer1.IsChecked = false;
                    cboxCorrectAnswer2.IsChecked = false;
                    cboxCorrectAnswer3.IsChecked = false;
                    cboxCorrectAnswer4.IsChecked = false;
                }
            }
            else if (_questionNumber > _numberOfQuestions)
            {
                if (_listNumber >= 1)
                {
                    listQuestions[_listNumber][0].answer1 = txbAnswer1.Text;
                    listQuestions[_listNumber][0].answer2 = txbAnswer2.Text;
                    listQuestions[_listNumber][0].answer3 = txbAnswer3.Text;
                    listQuestions[_listNumber][0].answer4 = txbAnswer4.Text;
                    listQuestions[_listNumber][0].question = txbQuizVraag.Text;
                    listQuestions[_listNumber][0].timer = Convert.ToInt32(txbTimer.Text);
                    if ((bool)cboxCorrectAnswer1.IsChecked)
                    {
                        listQuestions[_listNumber - 1][0].rightAnswer = 1;
                    }
                    else if ((bool)cboxCorrectAnswer2.IsChecked)
                    {
                        listQuestions[_listNumber - 1][0].rightAnswer = 2;
                    }
                    else if ((bool)cboxCorrectAnswer3.IsChecked)
                    {
                        listQuestions[_listNumber - 1][0].rightAnswer = 3;
                    }
                    else if ((bool)cboxCorrectAnswer4.IsChecked)
                    {
                        listQuestions[_listNumber - 1][0].rightAnswer = 4;
                    }
                }

                lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                txbAnswer1.Text = "";
                txbAnswer2.Text = "";
                txbAnswer3.Text = "";
                txbAnswer4.Text = "";
                txbQuizVraag.Text = "";
                txbTimer.Text = "30";
                cboxCorrectAnswer1.IsChecked = false;
                cboxCorrectAnswer2.IsChecked = false;
                cboxCorrectAnswer3.IsChecked = false;
                cboxCorrectAnswer4.IsChecked = false;
            }
            else 
            {
                if (txbAnswer1.Text == "" || txbAnswer2.Text == "")
                {
                    MessageBox.Show("Error: Vul minimaal 2 antwoorden in");
                }
                else if ((bool)cboxCorrectAnswer1.IsChecked == false && (bool)cboxCorrectAnswer2.IsChecked == false && (bool)cboxCorrectAnswer3.IsChecked == false && (bool)cboxCorrectAnswer4.IsChecked == false)
                {
                    MessageBox.Show("Error: Selecteer een juist antwoord");
                }
                else if (Convert.ToInt32(txbTimer.Text) > 120)
                {
                    MessageBox.Show("Error: Maximale tijd is 120 secondes");
                }
                else if (txbQuizVraag.Text == "")
                {
                    MessageBox.Show("Error: Vul de vraag in");
                }
                else
                {
                    _listNumber = _questionNumber - 1;
                    lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                    txbAnswer1.Text = listQuestions[_listNumber][0].answer1;
                    txbAnswer2.Text = listQuestions[_listNumber][0].answer2;
                    txbAnswer3.Text = listQuestions[_listNumber][0].answer3;
                    txbAnswer4.Text = listQuestions[_listNumber][0].answer4;
                    txbQuizVraag.Text = listQuestions[_listNumber][0].question;
                    txbTimer.Text = listQuestions[_listNumber][0].timer.ToString();
                    if (listQuestions[_listNumber][0].rightAnswer == 1)
                    {
                        cboxCorrectAnswer1.IsChecked = true;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 2)
                    {
                        cboxCorrectAnswer2.IsChecked = true;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 3)
                    {
                        cboxCorrectAnswer3.IsChecked = true;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 4)
                    {
                        cboxCorrectAnswer4.IsChecked = true;
                    }
                }
            }
        }

        private void BtnVorige_Click(object sender, RoutedEventArgs e)
        {
            if (_questionNumber != 1)
            {
                _questionNumber--;
            }
            _listNumber = _questionNumber - 1;

         /*   if (_questionNumber > _numberOfQuestions)
            {
                lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                txbAnswer1.Text = "Vul een antwoord in";
                txbAnswer2.Text = "Vul een antwoord in";
                txbAnswer3.Text = "Vul een antwoord in";
                txbAnswer4.Text = "Vul een antwoord in";
                txbQuizVraag.Text = "Vul de vraag in";
                txbTimer.Text = "30";
            } else*/
            
            lblQuestionNumber.Content = $"Vraag {_questionNumber}";
            txbAnswer1.Text = listQuestions[_listNumber][0].answer1.ToString();
            txbAnswer2.Text = listQuestions[_listNumber][0].answer2.ToString();
            txbAnswer3.Text = listQuestions[_listNumber][0].answer3.ToString();
            txbAnswer4.Text = listQuestions[_listNumber][0].answer4.ToString();
            txbQuizVraag.Text = listQuestions[_listNumber][0].question.ToString();
            txbTimer.Text = listQuestions[_listNumber][0].timer.ToString();
            if (listQuestions[_listNumber][0].rightAnswer == 1)
            {
                cboxCorrectAnswer1.IsChecked = true;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 2)
            {
                cboxCorrectAnswer2.IsChecked = true;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 3)
            {
                cboxCorrectAnswer3.IsChecked = true;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 4)
            {
                cboxCorrectAnswer4.IsChecked = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
