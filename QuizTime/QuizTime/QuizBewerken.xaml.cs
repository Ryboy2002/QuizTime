using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private Microsoft.Win32.OpenFileDialog _image = new Microsoft.Win32.OpenFileDialog();
        private List<string> listSelectedImages = new List<string>();
        private string _imgNaam;
        private string _selectedFileName;
        private int _questionNumber = 1;
        private int _listNumber;
        private int _rightAnswer = 0;
        private int _numberOfQuestions;
        private int AddArrayNumber;
        public string quizID;
        public QuizBewerken(int choosenQuiz)
        {
            InitializeComponent();

            btnQuit.Click += BtnQuit_Click;
            btnVolgende.Click += BtnVolgende_Click;
            btnVorige.Click += BtnVorige_Click;
            btnSave.Click += BtnSave_Click;
            btnImage.Click += BtnImage_Click;
            btnVerwijderImage.Click += BtnVerwijderImage_Click;

            PointCollection polygonColl = new PointCollection();
            polygonColl.Add(new Point(0, 80));
            polygonColl.Add(new Point(40, 10));
            polygonColl.Add(new Point(80, 80));
            polygonTriangle.Points = polygonColl;

            quizID = choosenQuiz.ToString();

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
            for (int i = 0; i < listQuestions.Count; i++) // Loop through List with for
            {
                if (listQuestions[i][0].image == "")
                {
                    listQuestions[i][0].image = null;
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
            if (listQuestions[0][0].image != null)
            {
                baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                baseDir = baseDir + @"Images\" + listQuestions[0][0].image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(baseDir);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);
                baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[0][0].image), "");
            }
            else
            {
                imgQuestion.Source = null;
                gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                borderGridImage.BorderThickness = new Thickness(3);
            }

            _numberOfQuestions = listQuestions.Count();
            if (listSelectedImages.Count < _numberOfQuestions)
            {
                AddArrayNumber = _numberOfQuestions - listSelectedImages.Count;
                for (int i = 0; i < AddArrayNumber; i++) // Loop through List with for
                {
                    listSelectedImages.Add(null);
                }
            }
        }

        private void BtnVerwijderImage_Click(object sender, RoutedEventArgs e)
        {
            listQuestions[_listNumber][0].image = null;
            listSelectedImages[_listNumber] = null;
            imgQuestion.Source = null;
            gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
            borderGridImage.BorderThickness = new Thickness(3);
        }

        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            _image.Title = "Voeg een afbeelding toe";
            _image.DefaultExt = ".png";
            _image.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            Nullable<bool> result = _image.ShowDialog();
            if (result == true)
            {
                _selectedFileName = _image.FileName;
                _imgNaam = System.IO.Path.GetFileName(_image.FileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_selectedFileName);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);

                if (_questionNumber > _numberOfQuestions && listSelectedImages.Count == _numberOfQuestions)
                {
                    listSelectedImages.Add(_selectedFileName);
                } else
                {
                    listSelectedImages[_listNumber] = _selectedFileName;
                }
            }
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
                _listNumber = _questionNumber - 1;
                if (listSelectedImages.Count < _numberOfQuestions)
                {
                    AddArrayNumber = _numberOfQuestions - listSelectedImages.Count;
                    for (int i = 0; i < AddArrayNumber; i++) // Loop through List with for
                    {
                        listSelectedImages.Add(null);
                    }
                }
                /* MessageBox.Show(_questionNumber + "|" + _listNumber);*/
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
                    if ((bool)cboxCorrectAnswer1.IsChecked)
                    {
                        _rightAnswer = 1;
                    }
                    else if ((bool)cboxCorrectAnswer2.IsChecked)
                    {
                        _rightAnswer = 2;
                    }
                    else if ((bool)cboxCorrectAnswer3.IsChecked)
                    {
                        _rightAnswer = 3;
                    }
                    else if ((bool)cboxCorrectAnswer4.IsChecked)
                    {
                        _rightAnswer = 4;
                    }

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
                    image = _imgNaam,
                    timer = Convert.ToInt32(txbTimer.Text)
                },
            };
                    listQuestions.Add(question);
                    _numberOfQuestions++;
                    lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                    txbAnswer1.Text = null;
                    txbAnswer2.Text = null;
                    txbAnswer3.Text = null;
                    txbAnswer4.Text = null;
                    txbQuizVraag.Text = null;
                    txbTimer.Text = "30";
                    cboxCorrectAnswer1.IsChecked = false;
                    cboxCorrectAnswer2.IsChecked = false;
                    cboxCorrectAnswer3.IsChecked = false;
                    cboxCorrectAnswer4.IsChecked = false;
                    imgQuestion.Source = null;
                    gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                    borderGridImage.BorderThickness = new Thickness(3);
                    _selectedFileName = null;
                    _imgNaam = null;
                }
            }
            else if (_questionNumber < _numberOfQuestions)
            {
                if (_listNumber >= 1)
                {
                    listQuestions[_listNumber -1][0].answer1 = txbAnswer1.Text;
                    listQuestions[_listNumber -1][0].answer2 = txbAnswer2.Text;
                    listQuestions[_listNumber -1][0].answer3 = txbAnswer3.Text;
                    listQuestions[_listNumber -1][0].answer4 = txbAnswer4.Text;
                    listQuestions[_listNumber -1][0].question = txbQuizVraag.Text;
                    listQuestions[_listNumber -1][0].timer = Convert.ToInt32(txbTimer.Text);
                    if (_imgNaam != null)
                    {
                        listQuestions[_listNumber - 1][0].image = _imgNaam;
                    }
                    if ((bool)cboxCorrectAnswer1.IsChecked)
                    {
                        listQuestions[_listNumber -1][0].rightAnswer = 1;
                    }
                    else if ((bool)cboxCorrectAnswer2.IsChecked)
                    {
                        listQuestions[_listNumber -1][0].rightAnswer = 2;
                    }
                    else if ((bool)cboxCorrectAnswer3.IsChecked)
                    {
                        listQuestions[_listNumber -1][0].rightAnswer = 3;
                    }
                    else if ((bool)cboxCorrectAnswer4.IsChecked)
                    {
                        listQuestions[_listNumber -1][0].rightAnswer = 4;
                    }
                    if (_numberOfQuestions < _questionNumber)
                    {
                        txbAnswer1.Text = null;
                        txbAnswer2.Text = null;
                        txbAnswer3.Text = null;
                        txbAnswer4.Text = null;
                        txbQuizVraag.Text = null;
                        txbTimer.Text = "30";
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                        _imgNaam = null;
                        _selectedFileName = null;
                    } else
                    {
                        txbAnswer1.Text = listQuestions[_listNumber][0].answer1;
                        txbAnswer2.Text = listQuestions[_listNumber][0].answer2;
                        txbAnswer3.Text = listQuestions[_listNumber][0].answer3;
                        txbAnswer4.Text = listQuestions[_listNumber][0].answer4;
                        txbQuizVraag.Text = listQuestions[_listNumber][0].question;
                        txbTimer.Text = listQuestions[_listNumber][0].timer.ToString();
                        if (listQuestions[_listNumber][0].rightAnswer == 1)
                        {
                            cboxCorrectAnswer1.IsChecked = true;
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 2)
                        {
                            cboxCorrectAnswer2.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 3)
                        {
                            cboxCorrectAnswer3.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 4)
                        {
                            cboxCorrectAnswer4.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                        }
                        if (listSelectedImages[_listNumber] != null)
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(listSelectedImages[_listNumber]);
                            bitmap.EndInit();
                            imgQuestion.Source = bitmap;
                            gridImage.Background = null;
                            borderGridImage.BorderThickness = new Thickness(0);
                        }
                        else if (listQuestions[_listNumber][0].image != null)
                        {
                            baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                            baseDir = baseDir + @"Images\" + listQuestions[_listNumber][0].image;
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(baseDir);
                            bitmap.EndInit();
                            imgQuestion.Source = bitmap;
                            gridImage.Background = null;
                            borderGridImage.BorderThickness = new Thickness(0);
                            baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[_listNumber][0].image), "");
                        }
                        else
                        {
                            imgQuestion.Source = null;
                            gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                            borderGridImage.BorderThickness = new Thickness(3);
                        }
                        _imgNaam = null;
                        _selectedFileName = null;
                    }
                    lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                }

          
            } else if (_questionNumber == 2 && _numberOfQuestions > 0)
            {
                listQuestions[0][0].answer1 = txbAnswer1.Text;
                listQuestions[0][0].answer2 = txbAnswer2.Text;
                listQuestions[0][0].answer3 = txbAnswer3.Text;
                listQuestions[0][0].answer4 = txbAnswer4.Text;
                listQuestions[0][0].question = txbQuizVraag.Text;
                listQuestions[0][0].timer = Convert.ToInt32(txbTimer.Text);
                if (_imgNaam != null)
                {
                    listQuestions[0][0].image = _imgNaam;
                }
                _imgNaam = null;
                _selectedFileName = null;
                if ((bool)cboxCorrectAnswer1.IsChecked)
                {
                    listQuestions[0][0].rightAnswer = 1;
                }
                else if ((bool)cboxCorrectAnswer2.IsChecked)
                {
                    listQuestions[0][0].rightAnswer = 2;
                }
                else if ((bool)cboxCorrectAnswer3.IsChecked)
                {
                    listQuestions[0][0].rightAnswer = 3;
                }
                else if ((bool)cboxCorrectAnswer4.IsChecked)
                {
                    listQuestions[0][0].rightAnswer = 4;
                }

                if (_numberOfQuestions == 2)
                {
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
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 2)
                    {
                        cboxCorrectAnswer2.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 3)
                    {
                        cboxCorrectAnswer3.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 4)
                    {
                        cboxCorrectAnswer4.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                    }

                    if (listSelectedImages[_listNumber] != null)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(listSelectedImages[_listNumber]);
                        bitmap.EndInit();
                        imgQuestion.Source = bitmap;
                        gridImage.Background = null;
                        borderGridImage.BorderThickness = new Thickness(0);
                    }
                    else if (listQuestions[_listNumber][0].image != null)
                    {
                        baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                        baseDir = baseDir + @"Images\" + listQuestions[_listNumber][0].image;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(baseDir);
                        bitmap.EndInit();
                        imgQuestion.Source = bitmap;
                        gridImage.Background = null;
                        borderGridImage.BorderThickness = new Thickness(0);
                        baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[_listNumber][0].image), "");
                    }
                    else
                    {
                        imgQuestion.Source = null;
                        gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                        borderGridImage.BorderThickness = new Thickness(3);
                    }
                }
                else
                {
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
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 2)
                    {
                        cboxCorrectAnswer2.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 3)
                    {
                        cboxCorrectAnswer3.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer4.IsChecked = false;
                    }
                    else if (listQuestions[_listNumber][0].rightAnswer == 4)
                    {
                        cboxCorrectAnswer4.IsChecked = true;
                        cboxCorrectAnswer1.IsChecked = false;
                        cboxCorrectAnswer2.IsChecked = false;
                        cboxCorrectAnswer3.IsChecked = false;
                    }

                    if (listSelectedImages[_listNumber] != null)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(listSelectedImages[_listNumber]);
                        bitmap.EndInit();
                        imgQuestion.Source = bitmap;
                        gridImage.Background = null;
                        borderGridImage.BorderThickness = new Thickness(0);
                    }
                    else if (listQuestions[_listNumber][0].image != null)
                    {
                        baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                        baseDir = baseDir + @"Images\" + listQuestions[_listNumber][0].image;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(baseDir);
                        bitmap.EndInit();
                        imgQuestion.Source = bitmap;
                        gridImage.Background = null;
                        borderGridImage.BorderThickness = new Thickness(0);
                        baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[_listNumber][0].image), "");
                    }
                    else
                    {
                        imgQuestion.Source = null;
                        gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                        borderGridImage.BorderThickness = new Thickness(3);
                    }
                }
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
                
                if (_questionNumber <= _numberOfQuestions)
                {
                    listQuestions[_listNumber - 1][0].answer1 = txbAnswer1.Text;
                    listQuestions[_listNumber - 1][0].answer2 = txbAnswer2.Text;
                    listQuestions[_listNumber - 1][0].answer3 = txbAnswer3.Text;
                    listQuestions[_listNumber - 1][0].answer4 = txbAnswer4.Text;
                    listQuestions[_listNumber - 1][0].question = txbQuizVraag.Text;
                    listQuestions[_listNumber - 1][0].timer = Convert.ToInt32(txbTimer.Text);
                    if (_imgNaam != null)
                    {
                        listQuestions[_listNumber - 1][0].image = _imgNaam;
                    }
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

                    if (_questionNumber <= _numberOfQuestions)
                    {
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
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 2)
                        {
                            cboxCorrectAnswer2.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 3)
                        {
                            cboxCorrectAnswer3.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer4.IsChecked = false;
                        }
                        else if (listQuestions[_listNumber][0].rightAnswer == 4)
                        {
                            cboxCorrectAnswer4.IsChecked = true;
                            cboxCorrectAnswer1.IsChecked = false;
                            cboxCorrectAnswer2.IsChecked = false;
                            cboxCorrectAnswer3.IsChecked = false;
                        }
                        if (listSelectedImages[_listNumber] != null)
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(listSelectedImages[_listNumber]);
                            bitmap.EndInit();
                            imgQuestion.Source = bitmap;
                            gridImage.Background = null;
                            borderGridImage.BorderThickness = new Thickness(0);
                        }
                        else if (listQuestions[_listNumber][0].image != null)
                        {
                            baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                            baseDir = baseDir + @"Images\" + listQuestions[_listNumber][0].image;
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(baseDir);
                            bitmap.EndInit();
                            imgQuestion.Source = bitmap;
                            gridImage.Background = null;
                            borderGridImage.BorderThickness = new Thickness(0);
                            baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[_listNumber][0].image), "");
                        }
                        else
                        {
                            imgQuestion.Source = null;
                            gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                            borderGridImage.BorderThickness = new Thickness(3);
                        }
                    }
                }
                else if (_questionNumber > _numberOfQuestions)
                {
                    listQuestions[_listNumber - 1][0].answer1 = txbAnswer1.Text;
                    listQuestions[_listNumber - 1][0].answer2 = txbAnswer2.Text;
                    listQuestions[_listNumber - 1][0].answer3 = txbAnswer3.Text;
                    listQuestions[_listNumber - 1][0].answer4 = txbAnswer4.Text;
                    listQuestions[_listNumber - 1][0].question = txbQuizVraag.Text;
                    listQuestions[_listNumber - 1][0].timer = Convert.ToInt32(txbTimer.Text);
                    if (_imgNaam != null)
                    {
                        listQuestions[_listNumber - 1][0].image = _imgNaam;
                    }
                    lblQuestionNumber.Content = $"Vraag {_questionNumber}";
                    txbAnswer1.Text = null;
                    txbAnswer2.Text = null;
                    txbAnswer3.Text = null;
                    txbAnswer4.Text = null;
                    txbQuizVraag.Text = null;
                    txbTimer.Text = "30";
                    cboxCorrectAnswer1.IsChecked = false;
                    cboxCorrectAnswer2.IsChecked = false;
                    cboxCorrectAnswer3.IsChecked = false;
                    cboxCorrectAnswer4.IsChecked = false;
                    imgQuestion.Source = null;
                    gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                    borderGridImage.BorderThickness = new Thickness(3);
                } 
                _imgNaam = null;
                _selectedFileName = null;
            }
        }

        private void BtnVorige_Click(object sender, RoutedEventArgs e)
        {
            if (_questionNumber != 1)
            {
                _questionNumber--;
            }
            _listNumber = _questionNumber - 1;

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
                cboxCorrectAnswer2.IsChecked = false;
                cboxCorrectAnswer3.IsChecked = false;
                cboxCorrectAnswer4.IsChecked = false;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 2)
            {
                cboxCorrectAnswer2.IsChecked = true;
                cboxCorrectAnswer1.IsChecked = false;
                cboxCorrectAnswer3.IsChecked = false;
                cboxCorrectAnswer4.IsChecked = false;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 3)
            {
                cboxCorrectAnswer3.IsChecked = true;
                cboxCorrectAnswer1.IsChecked = false;
                cboxCorrectAnswer2.IsChecked = false;
                cboxCorrectAnswer4.IsChecked = false;
            }
            else if (listQuestions[_listNumber][0].rightAnswer == 4)
            {
                cboxCorrectAnswer4.IsChecked = true;
                cboxCorrectAnswer1.IsChecked = false;
                cboxCorrectAnswer2.IsChecked = false;
                cboxCorrectAnswer3.IsChecked = false;
            }

            if (listSelectedImages[_listNumber] != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(listSelectedImages[_listNumber]);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);
            }
            else if (listQuestions[_listNumber][0].image != null)
            {
                baseDir = baseDir.Replace(@"bin\Debug\Images", "");
                baseDir = baseDir + @"Images\" + listQuestions[_listNumber][0].image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(baseDir);
                bitmap.EndInit();
                imgQuestion.Source = bitmap;
                gridImage.Background = null;
                borderGridImage.BorderThickness = new Thickness(0);
                baseDir = baseDir.Replace(string.Format(@"Images\{0}", listQuestions[_listNumber][0].image), "");
            }
            else
            {
                imgQuestion.Source = null;
                gridImage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B2FFFFFF");
                borderGridImage.BorderThickness = new Thickness(3);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(listQuestions);
            db.SaveEditQuiz(txbQuizTitel.Text, json, quizID);
            for (int i = 0; i < listSelectedImages.Count; i++) // Loop through List with for
            {
                string destinationPath = GetDestinationPath(listQuestions[i][0].image, "Images");
                MessageBox.Show(listSelectedImages[i]);
                if (listSelectedImages[i] != null)
                {
                    File.Copy(listSelectedImages[i], destinationPath, true);
                    MessageBox.Show("Gelukt");
                }
            }
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
