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
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Windows.Threading;
using System.Media;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for QuizSpelenAdmin.xaml
    /// </summary>
    public partial class QuizSpelenAdmin : Window
    {
        private quizState quizState;
        private QuizSpelenUser QuizSpelenUser;
        private database db;
        private List<List<SaveQuestions>> listQuestions = new List<List<SaveQuestions>>();
        private List<string> listTitel = new List<string>();
        public string quizID;
        private int _listNumber = 0;
        public int questionNumber = 1;
        private DispatcherTimer timer = new DispatcherTimer();
        public int getalTimer;
        public int timerSound;
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private SoundPlayer _TimerTick;
        private SoundPlayer _StartEndSound;
        public QuizSpelenAdmin(int choosenQuiz, string choosenMode)
        {
            InitializeComponent();
            btnContinue.Click += BtnContinue_Click;
            quizID = choosenQuiz.ToString();
            quizState = new quizState();
            QuizSpelenUser = new QuizSpelenUser();
            db = new database();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
           
            MaximizeToSecondaryMonitor();

            if (choosenMode == "Spelen")
            {
                quizState.gamestate = quizState.GAMESTATE.GAME_START;
            } else if (choosenMode == "Nakijken")
            {
                quizState.gamestate = quizState.GAMESTATE.CHECK_START;
            }
           
           

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

            for (int i = 0; i < listQuestions.Count; i++)
            {
                if (listQuestions[i][0].image == "")
                {
                    listQuestions[i][0].image = null;
                }
            }

            if (quizState.gamestate == quizState.GAMESTATE.GAME_START)
            {
                QuizSpelenUser.UpdateTimer(listQuestions[0][0].timer);
                QuizSpelenUser.StartGame(listTitel[0], listQuestions.Count.ToString());
                baseDir = baseDir.Replace(@"bin\Debug\Sounds", "");
                _StartEndSound = new SoundPlayer(baseDir + @"Sounds/Start-End.wav");
                _StartEndSound.Play();
                lblVolgendeVraag.Content = listQuestions[_listNumber][0].question;
                lblVolgendeA1.Content = listQuestions[_listNumber][0].answer1;
                lblVolgendeA2.Content = listQuestions[_listNumber][0].answer2;
                lblVolgendeA3.Content = listQuestions[_listNumber][0].answer3;
                lblVolgendeA4.Content = listQuestions[_listNumber][0].answer4;
                lblVolgendeTimer.Content = listQuestions[_listNumber][0].timer.ToString();

                switch (listQuestions[_listNumber][0].rightAnswer)
                {
                    case 1:
                        lblVolgendeRA.Content = listQuestions[_listNumber][0].answer1;
                        break;
                    case 2:
                        lblVolgendeRA.Content = listQuestions[_listNumber][0].answer2;
                        break;
                    case 3:
                        lblVolgendeRA.Content = listQuestions[_listNumber][0].answer3;
                        break;
                    case 4:
                        lblVolgendeRA.Content = listQuestions[_listNumber][0].answer4;
                        break;
                }
            } else if (quizState.gamestate == quizState.GAMESTATE.CHECK_START)
            {
                QuizSpelenUser.StartCheck(listTitel[0]);
                _listNumber = 0;
                questionNumber = 1;
                quizState.gamestate = quizState.GAMESTATE.SHOW_CHECK_QUESTIONS;
                if (listQuestions.Count > _listNumber)
                {
                    lblVolgendeVraag.Content = listQuestions[_listNumber + 1][0].question;
                    lblVolgendeA1.Content = listQuestions[_listNumber + 1][0].answer1;
                    lblVolgendeA2.Content = listQuestions[_listNumber + 1][0].answer2;
                    lblVolgendeA3.Content = listQuestions[_listNumber + 1][0].answer3;
                    lblVolgendeA4.Content = listQuestions[_listNumber + 1][0].answer4;
                    lblVolgendeTimer.Content = listQuestions[_listNumber + 1][0].timer.ToString();

                    switch (listQuestions[_listNumber + 1][0].rightAnswer)
                    {
                        case 1:
                            lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer1;
                            break;
                        case 2:
                            lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer2;
                            break;
                        case 3:
                            lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer3;
                            break;
                        case 4:
                            lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer4;
                            break;
                    }
                }
                else
                {
                    lblVolgendeVraag.Content = "Geen volgende vraag meer";
                    lblVolgendeA1.Content = "Geen volgende vraag meer";
                    lblVolgendeA2.Content = "Geen volgende vraag meer";
                    lblVolgendeA3.Content = "Geen volgende vraag meer";
                    lblVolgendeA4.Content = "Geen volgende vraag meer";
                    lblVolgendeRA.Content = "Geen volgende vraag meer";
                    lblVolgendeTimer.Content = "Geen volgende vraag meer";
                }
            }
            QuizSpelenUser.Show();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            getalTimer--;
            if (getalTimer == timerSound)
            {
                baseDir = baseDir.Replace(@"bin\Debug\Sounds", "");
                _TimerTick = new SoundPlayer(baseDir + @"Sounds/timer-tick.wav");
                _TimerTick.Play();
            }
                
            if (getalTimer == 0)
            {
                timer.Stop();
                _TimerTick.Stop();
                if (questionNumber > listQuestions.Count)
                {
                    QuizSpelenUser.StartCheck(listTitel[0]);
                    _listNumber = 0;
                    questionNumber = 1;
                    quizState.gamestate = quizState.GAMESTATE.SHOW_CHECK_QUESTIONS;
                    baseDir = baseDir.Replace(@"bin\Debug\Sounds", "");
                    _TimerTick = new SoundPlayer(baseDir + @"Sounds/Start-End.wav");
                    _TimerTick.Play();
                } else
                {
                    QuizSpelenUser.NextDivider(questionNumber.ToString());
                    quizState.gamestate = quizState.GAMESTATE.SHOW_NEXT_QUESTION; // next state
                }
              
            }
            QuizSpelenUser.UpdateTimer(getalTimer);
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {   
            if (getalTimer == 0)
            {
                switch (quizState.gamestate)
                {
                    case quizState.GAMESTATE.GAME_START:
                        quizState.gamestate = quizState.GAMESTATE.SHOW_NEXT_QUESTION;
                        getalTimer = listQuestions[_listNumber][0].timer;
                        timerSound = 5;
                        QuizSpelenUser.NextQuestion(listQuestions[_listNumber][0].answer1, listQuestions[_listNumber][0].answer2, listQuestions[_listNumber][0].answer3, listQuestions[_listNumber][0].answer4, listQuestions[_listNumber][0].question, getalTimer, listQuestions[_listNumber][0].image);
                        timer.Start();
                        _listNumber++;
                        questionNumber++;
                        lblVolgendeVraag.Content = listQuestions[_listNumber][0].question;
                        lblVolgendeA1.Content = listQuestions[_listNumber][0].answer1;
                        lblVolgendeA2.Content = listQuestions[_listNumber][0].answer2;
                        lblVolgendeA3.Content = listQuestions[_listNumber][0].answer3;
                        lblVolgendeA4.Content = listQuestions[_listNumber][0].answer4;
                        lblVolgendeTimer.Content = listQuestions[_listNumber][0].timer.ToString();

                        switch (listQuestions[_listNumber][0].rightAnswer)
                        {
                            case 1:
                                lblVolgendeRA.Content = listQuestions[_listNumber][0].answer1;
                                break;
                            case 2:
                                lblVolgendeRA.Content = listQuestions[_listNumber][0].answer2;
                                break;
                            case 3:
                                lblVolgendeRA.Content = listQuestions[_listNumber][0].answer3;
                                break;
                            case 4:
                                lblVolgendeRA.Content = listQuestions[_listNumber][0].answer4;
                                break;
                        }
                        break;
                    case quizState.GAMESTATE.SHOW_DIVIDER:
                        QuizSpelenUser.NextDivider(questionNumber.ToString());
                        quizState.gamestate = quizState.GAMESTATE.SHOW_NEXT_QUESTION;
                            break;
                    case quizState.GAMESTATE.SHOW_NEXT_QUESTION:
                        getalTimer = listQuestions[_listNumber][0].timer;
                        QuizSpelenUser.UpdateTimer(getalTimer);
                        QuizSpelenUser.NextQuestion(listQuestions[_listNumber][0].answer1, listQuestions[_listNumber][0].answer2, listQuestions[_listNumber][0].answer3, listQuestions[_listNumber][0].answer4, listQuestions[_listNumber][0].question, getalTimer, listQuestions[_listNumber][0].image);
                        timer.Start();
                        _listNumber++;
                        questionNumber++;
                        if (listQuestions.Count > _listNumber)
                        {
                            lblVolgendeVraag.Content = listQuestions[_listNumber][0].question;
                            lblVolgendeA1.Content = listQuestions[_listNumber][0].answer1;
                            lblVolgendeA2.Content = listQuestions[_listNumber][0].answer2;
                            lblVolgendeA3.Content = listQuestions[_listNumber][0].answer3;
                            lblVolgendeA4.Content = listQuestions[_listNumber][0].answer4;
                            lblVolgendeTimer.Content = listQuestions[_listNumber][0].timer.ToString();

                            switch (listQuestions[_listNumber][0].rightAnswer)
                            {
                                case 1:
                                    lblVolgendeRA.Content = listQuestions[_listNumber][0].answer1;
                                    break;
                                case 2:
                                    lblVolgendeRA.Content = listQuestions[_listNumber][0].answer2;
                                    break;
                                case 3:
                                    lblVolgendeRA.Content = listQuestions[_listNumber][0].answer3;
                                    break;
                                case 4:
                                    lblVolgendeRA.Content = listQuestions[_listNumber][0].answer4;
                                    break;
                            }
                        } else
                        {
                            lblVolgendeVraag.Content = "Geen volgende vraag meer";
                            lblVolgendeA1.Content = "Geen volgende vraag meer";
                            lblVolgendeA2.Content = "Geen volgende vraag meer";
                            lblVolgendeA3.Content = "Geen volgende vraag meer";
                            lblVolgendeA4.Content = "Geen volgende vraag meer";
                            lblVolgendeRA.Content = "Geen volgende vraag meer";
                            lblVolgendeTimer.Content = "Geen volgende vraag meer";
                        }
                        break;
                    case quizState.GAMESTATE.SHOW_CHECK_QUESTIONS:
                        if (questionNumber > listQuestions.Count)
                        {
                            QuizSpelenUser.gridTussenSchermEnd.Visibility = Visibility.Visible;
                            quizState.gamestate = quizState.GAMESTATE.QUIZ_END;
                        }
                        else
                        {
                            QuizSpelenUser.NextQuestionAnswer(listQuestions[_listNumber][0].answer1, listQuestions[_listNumber][0].answer2, listQuestions[_listNumber][0].answer3, listQuestions[_listNumber][0].answer4, listQuestions[_listNumber][0].question, getalTimer, listQuestions[_listNumber][0].image);
                            quizState.gamestate = quizState.GAMESTATE.SHOW_CHECK_RIGHTANSWERS;
                            if (listQuestions.Count > _listNumber + 1)
                            {
                                lblVolgendeVraag.Content = listQuestions[_listNumber + 1][0].question;
                                lblVolgendeA1.Content = listQuestions[_listNumber + 1][0].answer1;
                                lblVolgendeA2.Content = listQuestions[_listNumber + 1][0].answer2;
                                lblVolgendeA3.Content = listQuestions[_listNumber + 1][0].answer3;
                                lblVolgendeA4.Content = listQuestions[_listNumber + 1][0].answer4;
                                lblVolgendeTimer.Content = listQuestions[_listNumber + 1][0].timer.ToString();

                                switch (listQuestions[_listNumber + 1][0].rightAnswer)
                                {
                                    case 1:
                                        lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer1;
                                        break;
                                    case 2:
                                        lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer2;
                                        break;
                                    case 3:
                                        lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer3;
                                        break;
                                    case 4:
                                        lblVolgendeRA.Content = listQuestions[_listNumber + 1][0].answer4;
                                        break;
                                }
                            }
                            else
                            {
                                lblVolgendeVraag.Content = "Geen volgende vraag meer";
                                lblVolgendeA1.Content = "Geen volgende vraag meer";
                                lblVolgendeA2.Content = "Geen volgende vraag meer";
                                lblVolgendeA3.Content = "Geen volgende vraag meer";
                                lblVolgendeA4.Content = "Geen volgende vraag meer";
                                lblVolgendeRA.Content = "Geen volgende vraag meer";
                                lblVolgendeTimer.Content = "Geen volgende vraag meer";
                            }
                        }
                        break;
                    case quizState.GAMESTATE.SHOW_CHECK_RIGHTANSWERS:
                        QuizSpelenUser.ShowAnswer(listQuestions[_listNumber][0].rightAnswer);
                        quizState.gamestate = quizState.GAMESTATE.SHOW_CHECK_QUESTIONS;
                        _listNumber++;
                        questionNumber++;
                        break;
                    case quizState.GAMESTATE.QUIZ_END:
                        this.Close();
                        QuizSpelenUser.Close();
                        break;
                }
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
