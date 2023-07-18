# Welcome to my client-server web application!

## In this readme file, I provide you with some information about the backend side of my application.

I hope it will help you truly appreciate the executed work. I really invest a piece of yourself in this project, and I hope you will really enjoy it. :) So, let's start with an introduction!

I've decided to develop a web tamagotchi application (so it was a good task from the Innowise team) where you can store and manage your virtual pets. During the process of completing this task, I've improved my knowledge in .NET and MVC technologies. Let's talk a little bit about the technology stack I used in my application.

## Technology Stack

So, the project has been written on .NET 6 using the ASP.NET Core Web API project type.

![Project Type](./Images/api.jpg)

The communication is simple and is implemented using HTTP calls, following REST API principles.

I used MS SQL Server to store data in the database and Entity Framework as the ORM. I used the code-first approach. With the ApplicationContext class, I set up relationships between tables in the database.

So I used Repository pattern for my repository. I have a list of "Entity repository" like Farm repository, Pet repository and so on. And also I have repository Manager for managing all repositories:
```csharp
public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationContext _db;
        private IUserRepository _userRepository;
        private IFarmRepository _farmRepository;
        private IPetRepository _petRepository;
        private IPetFeedingRepository _petFeedingRepository;
        private IPetDrinkingRepository _petDrinkingRepository;
        public RepositoryManager(ApplicationContext db)
        {
            _db = db;
        }

        public IUserRepository User {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_db);
                }
                return _userRepository;
            }
        
        }
        //...Other getters
    }
```

##