using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gradebook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public class NamedObject 
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name) {}

        public override void AddGrade(double grade)
        {
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);

                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            using(var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name) {}

        public abstract void AddGrade(double grade);
        public abstract event GradeAddedDelegate GradeAdded;
        public abstract Statistics GetStatistics();
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name {get;}
        event GradeAddedDelegate GradeAdded;
    }


    public class InMemoryBook : Book
    {
        private readonly List<double> _grades;

        public InMemoryBook(string name) : base(name)
        {
            _grades = new List<double>();
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for(var index = 0; index < _grades.Count; index++)
            {
                result.Add(_grades[index]);
            }

            return result;
        }

        public void AddGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;

                case 'B':
                    AddGrade(80);
                    break;

                case 'C':
                    AddGrade(70);
                    break;
                
                case 'D':
                    AddGrade(60);
                    break;
                
                case 'F':
                    AddGrade(50);
                    break;
                
                default:
                    Console.WriteLine("You entered an invalid grade");
                    break;
            }
        }

        public override void AddGrade(double grade) 
        {
            if (grade >= 0 && grade <= 100) 
            {
                _grades.Add(grade);

                // Check if there are any events
                if (GradeAdded != null)
                    // If there are event then invoke them.
                    GradeAdded(this, new EventArgs());
            }
            else 
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public double GetAverage()
        {
            return _grades.Average();
        }

        public override event GradeAddedDelegate GradeAdded;
    }
}
