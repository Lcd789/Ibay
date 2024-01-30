using Microsoft.EntityFrameworkCore;
using DAL.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DAL.Data
{
    public class IbayContext : DbContext, IIbayContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

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
            var newUser = new User()
            {
                UserPseudo = userPseudo,
                UserEmail = userEmail,
                UserPassword = userPassword,
                UserMoney = 0,
                UserRole = UserRole.StandardUser,
                UserCart = new List<Product>(),
                UserProducts = new List<Product>(),
                CreationDate = DateTime.Now,
                UpdatedDate = null,

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

        public User GetUserById(int userId)
        {
            return Users.SingleOrDefault(u => u.UserId == userId)!;
        }

        public User GetUserByPseudo(string userPseudo)
        {
            return Users.SingleOrDefault(u => u.UserPseudo == userPseudo)!;
        }

        public User UpdateUser(int userId, string userEmail, string userPseudo, string userPassword)
        {
            var userToUpdate = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToUpdate == null)
            {
                return null!;
            }

            if (!userEmail.IsNullOrEmpty())
            {
                userToUpdate.UserEmail = userEmail;
            }
            if (!userPseudo.IsNullOrEmpty())
            {
                userToUpdate.UserPseudo = userPseudo;
            }
            if (!userPassword.IsNullOrEmpty())
            {
                userToUpdate.UserPassword = userPassword;
            }
            
            userToUpdate.UpdatedDate = DateTime.Now;

            var errorMessage = userToUpdate.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            SaveChanges();
            return userToUpdate;
        }
        
        public User DeleteUser(int userId)
        {
            var userToDelete = Users.SingleOrDefault(u => u.UserId == userId);
            if (userToDelete == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            Users.Remove(userToDelete);
            SaveChanges();

            return userToDelete;
        }

        public User ChangeUserRole(int userId, UserRole role)
        {
            var userToChangeRole = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToChangeRole == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new ArgumentException("Invalid user role");
            }

            userToChangeRole.UserRole = role;

            SaveChanges();

            return userToChangeRole;
        }
        
        public Product CreateProduct(int sellerId, string productName, string productDescription, double productPrice, int productStock)
        {
            var seller = Products.FirstOrDefault(u => u.SellerId == sellerId);
            if (seller == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Seller not found");
            }
            
            var newProduct = new Product()
            {
                ProductName = productName,
                ProductDescription = productDescription,
                ProductPrice = productPrice,
                ProductStock = productStock,
                AddedTime = DateTime.Now,
                UpdatedTime = null,
                SellerId = sellerId,
            };

            Products.Add(newProduct);
            SaveChanges();
            return newProduct;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.AddedProducts)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        public Product GetProductById(int productId)
        {
            return Products.SingleOrDefault(p => p.ProductId == productId)!;
        }

        public Product GetProductByName(string productName)
        {
            return Products.SingleOrDefault(p => p.ProductName == productName)!;
        }

        public Product UpdateProduct(int productId, string productName, string productDescription, double? productPrice, int? productStock, bool? available)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToUpdate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            if (!productName.IsNullOrEmpty())
            {
                productToUpdate.ProductName = productName;
            }
            if (!productDescription.IsNullOrEmpty())
            {
                productToUpdate.ProductDescription = productDescription;
            }
            if (productPrice != null)
            {
                productToUpdate.ProductPrice = productPrice.Value;
            }
            if (productStock != null)
            {
                productToUpdate.ProductStock = productStock.Value;
            }
            if (available != null)
            {
                productToUpdate.Available = available.Value;
            }
            
            productToUpdate.UpdatedTime = DateTime.Now;
            SaveChanges();
            return productToUpdate;
        }
        
        public Product DeleteProduct(int productId)
        {
            var productToDelete = Products.SingleOrDefault(p => p.ProductId == productId);
            if (productToDelete == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }
            Products.Remove(productToDelete);
            SaveChanges();
            return productToDelete;
        }
        
        public IEnumerable<Product> GetProductsOnSale(int userId)
        {
            var userToGetProductsOnSale = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToGetProductsOnSale == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            var products = Products.Where(p => p.SellerId == userId).ToList();

            return products;
        }
        
        public User BuyCart(int userId)
        {
            var userToBuyCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToBuyCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            var cartToBuy = userToBuyCart.UserCart;

            if (cartToBuy.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }

            if (cartToBuy.Any(product => !product.Available))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not available");
            }

            var totalPrice = cartToBuy.Sum(p => p.ProductPrice);

            if (userToBuyCart.UserMoney < totalPrice)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money");
            }

            userToBuyCart.UserMoney -= totalPrice;
            cartToBuy.Clear();
            SaveChanges();
            return userToBuyCart;
        }
        
        public User AddProductToCart(int userId, int productId, int quantity)
        {
            var userToAddProductToCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToAddProductToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            var productToAddToCart = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToAddToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            if (!productToAddToCart.Available)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not available");
            }

            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

            if (productToAddToCart.ProductStock < quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough stock");
            }

            userToAddProductToCart.UserCart.Add(productToAddToCart);
            productToAddToCart.ProductStock -= quantity;
            SaveChanges();
            return userToAddProductToCart;
        }
        
        public User RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var userToRemoveProductFromCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToRemoveProductFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            var productToRemoveFromCart = userToRemoveProductFromCart.UserCart.FirstOrDefault(p => p.ProductId == productId);
            if (productToRemoveFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found in cart");
            }

            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

            if (productToRemoveFromCart.ProductStock < quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough stock in cart");
            }

            productToRemoveFromCart.ProductStock -= quantity;

            if (productToRemoveFromCart.ProductStock == 0)
            {
                userToRemoveProductFromCart.UserCart.Remove(productToRemoveFromCart);
            }

            SaveChanges();
            return userToRemoveProductFromCart;
        }
    }
}
