using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        if (RemainingStock == 0)
        {
            Console.WriteLine(Id + " - " + Name + " - OUT OF STOCK");
        }
        else
        {
            Console.WriteLine(Id + " - " + Name + " - ₱" + Price + " - Stock: " + RemainingStock);
        }
    }

    public bool HasEnoughStock(int quantity)
    {
        return RemainingStock >= quantity;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
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
            new Product { Id = 1, Name = "Toyota Vios", Price = 900,000, RemainingStock = 3 },
            new Product { Id = 2, Name = "Honda Civic", Price = 1,200,000, RemainingStock = 2 },
            new Product { Id = 3, Name = "Ford Ranger", Price = 1,500,000, RemainingStock = 2 },
            new Product { Id = 4, Name = "Mitsubishi Montero", Price = 1,600,000, RemainingStock = 2 },
            new Product { Id = 5, Name = "Nissan Navara", Price = 1,400,000, RemainingStock = 2 },
            new Product { Id = 6, Name = "Suzuki Swift", Price = 700,000, RemainingStock = 4 },
            new Product { Id = 7, Name = "Hyundai Tucson", Price = 1,300,000, RemainingStock = 2 },
            new Product { Id = 8, Name = "Kia Sportage", Price = 1,250,000, RemainingStock = 2 }
        };

        CartItem[] cart = new CartItem[10];
        int cartCount = 0;

        while (true)
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View Cart");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Checkout");

            int menuChoice;
            Console.Write("Enter choice: ");
            while (!int.TryParse(Console.ReadLine(), out menuChoice))
            {
                Console.Write("Invalid input. Enter choice: ");
            }

            // ADD ITEM
            if (menuChoice == 1)
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

                if (selectedProduct.RemainingStock == 0)
                {
                    Console.WriteLine("This product is out of stock.");
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
                    existingItem.Quantity += quantity;
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
                Console.WriteLine("Item added to cart.");
            }

            // VIEW CART
            else if (menuChoice == 2)
            {
                Console.WriteLine("\n=== CART ===");

                if (cartCount == 0)
                {
                    Console.WriteLine("Cart is empty.");
                    continue;
                }

                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine((i + 1) + ". " + cart[i].Product.Name + " x" + cart[i].Quantity);
                }
            }

            // REMOVE ITEM
            else if (menuChoice == 3)
            {
                Console.Write("Enter item number to remove: ");
                int index;
                int.TryParse(Console.ReadLine(), out index);

                if (index > 0 && index <= cartCount)
                {
                    // RETURN STOCK BEFORE REMOVING
                    cart[index - 1].Product.RemainingStock += cart[index - 1].Quantity;

                    // SHIFT ITEMS
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
            
            // CLEAR CART
            else if (menuChoice == 4)
            {
                // RETURN ALL STOCK BEFORE CLEARING
                for (int i = 0; i < cartCount; i++)
                {
                    cart[i].Product.RemainingStock += cart[i].Quantity;
                }

                cartCount = 0;
                Console.WriteLine("Cart cleared.");
            }

            // CHECKOUT
            else if (menuChoice == 5)
            {
                Console.WriteLine("\n=== RECEIPT ===");

                double grandTotal = 0;

                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine(cart[i].Product.Name + " x" + cart[i].Quantity + " = ₱" + cart[i].SubTotal);
                    grandTotal += cart[i].SubTotal;
                }

                Console.WriteLine("\nGrand Total: ₱" + grandTotal);

                if (grandTotal >= 5000)
                {
                    double discount = grandTotal * 0.10;
                    grandTotal -= discount;
                    Console.WriteLine("Discount (10%): -₱" + discount);
                }

                Console.WriteLine("Final Total: ₱" + grandTotal);
                break;
            }
        }
    }
}
