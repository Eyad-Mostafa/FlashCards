using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.UI;

static internal class Menu
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Welcome to Flashcards App");
        PauseForUser();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please Enter your choice");
            Console.WriteLine("1- Add Stack");
            Console.WriteLine("2- Manage Stacks");
            Console.WriteLine("3- Study");
            Console.WriteLine("4- View StudySessions");
            Console.WriteLine("5- Exit");
            Console.WriteLine("");
            switch(Console.ReadLine()?.Trim())
            {
                case "1":
                    AddStack();
                    break;
                case "2":
                    ViewStacks();
                    break;
                case "3":
                    StudyMenu();
                    break;
                case "4":
                    StudySessionsMenu();
                    break;
                case "5":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice, Try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    private static void PauseForUser()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void AddStack()
    {
        Console.Write("Enter the name of the stack: ");
        var stackName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        var Stacks = DatabaseManager.GetStacks();
        if (Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Stack with the same name already exists. Please try again.");
            PauseForUser();
            return;
        }
        var stack = new Stack { Name = stackName };

        DatabaseManager.AddStack(stack);
        Console.WriteLine("Stack added successfully.");
        PauseForUser();
    }

    private static void ViewStacks()
    {
        var Stacks = DatabaseManager.GetStacks();
        var SelectedStack = new Stack();

        if (Stacks == null || Stacks.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No stacks available. Please add a stack first.");
            Console.WriteLine("Please Enter Another choice.");
            PauseForUser();
            return;
        }
        Console.Clear();
        Console.WriteLine("Stacks:");

        var DTOStacks = Stacks.Select(s => new StackDTO
        {
            Name = s.Name
        }).ToList();

        foreach (var stack in DTOStacks)
        {
            Console.WriteLine($"- {stack.Name}");
        }

        Console.WriteLine("Please enter a stack name to manage or 0 to go back to the main menu.");
        string stackName;
        while (true)
        {
            stackName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(stackName))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }

            if (stackName == "0")
            {
                return;
            }

            if (!Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid stack name. Please enter a valid name from the list.");
                continue;
            }
            else
            {
                SelectedStack = Stacks.First(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));
            }

            break;
        }

        ManageStacksMenu(SelectedStack);
    }

    private static void ManageStacksMenu(Stack stack)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Managing {stack.Name} Stack\n");
            Console.WriteLine("Please Enter your choice");
            Console.WriteLine("1- Add Flashcard");
            Console.WriteLine("2- View Flashcards");
            Console.WriteLine("3- Edit Flashcard");
            Console.WriteLine("4- Delete Flashcard");
            Console.WriteLine("5- Delete Stack");
            Console.WriteLine("0- Back to main menu");
            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    AddFlashcard(stack);
                    PauseForUser();
                    break;
                case "2":
                    ViewFlashcards(stack);
                    PauseForUser();
                    break;
                case "3":
                    EditFlashcard(stack);
                    break;
                case "4":
                    DatabaseManager.DeleteFlashcard(new Flashcard { StackId = stack.StackId });
                    break;
                case "5":
                    DatabaseManager.DeleteStack(stack);
                    Console.WriteLine("Stack deleted successfully.");
                    PauseForUser();
                    return;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice, Try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    private static void AddFlashcard(Stack stack)
    {
        var flashcard = new Flashcard { StackId = stack.StackId};
        Console.Write("Enter Question : ");
        string question = Console.ReadLine();
        Console.Write("Enter Answer : ");
        string answer = Console.ReadLine();
        if (string.IsNullOrEmpty(answer) || string.IsNullOrEmpty(question))
        {
            Console.WriteLine("Question and Answer cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        flashcard.Answer = answer;
        flashcard.Question = question;
        DatabaseManager.AddFlashcard(flashcard);
    }

    private static List<Flashcard>? ViewFlashcards(Stack stack)
    {
        var flashcards = new List<Flashcard>();
        flashcards = DatabaseManager.GetFlashcards(stack.StackId);
        if (flashcards == null || flashcards.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No flashcards available. Please add a flashcard first.");
            Console.WriteLine("Please Enter Another choice.");
            return flashcards;
        }
        Console.Clear();
        Console.WriteLine($"Flash Cards for {stack.Name} :");
        
        for (int i = 0; i < flashcards.Count; i++)
        {

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Flashcard Id: {i+1}");
            Console.WriteLine($"Question: {flashcards[i].Question}");
            Console.WriteLine($"Answer: {flashcards[i].Answer}");
            Console.WriteLine("-------------------------------------------------");
        }
        return flashcards;
    }

    private static void EditFlashcard(Stack stack)
    {
        var flashcards = ViewFlashcards(stack);
        if (flashcards == null || flashcards.Count == 0)
        {
            PauseForUser();
            return;
        }

        Console.WriteLine("Enter the ID of the flashcard you want to edit");
        int id = int.Parse(Console.ReadLine());
        if (id < 1 || id > flashcards.Count)
        {
            Console.WriteLine("Invalid ID. Please try again.");
            PauseForUser();
            return;
        }
        Console.WriteLine("Enter the new question");
        string question = Console.ReadLine();
        Console.WriteLine("Enter the new answer");
        string answer = Console.ReadLine();
        if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
        {
            Console.WriteLine("Question and Answer cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        var newFlashcard = new Flashcard
        {
            FlashcardId = flashcards[id - 1].FlashcardId,
            StackId = stack.StackId,
            Question = question,
            Answer = answer
        };
        DatabaseManager.EditFlashcard(newFlashcard);
        Console.WriteLine("Flashcard edited successfully.");
        PauseForUser();
    }

    private static void StudyMenu()
    {
        Console.Clear();
        Console.WriteLine("Please Enter Name of the stack you want to study from. Enter 0 to back");
        var stackName = Console.ReadLine()?.Trim();
        if (stackName == "0")
        {
            return;
        }
    }

    private static void StudySessionsMenu()
    {
        throw new NotImplementedException();
    }
}
