using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// a/b. Inventory record and interface
public interface IInventoryEntity
{
    int Id { get; }
}

public record InventoryItem(
    int Id,
    string Name,
    int Quantity,
    DateTime DateAdded
) : IInventoryEntity;

// c. Generic Inventory Logger
public class InventoryLogger<T>
    where T : IInventoryEntity
{
    private List<T> _log = new List<T>();

    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_log);
    }

    // Save data
    public void SaveToFile()
    {
        try
        {
            string json =
                JsonSerializer.Serialize(
                    _log,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            using (StreamWriter writer =
                   new StreamWriter(_filePath))
            {
                writer.Write(json);
            }

            Console.WriteLine(
                "Inventory data saved successfully."
            );
        }
        catch (IOException ex)
        {
            Console.WriteLine(
                $"File error while saving: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}"
            );
        }
    }

    // Load data
    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine(
                    "Inventory file does not exist."
                );

                return;
            }

            using (StreamReader reader =
                   new StreamReader(_filePath))
            {
                string json = reader.ReadToEnd();

                List<T>? loadedItems =
                    JsonSerializer.Deserialize<List<T>>(json);

                if (loadedItems != null)
                {
                    _log = loadedItems;
                }
            }

            Console.WriteLine(
                "Inventory data loaded successfully."
            );
        }
        catch (IOException ex)
        {
            Console.WriteLine(
                $"File error while loading: {ex.Message}"
            );
        }
        catch (JsonException ex)
        {
            Console.WriteLine(
                $"Invalid JSON data: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}"
            );
        }
    }

    public void Clear()
    {
        _log.Clear();
    }
}

// f. InventoryApp
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    private string _filePath = "inventory.json";

    public InventoryApp()
    {
        _logger =
            new InventoryLogger<InventoryItem>(_filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(
            new InventoryItem(
                1,
                "Laptop",
                10,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                2,
                "Keyboard",
                25,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                3,
                "Mouse",
                30,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                4,
                "Monitor",
                15,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                5,
                "Printer",
                8,
                DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        Console.WriteLine("\n=== INVENTORY ITEMS ===");

        List<InventoryItem> items =
            _logger.GetAll();

        foreach (InventoryItem item in items)
        {
            Console.WriteLine(
                $"ID: {item.Id}, " +
                $"Name: {item.Name}, " +
                $"Quantity: {item.Quantity}, " +
                $"Date Added: {item.DateAdded:g}"
            );
        }
    }
}

// Main
public class Program
{
    public static void Main()
    {
        // First session
        InventoryApp app =
            new InventoryApp();

        app.SeedSampleData();

        app.SaveData();

        Console.WriteLine(
            "\nSimulating a new session..."
        );

        // Clear reference to simulate closing application
        app = null!;

        // New session
        InventoryApp newApp =
            new InventoryApp();

        newApp.LoadData();

        newApp.PrintAllItems();
    }
}
