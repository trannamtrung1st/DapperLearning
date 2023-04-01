// Connect to the database 
using Dapper;
using Microsoft.Data.SqlClient;

var connectionString = "";

using (var connection = new SqlConnection(connectionString))
{
    // Create a query that retrieves all books with an author name of "John Smith"    
    var sql = "SELECT * FROM Books WHERE Author = @authorName";

    // Use the Query method to execute the query and return a list of objects    
    var books = connection.Query<object>(sql, new { authorName = "John Smith" }).ToList();
}