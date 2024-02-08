using Microsoft.EntityFrameworkCore;
using DAL.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DAL.Data
{
    public enum SortCategory
    {
        Date,
        Type,
        Name,
        Price
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base("Utilisateur non trouvé")
        {
        }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base("Produit non trouvé")
        {
        }
    }

    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string message) : base("Panier non trouvé")
        {
        }
    }

    public class IbayContext : DbContext, IIbayContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration["DbConnection:connectionString"];
                
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
                
            optionsBuilder.UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        

        public User CreateUser(string userPseudo, string userEmail, string userPassword)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPassword, salt);

            var newUser = new User()
            {
                user_pseudo = userPseudo,
                user_email = userEmail,
                user_password = hashedPassword,
                user_money = 0,
                user_role = UserRole.StandardUser,
                updated_date = null,
                creation_date = DateTime.Now,
            };

            var errorMessage = newUser.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Users.Add(newUser);
            SaveChanges();
            return newUser;
        }

        public IEnumerable<User> GetUsers()
        {
            return Users.ToList();
        }

        public User GetUserById(int user_id)
        {
            return Users.SingleOrDefault(u => u.user_id == user_id)!;
        }
        
        public User GetUserByEmail(string userEmail)
        {
            return Users.SingleOrDefault(u => u.user_email == userEmail)!;
        }
        
        public User GetUserByPseudo(string userPseudo)
        {
            return Users.SingleOrDefault(u => u.user_pseudo == userPseudo)!;
        }

        public User GetUserCart(int user_id)
        {
            return Users.SingleOrDefault(u => u.user_id == user_id)!;
        }

        public User UpdateUser(int user_id, string userEmail, string userPseudo, string userPassword)
        {
<<<<<<< Updated upstream
            var userToUpdate = Users.FirstOrDefault(u => u.UserId == userId);
=======
            var userToUpdate = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToUpdate == null)
            {
                return null!;
            }

            if (!userEmail.IsNullOrEmpty())
            {
                userToUpdate.user_email = userEmail;
            }
            if (!userPseudo.IsNullOrEmpty())
            {
                userToUpdate.user_pseudo = userPseudo;
            }
            if (!userPassword.IsNullOrEmpty())
            {
                userToUpdate.user_password = userPassword;
            }
            
            userToUpdate.updated_date = DateTime.Now;

            var errorMessage = userToUpdate.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            SaveChanges();
            return userToUpdate;
        }

        public User UpdateUserMoney(int user_id, double money)
        {
<<<<<<< Updated upstream
            var userToPutMoney = Users.FirstOrDefault(u => u.UserId == userId);
=======
            var userToPutMoney = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToPutMoney == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            userToPutMoney.user_money += money switch
            {
                0 => throw new System.ComponentModel.DataAnnotations.ValidationException(
                    "You can't put 0 in this method"),
                < 0 when userToPutMoney.user_money - money >= 0 => money,
                < 0 => throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money"),
                _ => money
            };

            if (userToPutMoney.user_money <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money");
            }

            SaveChanges();

            return userToPutMoney;
        }

        public User DeleteUser(int user_id)
        {
<<<<<<< Updated upstream
            var userToDelete = Users.SingleOrDefault(u => u.UserId == userId);
=======
            var userToDelete = Users.SingleOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToDelete == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            Users.Remove(userToDelete);
            SaveChanges();

            return userToDelete;
        }

        public User ChangeUserRole(int user_id, UserRole role)
        {
<<<<<<< Updated upstream
            var userToChangeRole = Users.FirstOrDefault(u => u.UserId == userId);
=======
            var userToChangeRole = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToChangeRole == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new ArgumentException("Invalid user role");
            }

            userToChangeRole.user_role = role;

            SaveChanges();

            return userToChangeRole;
        }

<<<<<<< Updated upstream

        // PRODUCT


        public Product CreateProduct(int sellerId, string productName, string productDescription, ProductType productType, double productPrice, int productStock)
        {
            
            var seller = Users.FirstOrDefault(u => u.UserId == sellerId);
=======
        public Product CreateProduct(int sellerId, string product_name, string product_description,
            ProductType product_type, double product_price, int product_stock)
        {
            
            var seller = Users.FirstOrDefault(u => u.user_id == sellerId);
            
>>>>>>> Stashed changes
            if (seller == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Seller not found");
            }
            
            var newProduct = new Product()
            {
                product_name = product_name,
                product_description = product_description,
                product_type = product_type,
                product_price = product_price,
                product_stock = product_stock,
                added_time = DateTime.Now,
                updated_time = null,
                fk_user_id = sellerId
            };
            // Vérifier que le produit n'existe pas déjà
            Products.Add(newProduct);
            SaveChanges();
            return newProduct;
        }
        
<<<<<<< Updated upstream
        

        public Product GetProductById(int productId)
=======
        public Product GetProductById(int product_id)
>>>>>>> Stashed changes
        {
            return Products.SingleOrDefault(p => p.product_id == product_id)!;
        }

        public Product GetProductByName(string product_name)
        {
            return Products.SingleOrDefault(p => p.product_name == product_name)!;
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products.ToList();
        }
        

        public IEnumerable<Product> GetProductSortedBy(SortCategory sortCategory, int limit)
        {
            if (limit.GetType() != typeof(int))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Limit must be an integer");
            }
            IQueryable<Product> query = Products;
            query = sortCategory switch
            {
                SortCategory.Date => query.OrderByDescending(p => p.added_time),
                SortCategory.Type => query.OrderBy(p => p.product_type),
                SortCategory.Name => query.OrderBy(p => p.product_name),
                SortCategory.Price => query.OrderBy(p => p.product_price),
                _ => query.OrderByDescending(p => p.added_time)
            };

            return query.Take(limit).ToList();
        }

