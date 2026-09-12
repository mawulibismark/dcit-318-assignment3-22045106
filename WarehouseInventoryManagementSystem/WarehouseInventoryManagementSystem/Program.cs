using System;
using System.Collections.Generic;

// a. Marker interface
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// b. ElectronicItem
public class ElectronicItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }

    public ElectronicItem(
        int id,
        string name,
        int quantity,
        string brand,
        int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

// c. GroceryItem
public class GroceryItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }

    public GroceryItem(
        int id,
        string name,
        int quantity,
        DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}

// e. Custom exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message)
        : base(message)
    {
    }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message)
        : base(message)
    {
    }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message)
        : base(message)
    {
    }
}

// d. Generic inventory repository
public class InventoryRepository<T>
    where T : IInventoryItem
{
    private Dictionary<int, T> _items =
        new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateItemException(
                $"Item with ID {item.Id} already exists."
            );
        }

        _items.Add(item.Id, item);
    }

    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException(
                $"Item with ID {id} was not found."
            );
        }

        return _items[id];
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException(
                $"Item with ID {id} was not found."
            );
        }

        _items.Remove(id);
    }

    public List<T> GetAllItems()
    {
        return new List<T>(_items.Values);
    }

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new InvalidQuantityException(
                "Quantity cannot be negative."
            );
        }

        T item = GetItemById(id);
        item.Quantity = newQuantity;
    }
}

// f. Warehouse Manager
public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics =
        new InventoryRepository<ElectronicItem>();

    private InventoryRepository<GroceryItem> _groceries =
        new InventoryRepository<GroceryItem>();

    public void SeedData()
    {
        // Electronics
        _electronics.AddItem(
            new ElectronicItem(
                1, "Laptop", 10, "Dell", 24));

        _electronics.AddItem(
            new ElectronicItem(
                2, "Smartphone", 15, "Samsung", 12));

        _electronics.AddItem(
            new ElectronicItem(
                3, "Television", 5, "LG", 18));

        // Groceries
        _groceries.AddItem(
            new GroceryItem(
                101,
                "Rice",
                50,
                new DateTime(2027, 5, 20)));

        _groceries.AddItem(
            new GroceryItem(
                102,
                "Milk",
                30,
                new DateTime(2026, 10, 15)));

        _groceries.AddItem(
            new GroceryItem(
                103,
                "Bread",
                25,
                new DateTime(2026, 9, 20)));
    }

    public void PrintAllItems<T>(
        InventoryRepository<T> repo)
        where T : IInventoryItem
    {
        foreach (T item in repo.GetAllItems())
        {
            Console.WriteLine(
                $"ID: {item.Id}, " +
                $"Name: {item.Name}, " +
                $"Quantity: {item.Quantity}"
            );
        }
    }

    public void IncreaseStock<T>(
        InventoryRepository<T> repo,
        int id,
        int quantity)
        where T : IInventoryItem
    {
        try
        {
            T item = repo.GetItemById(id);

            int newQuantity = item.Quantity + quantity;

            repo.UpdateQuantity(id, newQuantity);

            Console.WriteLine(
                $"Stock increased. New quantity: {newQuantity}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(
        InventoryRepository<T> repo,
        int id)
        where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);

            Console.WriteLine(
                $"Item {id} removed successfully."
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void TestExceptions()
    {
        Console.WriteLine("\n=== TESTING EXCEPTIONS ===");

        // Duplicate item
        try
        {
            _electronics.AddItem(
                new ElectronicItem(
                    1, "Another Laptop", 5, "HP", 12));
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"Duplicate Error: {ex.Message}");
        }

        // Non-existent item
        try
        {
            _electronics.RemoveItem(999);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Not Found Error: {ex.Message}");
        }

        // Invalid quantity
        try
        {
            _groceries.UpdateQuantity(101, -10);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"Quantity Error: {ex.Message}");
        }
    }

    public void Run()
    {
        SeedData();

        Console.WriteLine("=== GROCERY INVENTORY ===");
        PrintAllItems(_groceries);

        Console.WriteLine("\n=== ELECTRONIC INVENTORY ===");
        PrintAllItems(_electronics);

        TestExceptions();
    }
}

// Main
public class Program
{
    public static void Main()
    {
        WareHouseManager manager =
            new WareHouseManager();

        manager.Run();
    }
}
