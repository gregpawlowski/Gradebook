# What happens with dotnet run
* Restore nuget packages if needed (dotnet restore is run)
* Build the project (dotnet build) - Creates obj/bin folders.
* * Source code is compiled to binary and creates DLL (Assembly) in bin.
* * Obj folder is created for temporary files during the build and restore.
* Executes the DLL using (dotnet src/Gradebook/bin/Debug/netcoreapp3.1/Gradebook.dll)

# Unit Testing
Unit testing is done in a seperate project.

`dotnet new xunit` 
xunit is a popular tool.

## Unit tests are normaly broken up into three sections

Arrange - put together test data and arrange objects and values.
Act - Where you actually invoke a method or do caclucation.
Assert - You assert something about the value that was computed.

## Naming Convention
Tests should be named appropriately BookTests.
If book is the class/type then the test will be the name Book.

# XUnit

## Fact attribure
Tests are decorated with Fact attributes

## Test runner
Visual studio has a test runner built in
Visual Studio Code has an extension .net core test explorer

dotnet cli also includes a test runner.

`dotnet test`


# Adding references in CLI
`dotnet add reference ../../src/Gradebook/`


# Value Types vs Reference Types
Classes are references

numbers are values

## Pass by value
Csharp is always pass by value, in the event of complex typs like classes the value is a reference to the memory location it's created. If passing a number then the value of the number is passed into the function parameter.


You can pass by reference using ref or out keywords. Now you would be able to pass the reference to the function instead of the value, since you have a reference to the variable you can change what the variable holds.

The difference between the ref and out keyword is that out will expect the paramter to be initialized. So when it's passed to the function the function has to initalize the parmeter or assignd to it, otherwise it will throw an error.

## How do you know if the variable is a primitive or class?
If anything is defined by a class you're working with a reference type.
If you are working with anything defined as struct, it is a value type.

Many structs have aliases / shortcuts:
int for example is Int32
double is Double
bool is Boolean

But not all
DateTime is a struct (vlaue type) but it doesnt have a alias

### Strings are special
Strings are special in that they are classes so they act as value types in many ways because strings are immuatable so there's no way to modify them when they are passed.

## Garbage collection
.NET has a garbage collector that keeps track of all objects and variables and field, it can clear up object that are not being utilized. This clears up memory so that objects are cleared up when they are no longer used.



# Building Types
Classes allow us to define new reference types. Methods and fields are members of the class.

## Method Overloading
You can add different different methods with the same name but different parameters. C# looks at the method name but also the paramaters, it looks at the "method signature". Return type is not part of the method signature. The return types can't be different on method overloading.

## Defining properties
Properties encapsulate state and store data but they are more powerful than fields.

Properties have setters and getters.

Gives you more control
```C#

        // Declared property Longhand
        private string _name;

        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
```

## Auto Property
If you make this it'll create a private field for you no need to add the _name property.
```C#
        public string Name 
        {
            get; set;
        }
```

This is similar to a field but properties behave a little different due to reflection and serialication.

You can also change property access on each property
```C#
        public string Name 
        {
            get; 
            private set;
        }
```

## const and readonly
readonly can only be assigned in the constructor or variable initialization

const can only be assigned durign the initalization.

const fields are treated like static members of the class so no need to crate an object.


##  Delegate
Delegate decribes and builds a new type but it's different than Classes or Structs. 
You just define a method signature.

```C#
        [Fact] void WriteLogDelegatePointToMethod()
        {
            WriteLogDelegate log;

            log = new WriteLogDelegate(ReturnMessage);

            var result = log("Hello");
            Assert.Equal("Hello", result);
        }

        string ReturnMessage(string message)
        {
            return message;
        }
```

Another way:
```C#
        [Fact] void WriteLogDelegatePointToMethod()
        {
            WriteLogDelegate log;
            
            // WE don't need to use the new WriteLogDelegate synatx.
            log = ReturnMessage;

            var result = log("Hello");
            Assert.Equal("Hello", result);
        }

        string ReturnMessage(string message)
        {
            return message;
        }
```

## Delegates can invoke multipe methods.
Delegates are multicast.

