//Initial setup with Product class and menu display
using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id} - {Name} - ₱{Price} - Stock: {RemainingStock}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // PRODUCT LIST (INITIAL SETUP ONLY)
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Rice", Price = 50, RemainingStock = 10 },
            new Product { Id = 2, Name = "Milk", Price = 80, RemainingStock = 8 },
            new Product { Id = 3, Name = "Bread", Price = 30, RemainingStock = 15 }
        };

        // DISPLAY MENU
        Console.WriteLine("=== STORE MENU ===");

        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }

        Console.WriteLine("\nProgram End");
    }
}
