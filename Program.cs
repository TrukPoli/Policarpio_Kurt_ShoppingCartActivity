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
            new Product { Id = 3, Name = "Ford Ranger", Price = 1500000, RemainingStock = 2 }
        };

        CartItem[] cart = new CartItem[10];
        int cartCount = 0;

        while (true)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View Cart");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Checkout");

            int choice;
            Console.Write("Enter choice: ");
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Write("Invalid input. Enter choice: ");
            }

            if (choice == 1)
            {
                Console.WriteLine("\n=== PRODUCTS ===");
                for (int i = 0; i < products.Length; i++)
                {
                    products[i].DisplayProduct();
                }

                int productId, quantity;

                Console.Write("Enter product number: ");
                int.TryParse(Console.ReadLine(), out productId);

                Console.Write("Enter quantity: ");
                int.TryParse(Console.ReadLine(), out quantity);

                Product selectedProduct = null;

                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i].Id == productId)
                    {
                        selectedProduct = products[i];
                        break;
                    }
                }

                if (selectedProduct == null || quantity <= 0)
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                CartItem item = new CartItem();
                item.Product = selectedProduct;
                item.Quantity = quantity;
                item.UpdateSubtotal();

                cart[cartCount] = item;
                cartCount++;

                Console.WriteLine("Item added to cart.");
            }

            else if (choice == 2)
            {
                Console.WriteLine("\n=== CART ===");

                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine((i + 1) + ". " + cart[i].Product.Name + " x" + cart[i].Quantity);
                }
            }

            else if (choice == 3)
            {
                Console.Write("Enter item number to remove: ");
                int index;
                int.TryParse(Console.ReadLine(), out index);

                if (index > 0 && index <= cartCount)
                {
                    for (int i = index - 1; i < cartCount - 1; i++)
                    {
                        cart[i] = cart[i + 1];
                    }

                    cartCount--;
                    Console.WriteLine("Item removed.");
                }
                else
                {
                    Console.WriteLine("Invalid item.");
                }
            }

            else if (choice == 4)
            {
                cartCount = 0;
                Console.WriteLine("Cart cleared.");
            }

            else if (choice == 5)
            {
                Console.WriteLine("Proceeding to checkout...");
                break;
            }
        }
    }
}
