using System;
using System.Collections.Generic;
using System.IO;

// a. Student class
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Score { get; set; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100)
            return "A";

        if (Score >= 70)
            return "B";

        if (Score >= 60)
            return "C";

        if (Score >= 50)
            return "D";

        return "F";
    }
}

// b. Custom exception
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message)
        : base(message)
    {
    }
}

// c. Custom exception
public class MissingFieldException : Exception
{
    public MissingFieldException(string message)
        : base(message)
    {
    }
}

// d. StudentResultProcessor
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(
        string inputFilePath)
    {
        List<Student> students =
            new List<Student>();

        using (StreamReader reader =
               new StreamReader(inputFilePath))
        {
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = line.Split(',');

                // Check number of fields
                if (fields.Length != 3)
                {
                    throw new MissingFieldException(
                        $"Invalid record: {line}"
                    );
                }

                // Check for missing values
                if (string.IsNullOrWhiteSpace(fields[0]) ||
                    string.IsNullOrWhiteSpace(fields[1]) ||
                    string.IsNullOrWhiteSpace(fields[2]))
                {
                    throw new MissingFieldException(
                        $"A required field is missing: {line}"
                    );
                }

                int id;

                if (!int.TryParse(fields[0], out id))
                {
                    throw new InvalidScoreFormatException(
                        $"Invalid student ID: {fields[0]}"
                    );
                }

                int score;

                if (!int.TryParse(fields[2], out score))
                {
                    throw new InvalidScoreFormatException(
                        $"Invalid score: {fields[2]}"
                    );
                }

                if (score < 0 || score > 100)
                {
                    throw new InvalidScoreFormatException(
                        $"Score must be between 0 and 100: {score}"
                    );
                }

                Student student = new Student(
                    id,
                    fields[1].Trim(),
                    score
                );

                students.Add(student);
            }
        }

        return students;
    }

    public void WriteReportToFile(
        List<Student> students,
        string outputFilePath)
    {
        using (StreamWriter writer =
               new StreamWriter(outputFilePath))
        {
            foreach (Student student in students)
            {
                writer.WriteLine(
                    $"{student.FullName} " +
                    $"(ID: {student.Id}): " +
                    $"Score = {student.Score}, " +
                    $"Grade = {student.GetGrade()}"
                );
            }
        }
    }
}

// Main
public class Program
{
    public static void Main()
    {
        string inputFilePath = "students.txt";
        string outputFilePath = "student_report.txt";

        try
        {
            StudentResultProcessor processor =
                new StudentResultProcessor();

            List<Student> students =
                processor.ReadStudentsFromFile(
                    inputFilePath);

            processor.WriteReportToFile(
                students,
                outputFilePath);

            Console.WriteLine(
                "Student report generated successfully."
            );

            Console.WriteLine(
                $"Output file: {outputFilePath}"
            );
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(
                "Error: The input file was not found."
            );
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine(
                $"Invalid score format: {ex.Message}"
            );
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(
                $"Missing field: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}"
            );
        }
    }
}
