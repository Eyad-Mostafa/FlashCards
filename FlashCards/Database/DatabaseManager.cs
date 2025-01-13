using Microsoft.Data.SqlClient;
using FlashCards.Models;

namespace FlashCards.Database;

internal static class DatabaseManager
{
    private static readonly string _connectionString = "Server=localhost\\MSSQLSERVER08;Trusted_Connection=True;TrustServerCertificate=True;";
    private static readonly string _databaseConnectionString = "Server=localhost\\MSSQLSERVER08;Database=FlashCards;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Start()
    {
        CreateDatabase();
        CreateTabels();
    }

    private static void CreateDatabase()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        try
        {
            connection.Open();

            string createDatabaseQuery = @" IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashCards')
                    BEGIN
                        CREATE DATABASE FlashCards;
                    END;
                ";

            using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateTabels()
    { 
        string createStacksTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Stacks' AND xtype = 'U')
            BEGIN
                CREATE TABLE Stacks (
                    StackID INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) UNIQUE NOT NULL
                );
            END;
        ";

        string createFlashcardsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Flashcards' AND xtype = 'U')
            BEGIN
                CREATE TABLE Flashcards (
                    FlashcardID INT IDENTITY(1,1) PRIMARY KEY,
                    StackID INT NOT NULL,
                    Question NVARCHAR(255) NOT NULL,
                    Answer NVARCHAR(255) NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE
                );
            END;
        ";

        string createStudySessionsTable = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'StudySessions' AND xtype = 'U')
            BEGIN
                CREATE TABLE StudySessions (
                    SessionID INT IDENTITY(1,1) PRIMARY KEY,
                    StackID INT NOT NULL,
                    Date DATE NOT NULL,
                    Score FLOAT NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE
                );
            END;
        ";

        ExecuteSqlScript(_databaseConnectionString, createStacksTable);
        ExecuteSqlScript(_databaseConnectionString, createFlashcardsTable);
        ExecuteSqlScript(_databaseConnectionString, createStudySessionsTable);
    }

    private static void ExecuteSqlScript(string connectionString, string script)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            using SqlCommand command = new SqlCommand(script, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    internal static List<Stack> GetStacks()
    {
        var Stacks = new List<Stack>();

        string script = "SELECT * FROM Stacks";

        using SqlConnection connection = new SqlConnection(_databaseConnectionString);
        try
        {
            connection.Open();

            using SqlCommand command = new SqlCommand(script, connection);
            {
                using var reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        Stacks.Add(new Stack
                        {
                            StackId = (int)reader["StackID"],
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }

        return Stacks;
    }

    internal static void AddStack(Stack stack)
    {
        using (SqlConnection connection = new SqlConnection(_databaseConnectionString))
        try
        {
            connection.Open();

            string script = "INSERT INTO Stacks (Name) VALUES (@Name)";

            using (SqlCommand command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@Name", stack.Name);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }

    internal static List<Flashcard> GetFlashcards(int StackId)
    {
        var Flashcards = new List<Flashcard>();
        string script = "SELECT * FROM Flashcards WHERE StackID = @StackID";

        using (SqlConnection connection = new SqlConnection(_databaseConnectionString))
        try
        {
            connection.Open();
            
            using (SqlCommand command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@StackID", StackId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flashcards.Add(new Flashcard
                        {
                            FlashcardId = (int)reader["FlashcardID"],
                            StackId = (int)reader["StackID"],
                            Question = (string)reader["Question"],
                            Answer = (string)reader["Answer"]
                        });
                    }
                }
            }
        }
        catch
        {

        }

        return Flashcards;
    }

    internal static void AddFlashcard(Flashcard flashcard)
    {
        string script = "INSERT INTO Flashcards (StackID, Question, Answer) VALUES (@StackID, @Question, @Answer)";

        using (SqlConnection connection = new SqlConnection(_databaseConnectionString))
        try
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(script, connection))
            {
                command.Parameters.AddWithValue("@StackID", flashcard.StackId);
                command.Parameters.AddWithValue("@Question", flashcard.Question);
                command.Parameters.AddWithValue("@Answer", flashcard.Answer);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }
}