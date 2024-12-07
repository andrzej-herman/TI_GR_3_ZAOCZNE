using Microsoft.Data.SqlClient;
using Quiz.Data;

namespace Quiz.Api.Services
{
    public class QuizService : IQuizService
    {
        private const string connectionString = "Server=.\\HERMANLOCAL;Database=CqrsTp2;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection connection;
        Random random;

        public QuizService()
        {
            connection = new SqlConnection(connectionString);  
            random = new Random();
        }

        public async Task<QuestionDto?> GetQuestionAsync(int category)
        {
            try
            {
                var questions = new List<QuestionDto>();
                await connection.OpenAsync();
                var query = "select * from Questions where QuestionCategory = @category";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@category", category);
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetGuid(0);
                    var cat = reader.GetInt32(1);
                    var content = reader.GetString(2);
                    var question = new QuestionDto { Id = id, Category = cat, Content = content };
                    questions.Add(question);
                }
                await reader.CloseAsync();

                if (questions.Count == 0) return null;
                var randomNumber = random.Next(0, questions.Count);
                var selectedQuestion = questions[randomNumber];


                query = "select AnswerId, AnswerContent from Answers where QuestionId = @answerId";
                cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@answerId", selectedQuestion.Id);
                var answerReader = await cmd.ExecuteReaderAsync();
                while (await answerReader.ReadAsync())
                {
                    var id = answerReader.GetGuid(0);
                    var content = answerReader.GetString(1);
                    var answer = new AnswerDto { Id = id, Content = content };
                    selectedQuestion.Answers.Add(answer);
                }

                await connection.CloseAsync();
                return selectedQuestion;
            }
            catch (Exception)
            {
               return null;
            }
        }

        public async Task<CheckAnswerDto?> GetCheckAnswerAsync(Guid answerId, int category)
        {
            try
            {
                List<int> categories = [100, 200, 300, 400, 500, 750, 1000];
                bool isCorrect = false;
                await connection.OpenAsync();
                var query = "select AnswerIsCorrect from Answers where AnswerId = @answerId";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@answerId", answerId);
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    isCorrect = reader.GetBoolean(0);
                  
                await reader.CloseAsync();

                var index = categories.IndexOf(category);
                var nextCategory = index != 6 ? categories[index + 1] : 0;
                return new CheckAnswerDto { IsCorrect = isCorrect, NextCategory = nextCategory };
            }
            catch (Exception)
            {
                return null;
            }
        }      
    }
}
