using System;
using System.Collections.Generic;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;
    public string Category;

    public void DisplayProduct()
    {
        string status = RemainingStock == 0 ? "OUT OF STOCK" : $"₱{Price:F2} - Stock: {RemainingStock} [{Category}]";
        Console.WriteLine($"{Id}. {Name} - {status}");
    }

    public bool HasEnoughStock(int quantity) => RemainingStock >= quantity;
    public void DeductStock(int quantity) => RemainingStock -= quantity;
    public void Restock(int quantity) => RemainingStock += quantity;
}

class CartItem
{
    public Product Product;
    public int Quantity;
    public double SubTotal;
    public void UpdateSubtotal() => SubTotal = Product.Price * Quantity;
}

class Order
{
    public string ReceiptNumber;
    public DateTime OrderDate;
    public List<CartItem> Items;
    public double FinalTotal;
}

class Program
{
    static List<Order> orderHistory = new List<Order>();
    static int receiptCounter = 1;

    static void Main(string[] args)
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Toyota Vios", Price = 900000, RemainingStock = 9, Category = "Sedan" },
            new Product { Id = 2, Name = "Honda Civic", Price = 1200000, RemainingStock = 6, Category = "Sedan" },
            new Product { Id = 3, Name = "Ford Ranger", Price = 1500000, RemainingStock = 6, Category = "Pickup" },
            new Product { Id = 4, Name = "Mitsubishi Montero", Price = 1600000, RemainingStock = 7, Category = "SUV" },
            new Product { Id = 5, Name = "Nissan Navara", Price = 1400000, RemainingStock = 10, Category = "Pickup" }
        };

        List<CartItem> cart = new List<CartItem>();

        while (true)
        {
            Console.WriteLine("\n=== CAR STORE MENU ===");
            Console.WriteLine("1. Shop/Search Cars");
            Console.WriteLine("2. View/Manage Cart");
            Console.WriteLine("3. Checkout");
            Console.WriteLine("4. View Order History");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            if (choice == "1") ShoppingMenu(products, cart);
            else if (choice == "2") ManageCart(cart);
            else if (choice == "3") Checkout(cart, products);
            else if (choice == "4") ViewHistory();
            else if (choice == "5") break;
            else Console.WriteLine("Invalid selection. Please choose from 1-5.");
        }
    }

    static void ShoppingMenu(Product[] products, List<CartItem> cart)
    {
        bool isShopping = true;
        while (isShopping)
        {
            Console.Write("\nEnter car name to search (or press Enter to see all): ");
            string search = Console.ReadLine().ToLower();

            Console.WriteLine("\n--- SEARCH RESULTS ---");
            bool found = false;
            foreach (var p in products)
            {
                if (p.Name.ToLower().Contains(search))
                {
                    p.DisplayProduct();
                    found = true;
                }
            }
            if (!found) Console.WriteLine("No cars matched your search.");

            Console.Write("\nEnter Car ID to add to cart (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out int id) && id != 0)
            {
                Product selected = Array.Find(products, p => p.Id == id);
                if (selected != null && selected.RemainingStock > 0)
                {
                    Console.Write("Enter quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0 && selected.HasEnoughStock(qty))
                    {
                        var existing = cart.Find(c => c.Product.Id == id);
                        if (existing != null) existing.Quantity += qty;
                        else cart.Add(new CartItem { Product = selected, Quantity = qty });

                        selected.DeductStock(qty);
                        Console.WriteLine($"Added {qty} {selected.Name}(s) to cart.");
                    }
                    else Console.WriteLine("Invalid quantity or not enough stock.");
                }
                else Console.WriteLine("Car not found or out of stock.");
            }

            while (true)
            {
                Console.Write("\nAdd another item? (Y/N): ");
                string repeat = Console.ReadLine().ToUpper();

                if (repeat == "Y") break;
                if (repeat == "N")
                {
                    isShopping = false;
                    break;
                }
                Console.WriteLine("Invalid input. Please enter Y or N only.");
            }
        }
    }

    static void ManageCart(List<CartItem> cart)
    {
        if (cart.Count == 0) { Console.WriteLine("\nYour cart is empty."); return; }

        Console.WriteLine("\n--- YOUR CART ---");
        for (int i = 0; i < cart.Count; i++)
        {
            cart[i].UpdateSubtotal();
            Console.WriteLine($"{i + 1}. {cart[i].Product.Name} x{cart[i].Quantity} - ₱{cart[i].SubTotal:F2}");
        }

        Console.WriteLine("\nOptions: [R]emove Item, [C]lear Cart, [B]ack");
        string opt = Console.ReadLine().ToUpper();

        if (opt == "R")
        {
            Console.Write("Enter item number to remove: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= cart.Count)
            {
                cart[idx - 1].Product.Restock(cart[idx - 1].Quantity);
                cart.RemoveAt(idx - 1);
                Console.WriteLine("Item removed and stock returned.");
            }
            else Console.WriteLine("Invalid selection.");

            Console.ReadKey();
        }
        else if (opt == "C")
        {
            foreach (var item in cart) item.Product.Restock(item.Quantity);
            cart.Clear();
            Console.WriteLine("Cart cleared.");
            Console.ReadKey();
        }
    }

    static void Checkout(List<CartItem> cart, Product[] products)
    {
        if (cart.Count == 0) { Console.WriteLine("Cart is empty."); return; }

        double total = 0;
        foreach (var item in cart) total += item.Product.Price * item.Quantity;

        double discount = total > 5000 ? total * 0.10 : 0;
        double finalTotal = total - discount;

        Console.WriteLine($"\nGrand Total: ₱{total:F2}");
        Console.WriteLine($"Discount: -₱{discount:F2}");
        Console.WriteLine($"Final Total: ₱{finalTotal:F2}");

        double payment = 0;
        while (payment < finalTotal)
        {
            Console.Write("Enter payment: ");
            if (!double.TryParse(Console.ReadLine(), out payment) || payment < finalTotal)
                Console.WriteLine("Insufficient payment!: Please try again.");
        }

        string rNo = receiptCounter.ToString("D4");

        Console.WriteLine("\n========================================");
        Console.WriteLine("           OFFICIAL RECEIPT             ");
        Console.WriteLine($"Receipt No: {rNo}");
        Console.WriteLine($"Date: {DateTime.Now:MMMM dd, yyyy h:mm tt}");
        Console.WriteLine("----------------------------------------");

        foreach (var item in cart)
        {
            double subtotal = item.Product.Price * item.Quantity;
            Console.WriteLine($"{item.Product.Name} x{item.Quantity} @ ₱{item.Product.Price:F2} = ₱{subtotal:F2}");
        }

        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Final Total: ₱{finalTotal:F2}");
        Console.WriteLine($"Payment:     ₱{payment:F2}");
        Console.WriteLine($"Change:      ₱{(payment - finalTotal):F2}");
        Console.WriteLine("========================================");

        Console.WriteLine("\nThank you for purchasing from our Car Store!");
        Console.WriteLine("We appreciate your trust and support ");
        Console.WriteLine("Please come again!\n");

        orderHistory.Add(new Order
        {
            ReceiptNumber = rNo,
            OrderDate = DateTime.Now,
            Items = new List<CartItem>(cart),
            FinalTotal = finalTotal
        });

        receiptCounter++;
        cart.Clear();

        Console.WriteLine("\n--- INVENTORY STATUS ---");
        foreach (var p in products)
        {
            if (p.RemainingStock <= 5)
                Console.WriteLine($"LOW STOCK ALERT: {p.Name} has only {p.RemainingStock} left!");
        }
    }

    static void ViewHistory()
    {
        Console.WriteLine("\n=== COMPLETED TRANSACTIONS ===");
        if (orderHistory.Count == 0) Console.WriteLine("No history found.");

        foreach (var o in orderHistory)
        {
            Console.WriteLine($"Receipt #{o.ReceiptNumber} - {o.OrderDate:g} - ₱{o.FinalTotal:F2}");
        }
    }
}