In the above example we use += to add different methods to the log delegate. Then we can invoke them all at once.
```C#
        int count = 0;

        [Fact] void WriteLogDelegatePointToMethod()
        {
            WriteLogDelegate log = ReturnMessage;

            log += ReturnMessage;
            log += IncrementCount;

            var result = log("Hello");
            System.Console.WriteLine(result);
            Assert.Equal(3, count);
        }

        
        string IncrementCount(string message)
        {
            count ++;
            return message.ToLower();
        }

        string ReturnMessage(string message)
        {
            count ++;
            return message;
        }
```

## Events
Events can be added to a class.
They have been around since the beginning but are not used as much and they are difficult to understand.

Sometimes you want to know when something happens.

This is the Type:
```C#
        // Delegate convention for Events.
        public delegate void GradeAddedDelegate(object sender, EventArgs args);
```

Now in the Gradebook class we can add a field with the event keyword
The event keyword add some additional restrictions and makes it safer to use.
```C#
public event GradeAddedDelegate GradeAdded;
```

Now in the location you want to invoke the delegate
```C#
                // Check if there are any events
                if (GradeAdded != null)
                    // If there are event then invoke them.
                    GradeAdded(this, new EventArgs());
```

# OOP

## The Pillars of OOP
Encapsulation - Allows to hide details, methods and properties help you with this.
Inheritance - Ability to re-use code across similar classes
Polymorthism - You can have objects of the same type that behave differently.


## Inheritance

### Deriving from a base class
Book is a NamedOBject
```C#
    public class Book : NamedObject
```

### Chaning constructors
Base refers to the derived class.

```C#
    public class NamedObject 
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
    public class Book : NamedObject
    {
        // Delegate convention for Events.
        private readonly List<double> _grades;

        public Book(string name) : base(name)
        {
            _grades = new List<double>();
        }
```

### Deriving from System.Object
Everything that doesn't derive from some base class derives from System.Object.

So if you put 
public class NamedObject

Thats similar to
```C# 
Public class NamedObject : System.Object
```
or 
```C#
public class NamedObject : object
```

## Polymorhphism
An object or material that takes various forms.
Allows you to write generic methods without a specifc implementation.

### Abstract class
Like a regualr class but cannot be instanciated, only derived from.

Abstract methods have to be overridden to implement

```C#
    public abstract class BookBase : NamedObject
    {
        protected BookBase(string name) : base(name) {}

        public abstract void AddGrade(double grade);
    }


    public class Book : BookBase
    {
```

### Interface
Interface implements no implementation details on describes the members.

Interfaces should start with I

Interfaces are more common than abstarct classes

### virtual keyword
vierual keyword says soemeone may choose ort override,
abstract means you ahve to override.

## Writing to a file
Only open the file onces and keep writing to it

```C#
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
            }
```


use IDisable to dispose of the object. Will immediately clean the object up and close the file. you can use writer.Dispose() to close the file 
but it's better to use a using statement becuae in case of an exception using will still displose of it and realease the resource.


# Latest C#
C# added to help prevent Null Pointer Exception at runtime

Compiler can aggresively look through code and find where we might have an exception.

Reference types will be none nullable by default

Can be turned off in csproj.

```xml
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    // Verion of language by default it wil be newest
    <LangVersion>8.0</LangVerion>
    // Turn off and on Nullable context
    <NullableContextOptions>enable</NullableContextOptions>
  </PropertyGroup>
```

Can also use ? to say it will be a nullable type

```C#
Book? book = null;
```


# Creating a Solution
When you have mutliple projects and want to build them all and test everything etc you can add a solution file.

Solution keeps track of multiple projects. It allows you load all projects at once, or use cli to build all projects in the solution etc.

`dotnet new sln`

Add projects:
dotnet sln add ./src/Gradebook/
dotnet sln add ./test/Gradebook.Tests/

Now in the main directory you can just build everything together.
`dotnet build`

or find all test
`dotnet test`


# Next Pluralsight Courses for C#
Generics
Asynchrounous C# 5.0
LINQ Fundementals
Effective C# - C# Programming Paradigms