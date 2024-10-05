# Setting Up Final Project

cd ToDoList
dotnet new sln --name MojePrvniWebovaAplikace

## 📡 Creating ToDoList.WebApi project

```cmd
cd ToDoList
dotnet new web --name MojePrvniWebovaAplikace.WebApi --output src/MojePrvniWebovaAplikace.WebApi
```

## 📘 Creating ToDoList.Model project

```cmd
cd ToDoList
dotnet new classlib --name MojePrvniWebovaAplikace.Model --output src/MojePrvniWebovaAplikace.Model
```

## 🗃️ Creating ToDoList.Persistency project

```cmd
cd ToDoList
dotnet new classlib --name ToDoList.Persistency --output src/ToDoList.Persistency
```

## 🧪 Creating ToDoList.Test project

```cmd
cd ToDoList
dotnet new xunit --name ToDoList.Test --output tests/ToDoList.Test
```

## 🌐 Creating ToDoList.Frontend project

```cmd
cd ToDoList
dotnet new blazor --interactivity None --empty --name ToDoList.Frontend --output src/ToDoList.Frontend
```
