using System;
using System.Collections.Generic;

class TaskItem {
  public int Id { get; set; }
  public string Name { get; set; }
  public bool IsDone { get; set; }

  public TaskItem(int id, string name) {
    Id = id;
    Name = name;
    IsDone = false;
  }

  public override string ToString() {
    return $"{Id}. [{(IsDone ? "x" : " ")}] {Name}";
  }
}

interface ICommand {
  void Execute();
  void Undo();
}

class TaskManager {
  public List<TaskItem> Tasks { get; } = new();
  private readonly Stack<ICommand> undoStack = new();
  private readonly Stack<ICommand> redoStack = new();
  private int nextId = 1;

  public void Add(string name) {
    RunCommand(new AddTaskCommand(this, new TaskItem(nextId++, name)));
  }

  public void Complete(int id) {
    TaskItem? task = Tasks.Find(t => t.Id == id);
    if (task == null) {
      Console.WriteLine("Task not found.");
      return;
    }
    if (task.IsDone) {
      Console.WriteLine("Task is already completed.");
      return;
    }
    RunCommand(new CompleteTaskCommand(task));
  }

  public void Remove(int id) {
    TaskItem? task = Tasks.Find(t => t.Id == id);
    if (task == null) {
      Console.WriteLine("Task not found.");
      return;
    }
    RunCommand(new RemoveTaskCommand(this, task));
  }

  public void Undo() {
    if (undoStack.Count == 0) {
      Console.WriteLine("Nothing to undo.");
      return;
    }
    ICommand command = undoStack.Pop();
    command.Undo();
    redoStack.Push(command);
  }

  public void Redo() {
    if (redoStack.Count == 0) {
      Console.WriteLine("Nothing to redo.");
      return;
    }
    ICommand command = redoStack.Pop();
    command.Execute();
    undoStack.Push(command);
  }

  private void RunCommand(ICommand command) {
    command.Execute();
    undoStack.Push(command);
    redoStack.Clear();
  }
}

class AddTaskCommand : ICommand {
  private readonly TaskManager manager;
  private readonly TaskItem task;

  public AddTaskCommand(TaskManager manager, TaskItem task) {
    this.manager = manager;
    this.task = task;
  }

  public void Execute() {
    manager.Tasks.Add(task);
  }

  public void Undo() {
    manager.Tasks.Remove(task);
  }
}

class CompleteTaskCommand : ICommand {
  private readonly TaskItem task;

  public CompleteTaskCommand(TaskItem task) {
    this.task = task;
  }

  public void Execute() {
    task.IsDone = true;
  }

  public void Undo() {
    task.IsDone = false;
  }
}

class RemoveTaskCommand : ICommand {
  private readonly TaskManager manager;
  private readonly TaskItem task;
  private int index;

  public RemoveTaskCommand(TaskManager manager, TaskItem task) {
    this.manager = manager;
    this.task = task;
  }

  public void Execute() {
    index = manager.Tasks.IndexOf(task);
    manager.Tasks.Remove(task);
  }

  public void Undo() {
    manager.Tasks.Insert(index, task);
  }
}

class Program {
  public static void Main(string[] args) {
    TaskManager manager = new TaskManager();

    while (true) {
      Console.WriteLine("\n=== Task Manager ===");
      Console.WriteLine("1. Show tasks");
      Console.WriteLine("2. Add task");
      Console.WriteLine("3. Complete task");
      Console.WriteLine("4. Remove task");
      Console.WriteLine("5. Undo");
      Console.WriteLine("6. Redo");
      Console.WriteLine("0. Exit");
      Console.Write("Choose: ");

      string? choice = Console.ReadLine();

      try {
        switch (choice) {
          case "1":
            if (manager.Tasks.Count == 0) {
              Console.WriteLine("No tasks.");
            } else {
              foreach (TaskItem task in manager.Tasks) {
                Console.WriteLine(task);
              }
            }
            break;
          case "2":
            Console.Write("Task name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) {
              Console.WriteLine("Name cannot be empty.");
            } else {
              manager.Add(name.Trim());
            }
            break;
          case "3":
            Console.Write("Task ID: ");
            if (int.TryParse(Console.ReadLine(), out int completeId)) {
              manager.Complete(completeId);
            } else {
              Console.WriteLine("Enter a valid number.");
            }
            break;
          case "4":
            Console.Write("Task ID: ");
            if (int.TryParse(Console.ReadLine(), out int removeId)) {
              manager.Remove(removeId);
            } else {
              Console.WriteLine("Enter a valid number.");
            }
            break;
          case "5":
            manager.Undo();
            break;
          case "6":
            manager.Redo();
            break;
          case "0":
            return;
          default:
            Console.WriteLine("Unknown menu option.");
            break;
        }
      } catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
  }
}
