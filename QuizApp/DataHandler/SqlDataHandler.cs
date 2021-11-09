using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace QuizApp.DataHandler
{
    //SQLite är filbaserad vilket innebär att hela databasen finns lagrad i en enda fil på disk.
    //Vill man flytta databasen så räcker det att flytta filen.
    //Det behövs ingen särskild konfiguration av användare och lösenord. Enkelheten är en av fördelarna med en filbaserad databas.

    //Tanken var att kunna spara ALLT I JSON Men jag ville kolla lite närmre på SQL och detta är ju  SQLite som är filbaserad vilket innebär att hela databasen finns lagrad i en enda fil på disk.
    //Vill man flytta databasen så räcker det att flytta filen.
    class SqlDataHandler
    {
        string DirName = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%");
        string ConnName = string.Empty;
        string ConnectionString = string.Empty;

        public SqlDataHandler()
        {
            ConnName = $"{DirName}\\MyDatabase.db";
            ConnectionString = $"Data Source={ConnName}; Version=3;";
            CreateDB();
        }


       //Skapa en ny SQLITE DB om den inte finns
        private void CreateDB()
        {
            if (!File.Exists(ConnName))
            {
                //Directory.CreateDirectory($@"{DirName}\QuizImages");
                //SQLiteConnection.CreateFile(ConnName);
                var dbConnection = new SQLiteConnection(ConnectionString);
                dbConnection.Open();

                string sqlcommand = @"CREATE TABLE Quiz (
                                      id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      QuestionText TEXT NOT NULL,
                                      Answers TEXT NOT NULL,
                                      CorrectAnswer INTEGER NOT NULL,
                                      QuizImage TEXT
                                      );";
                var command = new SQLiteCommand(sqlcommand, dbConnection);
                command.ExecuteNonQuery();
            }
        }

        // hämta quiz frågor, av lista Quesitons 
        public async Task<List<Questions>> LoadQuiz()
        {
            var questions = new List<Questions>();

            using (IDbConnection connection = new SQLiteConnection(ConnectionString))
            {
                var questionsList = await connection.QueryAsync<Questions>("SELECT id, QuestionText, Answers, CorrectAnswer, QuizImage from Quiz");
                questions = questionsList.ToList<Questions>();
            }

            return questions;
        }

        //Lägg till nytt quiz+ frågor answers bilder
        public void AddNewQuiz(Questions questions)
        {
            using (IDbConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute($"INSERT into Quiz(QuestionText, Answers, CorrectAnswer, QuizImage) VALUES('{questions.QuestionText}','{questions.Answer}',{questions.CorrectAnswer}, '{questions.QuizImage}')");
            }
        }

        //Uppdaterar quiz när man skapar eller editerar
        public void UpdateQuiz(Questions questions)
        {
            using (IDbConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute($"UPDATE Quiz SET QuestionText = '{questions.QuestionText}', Answers = '{questions.Answer}', CorrectAnswer= {questions.CorrectAnswer}, QuizImage = '{questions.QuizImage}' WHERE id =={questions.id} ");
            }
        }

        //Ta bort quiz frågor eller quiz efter QUIZ ID
        public void DelteQuiz(int ID)
        {
            using (IDbConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute($"DELETE FROM Quiz WHERE id=={ID}");
            }
        }//
    }
}
