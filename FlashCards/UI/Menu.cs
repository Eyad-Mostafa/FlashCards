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
            Console.WriteLine("3- Manage Flashcards");
            Console.WriteLine("4- Study");
            Console.WriteLine("5- View StudySessions");
            Console.WriteLine("6- Exit");
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
                    ManageFlashcardsMenu();
                    break;
                case "4":
                    StudyMenu();
                    break;
                case "5":
                    StudySessionsMenu();
                    break;
                case "6":
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
        Console.WriteLine("Press any key to continue...");
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
        foreach (var stack in Stacks)
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

            break;
        }

        ManageStacksMenu(stackName);
    }


    private static void ManageStacksMenu(string stackName)
    {
        Console.Clear();
        Console.WriteLine($"Managing {stackName} Stack");
        Console.WriteLine("Please Enter your choice");
        Console.WriteLine("1- Add Flashcard");
        Console.WriteLine("2- View Flashcards");
        Console.WriteLine("3- Edit Flashcard");
        Console.WriteLine("4- Delete Flashcard");
        Console.WriteLine("5- Delete Stack");
        Console.WriteLine("0- Back to main menu");
        PauseForUser();
    }
    private static void ManageFlashcardsMenu()
    {
        throw new NotImplementedException();
    }

    private static void StudyMenu()
    {
        throw new NotImplementedException();
    }

    private static void StudySessionsMenu()
    {
        throw new NotImplementedException();
    }
}
