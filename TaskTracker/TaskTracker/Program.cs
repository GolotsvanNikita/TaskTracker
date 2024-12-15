using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTrackerProgram.Model;
using Task = TaskTrackerProgram.Model.Task;
namespace TaskTrackerProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tracker = new TaskTracker();
            bool running = true;

            while (running)
            {
                Console.WriteLine("Task Tracker Menu:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Remove Task");
                Console.WriteLine("3. Mark Task as Completed");
                Console.WriteLine("4. Display Tasks");
                Console.WriteLine("5. Save Task to JSON");
                Console.WriteLine("6. Load Task from JSON");
                Console.WriteLine("7. Save all Tasks to JSON");
                Console.WriteLine("8. Load all Tasks from JSON");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        ArgumentNullException.ThrowIfNull(description);
                        Console.Write("Enter task priority (High, Medium, Low): ");
                        List<string> priorityList = new List<string>() { "high", "medium", "low" };
                        string priority = Console.ReadLine();
                        ArgumentNullException.ThrowIfNull(priority);
                        if (priorityList.Contains(priority.ToLower()))
                        {
                            tracker.AddTask(description, priority.ToLower());
                            Console.WriteLine("Task added.");
                        }
                        else
                        {
                            Console.WriteLine("Task not added. Incorrect name of priority.");
                        }
                        break;
                    case "2":
                        Console.Write("Enter task description to remove: ");
                        string taskToRemove = Console.ReadLine();
                        tracker.RemoveTask(taskToRemove);
                        break;

                    case "3":
                        Console.Write("Enter task description to mark as completed: ");
                        string taskToComplete = Console.ReadLine();
                        tracker.TaskCompleted(taskToComplete);
                        Console.WriteLine("Task marked as completed.");
                        break;

                    case "4":
                        Console.WriteLine("Current tasks:");
                        tracker.DisplayTasks();
                        break;

                    case "5":
                        Console.Write("Enter task description to save as JSON: ");
                        string taskToSave = Console.ReadLine();
                        Task foundTask = tracker.DescriptionFound(taskToSave);
                        if (foundTask == null)
                        {
                            Console.WriteLine("Task not found.");
                            break;
                        }

                        Console.Write("Enter file path to save JSON (e.g., C:\\Users\\nameUser\\Desktop\\testTask.json): ");
                        string savePath = Console.ReadLine();
                        if (savePath.IsIncorrectPath())
                        {
                            break;
                        }

                        try
                        {
                            tracker.SaveJson(tracker.JsonSerialize(foundTask), savePath);
                            Console.WriteLine("Task saved to JSON.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "6":
                        Console.Write("Enter file path to load JSON (e.g., C:\\Users\\nameUser\\Desktop\\testTask.json): ");
                        string loadPath = Console.ReadLine();
                        try
                        {
                            string loadedJson = tracker.LoadJson(loadPath);
                            Task loadedTask = tracker.DeserializeJson(loadedJson);
                            Console.WriteLine("Loaded Task:");
                            Console.WriteLine($"Description: {loadedTask.Description}");
                            Console.WriteLine($"Priority: {loadedTask.Priority}");
                            Console.WriteLine($"Is Completed: {loadedTask.IsCompleted}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading JSON: {ex.Message}");
                        }
                        break;
                    case "7":
                        Console.Write("Enter file path to save JSON (e.g., C:\\Users\\nameUser\\Desktop\\Tasks.json): ");
                        string save = Console.ReadLine();

                        if (save.IsIncorrectPath())
                        {
                            break;
                        }
                        else
                        {
                            tracker.SerializeAllTasksToJson(save);
                        }
                        break;
                    case "8":
                        Console.Write("Enter file path to load JSON (e.g., C:\\Users\\nameUser\\Desktop\\Tasks.json): ");
                        string load = Console.ReadLine();

                        if (load.IsIncorrectPath())
                        {
                            break;
                        }
                        else
                        {
                            tracker.DeserializeTasksFromJson(load);
                        }
                        break;

                    case "9":
                        running = false;
                        Console.WriteLine("Exiting Task Tracker. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
    public static class BooleanExtension
    {
        public static bool IsIncorrectPath(this string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                Console.WriteLine("Invalid file path.");
                return true;
            }
            return false;
        }
    }
}
