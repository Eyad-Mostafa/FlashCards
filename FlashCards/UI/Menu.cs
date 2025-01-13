using FlashCards.Database;

namespace FlashCards.UI;

static internal class Menu
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Welcome to Flashcards App");
        Console.WriteLine("Please Enter your choice");
        Console.WriteLine("1- Manage Stacks");
        Console.WriteLine("2- Manage Flashcards");
        Console.WriteLine("3- Study");
        Console.WriteLine("4- View StudySessions");
        Console.WriteLine("5- Exit");
        Console.WriteLine("");
        while (true)
        {
            switch(Console.ReadLine()?.Trim())
            {
                case "1":
                    ViewStacks();
                    break;
                case "2":
                    ManageFlashcardsMenu();
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
                    Console.WriteLine("Invalid choice, Try again");
                    break;
            }
        }
    }

    private static void ViewStacks()
    {
        var Stacks = DatabaseManager.GetStacks();

        if (Stacks == null || Stacks.Count == 0)
        {
            Console.WriteLine("No stacks available. Please add a stack first.");
            return;
        }

        Console.WriteLine("Please enter a stack name to manage or 0 to go back to the main menu.");
        Console.WriteLine("Stacks:");
        foreach (var stack in Stacks)
        {
            Console.WriteLine($"- {stack.Name}");
        }

        string stackName;
        while (true)
        {
            Console.Write("Enter your choice: ");
            stackName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(stackName))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }

            if (stackName == "0")
            {
                return; // Go back to the main menu
            }

            // Check if the entered stack name exists
            if (!Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid stack name. Please enter a valid name from the list.");
                continue;
            }

            break; // Valid input, exit the loop
        }

        ManageStacksMenu(stackName);
    }


    private static void ManageStacksMenu(string stackName)
    {
        Console.Clear();
        Console.WriteLine("Manage Stacks");
        Console.WriteLine("Please Enter your choice");
        Console.WriteLine("0- Back to main menu");
        Console.WriteLine("1- Add Stack");
        Console.WriteLine("2- Edit Stack");
        Console.WriteLine("3- Delete Stack");
        Console.WriteLine("2- Manage Flashcards");
        Console.WriteLine("3- Study");
        Console.WriteLine("4- View StudySessions");
        Console.WriteLine("5- Exit");
        Console.WriteLine("");
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
