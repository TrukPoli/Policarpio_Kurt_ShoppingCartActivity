using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine(Id + " - " + Name + " - ₱" + Price + " - Stock: " + RemainingStock);
    }

    public bool HasEnoughStock(int quantity)
    {
        return RemainingStock >= quantity;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock = RemainingStock - quantity;
    }
}

class CartItem
{
    public Product Product;
    public int Quantity;
    public double SubTotal;

    public void UpdateSubtotal()
    {
        SubTotal = Product.Price * Quantity;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Toyota Vios", Price = 900000, RemainingStock = 3 },
            new Product { Id = 2, Name = "Honda Civic", Price = 1200000, RemainingStock = 2 },
            new Product { Id = 3, Name = "Ford Ranger", Price = 1500000, RemainingStock = 2 },
            new Product { Id = 4, Name = "Mitsubishi Montero", Price = 1600000, RemainingStock = 2 },
            new Product { Id = 5, Name = "Nissan Navara", Price = 1400000, RemainingStock = 2 },
            new Product { Id = 6, Name = "Suzuki Swift", Price = 700000, RemainingStock = 4 },
            new Product { Id = 7, Name = "Hyundai Tucson", Price = 1300000, RemainingStock = 2 },
            new Product { Id = 8, Name = "Kia Sportage", Price = 1250000, RemainingStock = 2 }
        };

        CartItem[] cart = new CartItem[10];
        int cartCount = 0;

        while (true)
        {
            Console.WriteLine("\n=== CAR STORE MENU ===");

            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            int productId;
            int quantity;

            Console.Write("Enter car number: ");
            while (!int.TryParse(Console.ReadLine(), out productId))
            {
                Console.Write("Invalid input. Enter car number: ");
            }

            Console.Write("Enter quantity: ");
            while (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.Write("Invalid input. Enter quantity: ");
            }

            Product selectedProduct = null;

            for (int i = 0; i < products.Length; i++)
            {
                if (products[i].Id == productId)
                {
                    selectedProduct = products[i];
                    break;
                }
            }

            if (selectedProduct == null)
            {
                Console.WriteLine("Invalid car number.");
                continue;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selectedProduct.HasEnoughStock(quantity))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            CartItem existingItem = null;

            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Product.Id == selectedProduct.Id)
                {
                    existingItem = cart[i];
                    break;
                }
            }

            if (existingItem != null)
            {
                existingItem.Quantity = existingItem.Quantity + quantity;
                existingItem.UpdateSubtotal();
            }
            else
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                CartItem newItem = new CartItem();
                newItem.Product = selectedProduct;
                newItem.Quantity = quantity;
                newItem.UpdateSubtotal();

                cart[cartCount] = newItem;
                cartCount++;
            }

            selectedProduct.DeductStock(quantity);

            Console.Write("Add another car? (Y/N): ");
            string choice = Console.ReadLine().ToUpper();

            if (choice == "N")
            {
                break;
            }
        }
        
        Console.WriteLine("\n=== RECEIPT ===");

        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine(cart[i].Product.Name + " x" + cart[i].Quantity + " = ₱" + cart[i].SubTotal);
            grandTotal = grandTotal + cart[i].SubTotal;
        }

        Console.WriteLine("\nGrand Total: ₱" + grandTotal);

        if (grandTotal >= 5000000)
        {
            double discount = grandTotal * 0.10;
            grandTotal = grandTotal - discount;

            Console.WriteLine("Discount (10%): -₱" + discount);
        }

        Console.WriteLine("Final Total: ₱" + grandTotal);

        Console.WriteLine("\n=== UPDATED STOCK ===");

        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }
    }
}
