using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace QuizTime
{
    class database
    {
        private string _ConnecDB = "server=localhost;userid=root;password=;database=quiztime";

        public MySqlDataReader SelectQuizVragen(string ID)
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            var sql = $"SELECT `vragen` FROM `quiz` WHERE `ID` = {ID}";
            MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
            return cmd.ExecuteReader();
        }

        public MySqlDataReader SelectQuizID()
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            var sql = $"SELECT `id` FROM `quiz`";
            MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
            return cmd.ExecuteReader();
        }
        public MySqlDataReader SelectedQuizTitel(string ID)
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            var sql = $"SELECT `titel` FROM `quiz` WHERE ID = {ID}";
            MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
            return cmd.ExecuteReader();
        }

        public MySqlDataReader SelectQuizTitel()
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            var sql = $"SELECT `titel` FROM `quiz`";
            MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
            return cmd.ExecuteReader();
        }

        public void SaveQuiz(string titel, string jsonArray)
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            try
            {
                var sql = $"INSERT INTO `quiz` (titel, vragen) VALUES ('{titel}', '{jsonArray}')";
                MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
                cmd.ExecuteReader();
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }          
           
        }

        public void SaveEditQuiz(string titel, string jsonArray, string ID)
        {
            MySqlConnection dvDBConnect = new MySqlConnection(_ConnecDB);
            dvDBConnect.Open();
            try
            {
                var sql = $"UPDATE `quiz` SET `titel`='{titel}',`vragen`='{jsonArray}' WHERE ID = {ID}";
                MySqlCommand cmd = new MySqlCommand(sql, dvDBConnect);
                cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
