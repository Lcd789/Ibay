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

        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message)
            {
            }
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message)
            {
            }
        }



        public User CreateUser(string userPseudo, string userEmail, string userPassword)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPassword, salt);

            var newUser = new User()
            {
                UserPseudo = userPseudo,
                UserEmail = userEmail,
                UserPassword = hashedPassword,
                UserMoney = 0,
                UserRole = UserRole.StandardUser,
                UpdatedDate = null,
                CreationDate = DateTime.Now,
            };

            var errorMessage = newUser.ValidateUser();
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new BadRequestException(errorMessage);
            }
            
            Users.Add(newUser);
            
            SaveChanges();
            return newUser;
        }

        public IEnumerable<User> GetUsers()
        {
            return Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return Users.SingleOrDefault(u => u.UserId == userId)!;
        }
        
        public User GetUserByEmail(string userEmail)
        {
            return Users.SingleOrDefault(u => u.UserEmail == userEmail)!;
        }
        
        public User GetUserByPseudo(string userPseudo)
        {
            return Users.SingleOrDefault(u => u.UserPseudo == userPseudo)!;
        }

        public User GetUserCart(int userId)
        {
            return Users.SingleOrDefault(u => u.UserId == userId)!;
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
                throw new BadRequestException(errorMessage);
            }
            
            SaveChanges();
            return userToUpdate;
        }

        public User UpdateUserMoney(int userId, double money)
        {
            var userToPutMoney = Users.FirstOrDefault(u => u.UserId == userId);
            
            if (userToPutMoney == null)
            {
                throw new NotFoundException("User not found");
            }

            userToPutMoney.UserMoney += money switch
            {
                0 => throw new BadRequestException("You can't put 0 in this method"),
                < 0 when userToPutMoney.UserMoney - money >= 0 => money,
                < 0 => throw new BadRequestException("Not enough money"),
                _ => money
            };

            if (userToPutMoney.UserMoney <= 0)
            {
                throw new BadRequestException("Not enough money");
            }

            SaveChanges();
            return userToPutMoney;
        }

        public User DeleteUser(int userId)
        {
            var userToDelete = Users.SingleOrDefault(u => u.UserId == userId);
            
            if (userToDelete == null)
            {
                throw new NotFoundException("User not found");
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
                throw new NotFoundException("User not found");
            }

            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new BadRequestException("Invalid user role");
            }

            userToChangeRole.UserRole = role;

            SaveChanges();
            return userToChangeRole;
        }

        public Product CreateProduct(int sellerId, string productName, string productDescription,
            ProductType productType, double productPrice, int productStock)
        {
            
            var seller = Users.FirstOrDefault(u => u.UserId == sellerId);
            
            if (seller == null)
            {
                throw new NotFoundException("Seller not found");
            }

            if (productPrice <= 0)
            {
                throw new BadRequestException("Price must be positive");
            }

            if (productStock <= 0)
            {
                throw new BadRequestException("Stock must be positive");
            }
            
            var newProduct = new Product()
            {
                ProductName = productName,
                ProductDescription = productDescription,
                ProductType = productType,
                ProductPrice = productPrice,
                ProductStock = productStock,
                Available = true,
                AddedTime = DateTime.Now,
                UpdatedTime = null,
                FK_UserId = sellerId
            };

            Products.Add(newProduct);
            
            SaveChanges();
            return newProduct;
        }
        
        public Product GetProductById(int productId)
        {
            return Products.Include(p => p.Seller).SingleOrDefault(p => p.ProductId == productId)!;
        }

        public Product GetProductByName(string productName)
        {
            return Products.Include(p => p.Seller).SingleOrDefault(p => p.ProductName == productName)!;
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products.Include(p=>p.Seller).ToList();
        }
        
        public IEnumerable<Product> GetProductSortedBy(SortCategory sortCategory, int limit)
        {
            if (limit.GetType() != typeof(int))
            {
                throw new BadRequestException("Limit must be an integer");
            }
            
            IQueryable<Product> query = Products;
            
            query = sortCategory switch
            {
                SortCategory.Date => query.OrderByDescending(p => p.AddedTime),
                SortCategory.Type => query.OrderBy(p => p.ProductType),
                SortCategory.Name => query.OrderBy(p => p.ProductName),
                SortCategory.Price => query.OrderBy(p => p.ProductPrice),
                _ => query.OrderByDescending(p => p.AddedTime)
            };

            return query.Take(limit).ToList();
        }

        public Product UpdateProduct(int productId, string productName, string productDescription,
            ProductType productType, double? productPrice, int? productStock, bool? available)
        {
            var productToUpdate = Products.Include(p=>p.Seller).FirstOrDefault(p => p.ProductId == productId);
            
            if (productToUpdate == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (!productName.IsNullOrEmpty())
            {
                productToUpdate.ProductName = productName;
            }
            
            if (!productDescription.IsNullOrEmpty())
            {
                productToUpdate.ProductDescription = productDescription;
            }
            
            if (productType != productToUpdate.ProductType)
            {
                productToUpdate.ProductType = productType;
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
                throw new NotFoundException("Product not found");
            }

            Products.Remove(productToDelete);
            
            SaveChanges();
            return productToDelete;
        }

        public IEnumerable<Cart> GetCart(int userId)
        {
            var userToGetCart = Users.FirstOrDefault(u => u.UserId == userId);
            
            if (userToGetCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var cart = Carts.Where(c => c.FK_UserId == userId).Include(c=>c.Product).ToList();
            /*
            if (cart.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }*/

            return cart;
        }

        public void AddProductToCart(int userId, int productId, int quantity)
        {
            var userToAddProductToCart = Users.FirstOrDefault(u => u.UserId == userId);
            
            if (userToAddProductToCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var productToAddToCart = Products.FirstOrDefault(p => p.ProductId == productId);
            
            if (productToAddToCart == null)
            {
                throw new NotFoundException("Product not found");
            }
            
            if (quantity <= 0)
            {
                throw new BadRequestException("Quantity must be positive");
            }

            if (productToAddToCart.ProductStock <= quantity)
            {
                throw new BadRequestException("Not enough stock");
            }

            var existingCart = Carts.FirstOrDefault(c => c.FK_UserId == userId);
            
            if(existingCart != null)
            {
                existingCart.Quantity += quantity;
            }
            
            else
            {
                var newCart = new Cart()
                {
                    FK_ProductId = productId,
                    Product = productToAddToCart,
                    FK_UserId = userId,
                    User = userToAddProductToCart,
                    Quantity = quantity
                };

                Carts.Add(newCart);
                SaveChanges();
            }
        }
        

        public User RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var userToRemoveProductFromCart = Users.FirstOrDefault(u => u.UserId == userId);
            
            if (userToRemoveProductFromCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var productToRemoveFromCart = Products.FirstOrDefault(p => p.ProductId == productId);
            
            if (productToRemoveFromCart == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (quantity <= 0)
            {
                throw new BadRequestException("Quantity must be positive");
            }

            var productInCart = Carts.FirstOrDefault(c => c.FK_UserId == userId && c.FK_ProductId == productId);
            
            if (productInCart == null)
            {
                throw new NotFoundException("Product not in cart");
            }

            if (quantity > productInCart.Quantity)
            {
                throw new BadRequestException("Quantity in cart is less than quantity to remove");
            }

            if (quantity == productInCart.Quantity)
            {
                Carts.Remove(productInCart);
                
                SaveChanges();
                return userToRemoveProductFromCart;
            }

            productInCart.Quantity -= quantity;
            
            SaveChanges();
            return userToRemoveProductFromCart;
        }


        public User BuyCart(int userId)
        {
            var userToBuyCart = Users.FirstOrDefault(u => u.UserId == userId);
            
            if (userToBuyCart == null)
            {
                throw new NotFoundException("User not found");
            }
            var cart = Carts.Include(c => c.Product).Where(c => c.FK_UserId == userId).ToList();
            
            if (cart.Count == 0)
            {
                throw new BadRequestException("Cart is empty");
            }
            
            var totalPrice = cart.Sum(product => product.Product.ProductPrice * product.Quantity);

            if (userToBuyCart.UserMoney < totalPrice)
            {
                throw new BadRequestException("Not enough money");
            }
            
            userToBuyCart.UserMoney -= totalPrice;

            Carts.RemoveRange(cart);
            
            SaveChanges();
            return userToBuyCart;
        }
    }
}
