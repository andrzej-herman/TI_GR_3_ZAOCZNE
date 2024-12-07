using System.Data.SqlClient;

var path = "S:\\ZIMA 2024\\TECHNOLOGIE_INTERNETOWE\\questions.json";
var text = File.ReadAllText(path);
var ile = text.Length;

var connectionString = "Server=.\\HERMANLOCAL;Database=QuizDb3;Integrated Security=True";
var connection = new SqlConnection(connectionString);	
connection.Open();
var query = "SELECT * FROM Questions";
var cmd = new SqlCommand(query, connection);
var reader = cmd.ExecuteReader();
while (reader.Read())
{
	var id = reader.GetInt32(0);
	var category = reader.GetInt32(1);
	var content = reader.GetString(2);
    Console.WriteLine(id);
	Console.WriteLine(category);
    Console.WriteLine(content);
}

connection.Close();
Console.ReadLine();