<<<<<<< Updated upstream
        public Product UpdateProduct(int productId, string productName, string productDescription, ProductType productType, double? productPrice, int? productStock, bool? available)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
=======
        public Product UpdateProduct(int product_id, string product_name, string product_description,
            ProductType product_type, double? product_price, int? product_stock, bool? available)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.product_id == product_id);
            
>>>>>>> Stashed changes
            if (productToUpdate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            if (!product_name.IsNullOrEmpty())
            {
                productToUpdate.product_name = product_name;
            }
<<<<<<< Updated upstream
            if (!productDescription.IsNullOrEmpty())
=======
            
            if (!product_description.IsNullOrEmpty())
>>>>>>> Stashed changes
            {
                productToUpdate.product_description = product_description;
            }
<<<<<<< Updated upstream
            if (productType != productToUpdate.ProductType)
=======
            
            if (product_type != productToUpdate.product_type)
>>>>>>> Stashed changes
            {
                productToUpdate.product_type = product_type;
            }
<<<<<<< Updated upstream
            if (productPrice != null)
=======
            
            if (product_price != null)
>>>>>>> Stashed changes
            {
                productToUpdate.product_price = product_price.Value;
            }
<<<<<<< Updated upstream
            if (productStock != null)
=======
            
            if (product_stock != null)
>>>>>>> Stashed changes
            {
                productToUpdate.product_stock = product_stock.Value;
            }
            if (available != null)
            {
                productToUpdate.available = available.Value;
            }
            
<<<<<<< Updated upstream
            productToUpdate.UpdatedTime = DateTime.Now;
=======
            productToUpdate.updated_time = DateTime.Now;
            
>>>>>>> Stashed changes
            SaveChanges();
            return productToUpdate;
        }
        
        public Product DeleteProduct(int product_id)
        {
<<<<<<< Updated upstream
            var productToDelete = Products.SingleOrDefault(p => p.ProductId == productId);
=======
            var productToDelete = Products.SingleOrDefault(p => p.product_id == product_id);
            
>>>>>>> Stashed changes
            if (productToDelete == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }
            Products.Remove(productToDelete);
            SaveChanges();
            return productToDelete;
        }
        
        


        // CART
        
        //TODO: Refaire les méthodes de cart

        // GetCart

        public IEnumerable<Cart> GetCart(int user_id)
        {
<<<<<<< Updated upstream
            var userToGetCart = Users.FirstOrDefault(u => u.UserId == userId);
=======
            var userToGetCart = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToGetCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

<<<<<<< Updated upstream
            var cart = Carts.Where(c => c.FK_UserId == userId).Include(c=>c.Product).ToList();
=======
            var cart = Carts.Where(c => c.fk_user_id == user_id).Include(c=>c.product)
                .ToList();
            
>>>>>>> Stashed changes
            if (cart.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }

            return cart;
        }

