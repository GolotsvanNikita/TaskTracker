using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskTrackerProgram.Model
{
    internal class TaskTracker
    {
        private List<Task> _tasks = new();

        public void AddTask(string description, string priority)
        {
            _tasks.Add(new Task { Description = description, Priority = priority, IsCompleted = false });
            SortTasksByPriority();
        }

        public void RemoveTask(string description)
        {
            ArgumentNullException.ThrowIfNull(_tasks);

            var taskToRemove = _tasks.FirstOrDefault(task => task.Description == description);

            if (taskToRemove != null)
            {
                _tasks.Remove(taskToRemove);
                Console.WriteLine("Task deleted.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }

        public void TaskCompleted(string description)
        {
            ArgumentNullException.ThrowIfNull(_tasks);
            foreach (var task in _tasks)
            {
                if (task.Description == description)
                {
                    task.IsCompleted = true;
                }
            }
        }

        public void DisplayTasks()
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("No tasks available!");
                return;
            }

            for (int i = 0; i < _tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_tasks[i]}");
            }
            Console.WriteLine();
        }

        public string JsonSerialize(Task task)
        {
            ArgumentNullException.ThrowIfNull(task);

            return JsonSerializer.Serialize(task) ?? throw new JsonException("Serialize error");
        }

        public void SaveJson(string json, string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Path.IsPathFullyQualified(path))
            {
                throw new ArgumentException("Invalid file path.");
            }

            ArgumentNullException.ThrowIfNull(json);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                using FileStream fstream = File.Create(path);
                fstream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public string LoadJson(string path)
        {
            ArgumentNullException.ThrowIfNull(path);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            return File.ReadAllText(path);
        }

        public Task DeserializeJson(string json)
        {
            ArgumentNullException.ThrowIfNull(json);

            return JsonSerializer.Deserialize<Task>(json) ?? throw new JsonException("Deserialize error");
        }

        public Task DescriptionFound(string description)
        {
            foreach (var task in _tasks)
            {
                if (task.Description == description)
                {
                    return task;
                }
            }
            return null;
        }
        public void SerializeAllTasksToJson(string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine("All tasks have been serialized to JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while serializing tasks: {ex.Message}");
            }
        }
        public void DeserializeTasksFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    return;
                }

                string json = File.ReadAllText(filePath);
                var tasksFromJson = JsonSerializer.Deserialize<List<Task>>(json);

                if (tasksFromJson != null)
                {
                    _tasks.AddRange(tasksFromJson);
                    SortTasksByPriority();
                    Console.WriteLine("All tasks have been deserialized and added to the task list.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deserializing tasks: {ex.Message}");
            }
        }
        private void SortTasksByPriority()
        {
            _tasks.Sort((x, y) => GetPriorityValue(y.Priority).CompareTo(GetPriorityValue(x.Priority)));
        }

        private int GetPriorityValue(string priority)
        {
            return priority switch
            {
                "high" => 3,
                "medium" => 2,
                "low" => 1,
                _ => 0
            };
        }
    }
}
