using System;


public class Task
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }

    public Task(int id, string name, string description, DateTime date)
    {
        ID = id;
        Name = name;
        Description = description;
        Date = date;
        Status = "To Do"; 
    }

    public override string ToString()
    {
        return $"Task {ID}: {Name} ({Status}) - Due: {Date:yyyy-MM-dd}";
    }
}

// Node class for linked list
public class Node
{
    public Task TaskData { get; set; }
    public Node Next { get; set; }

    public Node(Task task)
    {
        TaskData = task;
        Next = null;
    }
}

// To-Do List (maintains ascending order by date)
public class ToDoList
{
    private Node head;

    public ToDoList()
    {
        head = null;
    }

   
    public void InsertOrdered(Task task)
    {
        Node newNode = new Node(task);

        
        if (head == null || task.Date < head.TaskData.Date)
        {
            newNode.Next = head;
            head = newNode;
            return;
        }

        // Find the correct position to insert
        Node current = head;
        while (current.Next != null && current.Next.TaskData.Date <= task.Date)
        {
            current = current.Next;
        }

        newNode.Next = current.Next;
        current.Next = newNode;
    }

    // Remove and return task with given ID
    public Task RemoveTask(int taskId)
    {
        if (head == null)
            return null;

        
        if (head.TaskData.ID == taskId)
        {
            Task task = head.TaskData;
            head = head.Next;
            return task;
        }

        Node current = head;
        while (current.Next != null)
        {
            if (current.Next.TaskData.ID == taskId)
            {
                Task task = current.Next.TaskData;
                current.Next = current.Next.Next;
                return task;
            }
            current = current.Next;
        }

        return null;
    }

    public void Display()
    {
        if (head == null)
        {
            Console.WriteLine("To-Do list is empty.");
            return;
        }

        Console.WriteLine("To-Do Tasks (ordered by date):");
        Node current = head;
        while (current != null)
        {
            Console.WriteLine($"  {current.TaskData}");
            current = current.Next;
        }
    }
}

// In-Progress Stack (LIFO)
public class InProgressStack
{
    private Node head;

    public InProgressStack()
    {
        head = null;
    }

 
    public void Push(Task task)
    {
        Node newNode = new Node(task);
        newNode.Next = head;
        head = newNode;
    }

    
    public Task Pop()
    {
        if (head == null)
            return null;

        Task task = head.TaskData;
        head = head.Next;
        return task;
    }

    public void Display()
    {
        if (head == null)
        {
            Console.WriteLine("In-Progress list is empty.");
            return;
        }

        Console.WriteLine("In-Progress Tasks (stack - most recent first):");
        Node current = head;
        while (current != null)
        {
            Console.WriteLine($"  {current.TaskData}");
            current = current.Next;
        }
    }
}

// Completed Queue (FIFO)
public class CompletedQueue
{
    private Node head;
    private Node tail;

    public CompletedQueue()
    {
        head = null;
        tail = null;
    }

    // Add task to the end of the queue
    public void Enqueue(Task task)
    {
        Node newNode = new Node(task);

        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }
    }

    public void Display()
    {
        if (head == null)
        {
            Console.WriteLine("Completed list is empty.");
            return;
        }

        Console.WriteLine("Completed Tasks (queue - first completed first):");
        Node current = head;
        while (current != null)
        {
            Console.WriteLine($"  {current.TaskData}");
            current = current.Next;
        }
    }
}

// Main Task Manager
public class TaskManager
{
    private ToDoList todoList;
    private InProgressStack inProgressStack;
    private CompletedQueue completedQueue;

    public TaskManager()
    {
        todoList = new ToDoList();
        inProgressStack = new InProgressStack();
        completedQueue = new CompletedQueue();
    }

    // Add a new task to the to-do list
    public void AddTask(int id, string name, string description, DateTime date)
    {
        Task task = new Task(id, name, description, date);
        todoList.InsertOrdered(task);
        Console.WriteLine($"Added task: {task}");
    }

    // Move task from to-do to in-progress
    public void StartTask(int taskId)
    {
        Task task = todoList.RemoveTask(taskId);
        if (task != null)
        {
            task.Status = "In Progress";
            inProgressStack.Push(task);
            Console.WriteLine($"Started task: {task}");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found in to-do list.");
        }
    }

    // Complete the most recent in-progress task
    public void CompleteTask()
    {
        Task task = inProgressStack.Pop();
        if (task != null)
        {
            task.Status = "Completed";
            completedQueue.Enqueue(task);
            Console.WriteLine($"Completed task: {task}");
        }
        else
        {
            Console.WriteLine("No tasks in progress.");
        }
    }

    // Display all task lists
    public void DisplayAll()
    {
        Console.WriteLine("\n" + new string('=', 50));
        todoList.Display();
        Console.WriteLine();
        inProgressStack.Display();
        Console.WriteLine();
        completedQueue.Display();
        Console.WriteLine(new string('=', 50) + "\n");
    }
}

// Program class with Main method
public class Program
{
    public static void Main(string[] args)
    {
        // Create task manager
        TaskManager tm = new TaskManager();

        // Add some tasks with different dates
        tm.AddTask(1, "Design Database", "Create ER diagram and schema", new DateTime(2024, 6, 15));
        tm.AddTask(2, "Write API", "Implement REST API endpoints", new DateTime(2024, 6, 10));
        tm.AddTask(3, "Frontend UI", "Create user interface", new DateTime(2024, 6, 20));
        tm.AddTask(4, "Testing", "Unit and integration tests", new DateTime(2024, 6, 12));

        // Display initial state
        tm.DisplayAll();

        // Start some tasks (move to in-progress)
        tm.StartTask(2); // Write API
        tm.StartTask(4); // Testing
        tm.StartTask(1); // Design Database

        tm.DisplayAll();

       
        tm.CompleteTask()
        tm.CompleteTask(); 

        tm.DisplayAll();

        
        tm.StartTask(3); 

        
        tm.DisplayAll();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