<<<<<<< Updated upstream

        // AddProductToCart
        // Un User n'a qu'un Cart, si le cart n'existe pas, on le crée
        // Dans Cart, on a une colonne pour les Produits et une colonne pour les quantités
        // Si un Product est déjà dans le Cart, on ajoute la quantité demandée à la quantité existante
        // Si un Product n'est pas dans le Cart, on l'ajoute avec la quantité demandée
        // User n'a pas de relation avec Cart, mais Cart a une relation avec User
        // Cart a une relation avec Product

        public void AddProductToCart(int userId, int productId, int quantity)
        {
            var userToAddProductToCart = Users.FirstOrDefault(u => u.UserId == userId);
=======
        public void AddProductToCart(int user_id, int product_id, int quantity)
        {
            var userToAddProductToCart = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToAddProductToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

<<<<<<< Updated upstream
            var productToAddToCart = Products.FirstOrDefault(p => p.ProductId == productId);
=======
            var productToAddToCart = Products.FirstOrDefault(p => p.product_id == product_id);
            
>>>>>>> Stashed changes
            if (productToAddToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }
            
            

            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

            if (productToAddToCart.product_stock <= quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough stock");
            }

<<<<<<< Updated upstream
            var existingCart = Carts.FirstOrDefault(c => c.FK_UserId == userId);
=======
            var existingCart = Carts.FirstOrDefault(c => c.fk_user_id == user_id);
            
>>>>>>> Stashed changes
            if(existingCart != null)
            {
                existingCart.quantity += quantity;
            }
            else
            {
                var newCart = new Cart()
                {
                    fk_product_id = product_id,
                    product = productToAddToCart,
                    fk_user_id = user_id,
                    user = userToAddProductToCart,
                    quantity = quantity
                };
                Console.WriteLine();
                Console.WriteLine("#################################################################################");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("newCart : " + newCart);
                Console.WriteLine("newCart Type : " + newCart.GetType());
                Console.WriteLine("newCart.Product : " + newCart.Product.ProductName);
                Console.WriteLine("newCart.User : " + newCart.User.UserPseudo);
                Console.WriteLine("newCart.Quantity : " + newCart.Quantity);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("#################################################################################");
                Console.WriteLine();
                Carts.Add(newCart);
                SaveChanges();
            }
        }
<<<<<<< Updated upstream

        // RemoveProductFromCart
        // Si le produit n'est pas dans le panier, erreur 404
        // Si la quantité demandée est supérieure à la quantité dans le panier, erreur 400
        // Si la quantité demandée est négative, erreur 400
        // Si la quantité demandée est égale à la quantité dans le panier, on retire le produit du panier
        // Si la quantité demandée est inférieure à la quantité dans le panier, on retire la quantité demandée de la quantité dans le panier
        public User RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var userToRemoveProductFromCart = Users.FirstOrDefault(u => u.UserId == userId);
=======
        
        public User RemoveProductFromCart(int user_id, int product_id, int quantity)
        {
            var userToRemoveProductFromCart = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToRemoveProductFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

<<<<<<< Updated upstream
            var productToRemoveFromCart = Products.FirstOrDefault(p => p.ProductId == productId);
=======
            var productToRemoveFromCart = Products.FirstOrDefault(p => p.product_id == product_id);
            
>>>>>>> Stashed changes
            if (productToRemoveFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

<<<<<<< Updated upstream
            var productInCart = Carts.FirstOrDefault(c => c.FK_UserId == userId && c.FK_ProductId == productId);
=======
            var productInCart = Carts.FirstOrDefault(c => c.fk_user_id == user_id && c.fk_product_id == product_id);
            
>>>>>>> Stashed changes
            if (productInCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not in cart");
            }

            if (quantity > productInCart.quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity in cart is less than quantity to remove");
            }

            if (quantity == productInCart.quantity)
            {
                Carts.Remove(productInCart);
                SaveChanges();
                return userToRemoveProductFromCart;
            }

<<<<<<< Updated upstream
            productInCart.Quantity -= quantity;
=======
            productInCart.quantity -= quantity;
            
>>>>>>> Stashed changes
            SaveChanges();
            return userToRemoveProductFromCart;
        }

<<<<<<< Updated upstream
        // BuyCart
        // Si le User n'existe pas, erreur 404
        // Si le panier est vide, erreur 400
        // On calcule le prix total du panier
        // Si le User n'a pas assez d'argent, erreur 400
        // On retire le prix total du panier de l'argent du User
        // On vide le panier
        public User BuyCart(int userId)
        {
            User userToBuyCart = Users.FirstOrDefault(u => u.UserId == userId);
=======
        public User BuyCart(int user_id)
        {
            var userToBuyCart = Users.FirstOrDefault(u => u.user_id == user_id);
            
>>>>>>> Stashed changes
            if (userToBuyCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }
<<<<<<< Updated upstream
            // On récupère le panier du User
            var cart = Carts.Where(c => c.FK_UserId == userId).ToList();
=======
            var cart = Carts.Where(c => c.fk_user_id == user_id).ToList();
            
>>>>>>> Stashed changes
            if (cart.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }
<<<<<<< Updated upstream
            // On calcule le prix total du panier
            double totalPrice = 0;
            foreach (var product in cart)
            {
                totalPrice += product.Product.ProductPrice * product.Quantity;
            }
            // Si le User n'a pas assez d'argent, erreur 400
            if (userToBuyCart.UserMoney < totalPrice)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money");
            }
            // On retire le prix total du panier de l'argent du User
            userToBuyCart.UserMoney -= totalPrice;
            // On vide le panier
=======
            
            var totalPrice = cart.Sum(product => product.product.product_price * product.quantity);

            if (userToBuyCart.user_money < totalPrice)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money");
            }
            
            userToBuyCart.user_money -= totalPrice;

>>>>>>> Stashed changes
            Carts.RemoveRange(cart);
            SaveChanges();
            return userToBuyCart;
        }

    }
}
