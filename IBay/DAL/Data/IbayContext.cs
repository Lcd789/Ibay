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
            return Users.SingleOrDefault(u => u.user_id == userId)!;
        }
        
        public User GetUserByEmail(string userEmail)
        {
            return Users.SingleOrDefault(u => u.user_email == userEmail)!;
        }
        
        public User GetUserByPseudo(string userPseudo)
        {
            return Users.SingleOrDefault(u => u.user_pseudo == userPseudo)!;
        }

        public User GetUserCart(int userId)
        {
            return Users.SingleOrDefault(u => u.user_id == userId)!;
        }

        public User UpdateUser(int userId, string userEmail, string userPseudo, string userPassword)
        {
            var userToUpdate = Users.FirstOrDefault(u => u.user_id == userId);
            
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
                throw new BadRequestException(errorMessage);
            }
            
            SaveChanges();
            return userToUpdate;
        }

        public User UpdateUserMoney(int userId, double money)
        {
            var userToPutMoney = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToPutMoney == null)
            {
                throw new NotFoundException("User not found");
            }

            userToPutMoney.user_money += money switch
            {
                0 => throw new BadRequestException("You can't put 0 in this method"),
                < 0 when userToPutMoney.user_money - money >= 0 => money,
                < 0 => throw new BadRequestException("Not enough money"),
                _ => money
            };

            if (userToPutMoney.user_money <= 0)
            {
                throw new BadRequestException("Not enough money");
            }

            SaveChanges();
            return userToPutMoney;
        }

        public User DeleteUser(int userId)
        {
            var userToDelete = Users.SingleOrDefault(u => u.user_id == userId);
            
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
            var userToChangeRole = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToChangeRole == null)
            {
                throw new NotFoundException("User not found");
            }

            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new BadRequestException("Invalid user role");
            }

            userToChangeRole.user_role = role;

            SaveChanges();
            return userToChangeRole;
        }

        public Product CreateProduct(int sellerId, string productName, string productDescription,
            ProductType productType, double productPrice, int productStock)
        {
            
            var seller = Users.FirstOrDefault(u => u.user_id == sellerId);
            
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
                product_name = productName,
                product_description = productDescription,
                product_type = productType,
                product_price = productPrice,
                product_stock = productStock,
                available = true,
                added_time = DateTime.Now,
                updated_time = null,
                fk_user_id = sellerId
            };

            Products.Add(newProduct);
            
            SaveChanges();
            return newProduct;
        }
        
        public Product GetProductById(int productId)
        {
            return Products.Include(p => p.seller).SingleOrDefault(p => p.product_id == productId)!;
        }

        public Product GetProductByName(string productName)
        {
            return Products.Include(p => p.seller).SingleOrDefault(p => p.product_name == productName)!;
        }

        public IEnumerable<Product> GetProducts()
        {
            return Products.Include(p=>p.seller).ToList();
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
                SortCategory.Date => query.OrderByDescending(p => p.added_time),
                SortCategory.Type => query.OrderBy(p => p.product_type),
                SortCategory.Name => query.OrderBy(p => p.product_name),
                SortCategory.Price => query.OrderBy(p => p.product_price),
                _ => query.OrderByDescending(p => p.added_time)
            };

            return query.Take(limit).ToList();
        }

        public Product UpdateProduct(int productId, string productName, string productDescription,
            ProductType productType, double? productPrice, int? productStock, bool? available)
        {
            var productToUpdate = Products.Include(p=>p.seller).FirstOrDefault(p => p.product_id == productId);
            
            if (productToUpdate == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (!productName.IsNullOrEmpty())
            {
                productToUpdate.product_name = productName;
            }
            
            if (!productDescription.IsNullOrEmpty())
            {
                productToUpdate.product_description = productDescription;
            }
            
            if (productType != productToUpdate.product_type)
            {
                productToUpdate.product_type = productType;
            }
            
            if (productPrice != null)
            {
                productToUpdate.product_price = productPrice.Value;
            }
            
            if (productStock != null)
            {
                productToUpdate.product_stock = productStock.Value;
            }
            
            if (available != null)
            {
                productToUpdate.available = available.Value;
            }
            
            productToUpdate.updated_time = DateTime.Now;
            
            SaveChanges();
            return productToUpdate;
        }
        
        public Product DeleteProduct(int productId)
        {
            var productToDelete = Products.SingleOrDefault(p => p.product_id == productId);
            
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
            var userToGetCart = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToGetCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var cart = Carts.Where(c => c.fk_user_id == userId).Include(c=>c.product).ToList();
            /*
            if (cart.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }*/

            return cart;
        }

        public void AddProductToCart(int userId, int productId, int quantity)
        {
            var userToAddProductToCart = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToAddProductToCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var productToAddToCart = Products.FirstOrDefault(p => p.product_id == productId);
            
            if (productToAddToCart == null)
            {
                throw new NotFoundException("Product not found");
            }
            
            if (quantity <= 0)
            {
                throw new BadRequestException("Quantity must be positive");
            }

            if (productToAddToCart.product_stock <= quantity)
            {
                throw new BadRequestException("Not enough stock");
            }

            var existingCart = Carts.FirstOrDefault(c => c.fk_user_id == userId);
            
            if(existingCart != null)
            {
                existingCart.quantity += quantity;
            }
            
            else
            {
                var newCart = new Cart()
                {
                    fk_produc_id = productId,
                    product = productToAddToCart,
                    fk_user_id = userId,
                    user = userToAddProductToCart,
                    quantity = quantity
                };

                Carts.Add(newCart);
                SaveChanges();
            }
        }
        

        public User RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var userToRemoveProductFromCart = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToRemoveProductFromCart == null)
            {
                throw new NotFoundException("User not found");
            }

            var productToRemoveFromCart = Products.FirstOrDefault(p => p.product_id == productId);
            
            if (productToRemoveFromCart == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (quantity <= 0)
            {
                throw new BadRequestException("Quantity must be positive");
            }

            var productInCart = Carts.FirstOrDefault(c => c.fk_user_id == userId && c.fk_produc_id == productId);
            
            if (productInCart == null)
            {
                throw new NotFoundException("Product not in cart");
            }

            if (quantity > productInCart.quantity)
            {
                throw new BadRequestException("Quantity in cart is less than quantity to remove");
            }

            if (quantity == productInCart.quantity)
            {
                Carts.Remove(productInCart);
                
                SaveChanges();
                return userToRemoveProductFromCart;
            }

            productInCart.quantity -= quantity;
            
            SaveChanges();
            return userToRemoveProductFromCart;
        }


        public User BuyCart(int userId)
        {
            var userToBuyCart = Users.FirstOrDefault(u => u.user_id == userId);
            
            if (userToBuyCart == null)
            {
                throw new NotFoundException("User not found");
            }
            var cart = Carts.Include(c => c.product).Where(c => c.fk_user_id == userId).ToList();
            
            if (cart.Count == 0)
            {
                throw new BadRequestException("Cart is empty");
            }
            
            var totalPrice = cart.Sum(product => product.product.product_price * product.quantity);

            if (userToBuyCart.user_money < totalPrice)
            {
                throw new BadRequestException("Not enough money");
            }
            
            userToBuyCart.user_money -= totalPrice;

            Carts.RemoveRange(cart);
            
            SaveChanges();
            return userToBuyCart;
        }
    }
}
