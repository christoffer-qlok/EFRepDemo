using EFRepDemo.Data;
using EFRepDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFRepDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(TodoContext context = new TodoContext())
            {
                User user = UserMenu(context);
                TodoMenu(context, user);
            }
        }

        static User UserMenu(TodoContext context)
        {
            Console.Clear();

            // Hämta alla users
            List<User> users = context.Users.ToList();
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i}. {users[i].Name}");
            }
            Console.WriteLine("c to create new user");
            string input = Console.ReadLine();

            if (input == "c")
            {
                Console.Write("Enter user name:");
                string name = Console.ReadLine();
                User user = new User()
                {
                    Name = name
                };
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }

            int menuChoice = Convert.ToInt32(input);
            return users[menuChoice];
        }

        static void TodoMenu(TodoContext context, User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{user.Name}s todo list");

                // Hämta vår User och plocka ut dess TodoItems
                List<TodoItem> todoItems = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.TodoItems) 
                    .Single()
                    .TodoItems
                    .ToList();

                for (int i = 0; i < todoItems.Count; i++)
                {
                    Console.WriteLine($"{i}. {todoItems[i].Title}: {todoItems[i].Description}");
                }
                Console.WriteLine("c to create new todo item");
                Console.WriteLine("q to quit");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "c":
                        Console.Write("Enter title:");
                        string title = Console.ReadLine();
                        Console.Write("Enter description:");
                        string description = Console.ReadLine();
                        TodoItem newItem = new TodoItem()
                        {
                            Title = title,
                            Description = description,
                            User = user
                        };
                        context.TodoItems.Add(newItem);
                        context.SaveChanges();
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}