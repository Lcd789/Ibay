using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.ComponentModel.DataAnnotations;
using Bogus;

namespace DAL.Data
{
    public class IbayContext : DbContext, IIbayContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string LOUIS_Bdd = "Data Source=HELIX\\SQLExpress;Initial Catalog=IBAY2;Integrated Security=True;Trust Server Certificate=True";

            // Pour lorsqu'on récupère le projet, il faut mettre le chemin de sa bdd
            string MATHIS_Bdd = ""; 
            string SAMY_Bdd = "";

            var connectionString = LOUIS_Bdd; // mettre la variable de SA PROPRE BDD LOCALE
            optionsBuilder.UseSqlServer(connectionString);
        }

        // User

        public User CreateUser(User user)
        {
            string errorMessage = user.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Users.Add(user);
            SaveChanges();
            return user;
        }

        public User GetUserById(int userId)
        {
            return Users.Find(userId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
        }

        public User UpdateUser(User updatedUser)
        {
            string errorMessage = updatedUser.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            User userToUpdate = Users.Find(updatedUser.UserId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            userToUpdate.UserEmail = updatedUser.UserEmail;
            userToUpdate.UserPseudo = updatedUser.UserPseudo;
            userToUpdate.UserPassword = updatedUser.UserPassword;
            userToUpdate.UserRole = updatedUser.UserRole;
            SaveChanges();
            return userToUpdate;
        }

        public User DeleteUser(int userId)
        {
            User userToDelete = Users.Find(userId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            Users.Remove(userToDelete);
            SaveChanges();
            return userToDelete;
        }

        public User ChangeUserRole(int userId, UserRole role)
        {
            User userToChangeRole = Users.Find(userId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            userToChangeRole.UserRole = role;
            SaveChanges();
            return userToChangeRole;
        }

        // Product

        public Product CreateProduct(Product product)
        {
            string errorMessage = product.ValidateProduct();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Products.Add(product);
            SaveChanges();
            return product;
        }

        public Product GetProductById(int productId)
        {
            return Products.Find(productId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
        }

        public Product UpdateProduct(Product updatedProduct)
        {
            string errorMessage = updatedProduct.ValidateProduct();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Product productToUpdate = Products.Find(updatedProduct.ProductId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
           
            productToUpdate.ProductName = updatedProduct.ProductName;
            productToUpdate.ProductDescription = updatedProduct.ProductDescription;
            productToUpdate.ProductPrice = updatedProduct.ProductPrice;
            productToUpdate.ProductStock = updatedProduct.ProductStock;
            productToUpdate.Available = updatedProduct.Available;
            productToUpdate.UpdatedTime = new DateTime();
            productToUpdate.Seller = updatedProduct.Seller;
            
            SaveChanges();
            return productToUpdate;
        }

        public Product DeleteProduct(int productId)
        {
            Product productToDelete = Products.Find(productId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            Products.Remove(productToDelete);
            SaveChanges();
            return productToDelete;
        }

        // Cart

        public Cart CreateCart(Cart cart)
        {
            string errorMessage = cart.ValidateCart();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Carts.Add(cart);
            SaveChanges();
            return cart;
        }

        public Cart GetCartById(int cartId)
        {
            return Carts.Find(cartId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Cart not found");
        }

        // Méthode assez maladroite en raison qu'on n'utilise que l'id , à revoir
        public Cart UpdateCart(Cart updatedCart)
        {
            string errorMessage = updatedCart.ValidateCart();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }
            Cart cartToUpdate = Carts.Find(updatedCart.CartId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Cart not found");
            cartToUpdate.UserId = updatedCart.UserId;
            cartToUpdate.Products = updatedCart.Products;
            cartToUpdate.UpdatedAt = new DateTime();
            SaveChanges();
            return cartToUpdate;
        }

        public Cart DeleteCart(int cartId)
        {
            Cart cartToDelete = Carts.Find(cartId) ?? throw new System.ComponentModel.DataAnnotations.ValidationException("Cart not found");
            Carts.Remove(cartToDelete);
            SaveChanges();
            return cartToDelete;
        }

        public void AddProductToCart(int productId, int cartId, int quantity)
        {
            User user = GetUserById(cartId);
            Product product = GetProductById(productId);
            if(user == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }
            if(product == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }
            

        }



        /*
        //CRUD User

        // GET
        public User GetUserById(int id)
        {
            User user = (User)this.Users.Where(u => u.UserId == id);
            return user;
        }

        public User GetUserByPseudo(string pseudo)
        {
            User user = (User)this.Users.Where(u => u.UserPseudo == pseudo);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user = (User)this.Users.Where(u => u.UserEmail == email);
            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = this.Users.ToList();
            return users;
        }

        //POST
        public User CreateUser(User user)
        {
            User NewUser = new User() { UserRole = UserRole.StandardUser, UserEmail = user.UserEmail, UserPseudo = user.UserPseudo, UserPassword = user.UserPassword };
            string errorMessage = NewUser.ValidateUser();
            if(!string.IsNullOrEmpty(errorMessage))
            {
                throw new ValidationException(errorMessage);
            }
            else
            {
                Users.Add(NewUser);
                SaveChanges();
                return NewUser;
            }
        }

        //PUT
        public User UpdateUser(User user)
        {
            string errorMessage = user.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ValidationException(errorMessage);
            }
            else
            {
                User userToUpdate = (User)this.Users.Where(u => u.UserId == user.UserId);
                userToUpdate.UserEmail = user.UserEmail;
                userToUpdate.UserPseudo = user.UserPseudo;
                userToUpdate.UserPassword = user.UserPassword;
                userToUpdate.UserRole = user.UserRole;
                // Au lieu de faire toutes les vérifications une par une, on ne peut pas juste faire User.Update(user) ?
                // Et dans la méthode Update, on fait toutes les vérifications ?
                // Et si une vérification n'est pas bonne, on renvoie une erreur ?
                // Et si toutes les vérifications sont bonnes, on fait les changements ?
                // Et si on veut changer le rôle d'un utilisateur, on fait une méthode à part
                // 
                SaveChanges();
                return userToUpdate;
            }
        }

        //DELETE
        public User DeleteUser(User user)
        {
            User userToDelete = (User)this.Users.Where(u => u.UserId == user.UserId);
            Users.Remove(userToDelete);
            SaveChanges();
            return userToDelete;
        }

        //Changement de rôle

        // Vérifier si le rôle est bien renseigné
        // Vérifier si le rôle est bien un rôle existant
        // Vérifier si le rôle est bien un rôle valide (Admin, Moderator, StandardUser, Banned)
        // Seul un admin peut changer le rôle d'un utilisateur
        // Un utilisateur ne peut pas changer son propre rôle
        
        public User ChangeUserRole(int id, UserRole role)
        {
            User userToChangeRole = (User)this.Users.Where(u => u.UserId == id);
            if (userToChangeRole == null)
            {
                throw new ValidationException("User not found");
            }
            else
            {
                userToChangeRole.UserRole = role;
                SaveChanges();
                return userToChangeRole;
            }
        }


        //CRUD Product

        // GET
        public Product GetProductById(int id)
        {
            Product product = (Product)this.Products.Where(p => p.ProductId == id);
            return product;
        }

        public Product GetProductByName(string name)
        {
            Product product = (Product)this.Products.Where(p => p.ProductName == name);
            return product;
        }

        public Product GetProductByCategory(string category)
        {
            Product product = (Product)this.Products.Where(p => p.ProductCategory == category);
            return product;
        }

        public Product GetProductBySubCategory(string subCategory)
        {
            Product product = (Product)this.Products.Where(p => p.ProductSubCategory == subCategory);
            return product;
        }

        public Product GetProductByBrand(string brand)
        {
            Product product = (Product)this.Products.Where(p => p.ProductBrand == brand);
            return product;
        }

        public Product GetProductBySeller(User seller)
        {
            Product product = (Product)this.Products.Where(p => p.ProductSeller == seller);
            return product;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = this.Products.ToList();
            return products;
        }

        //POST
        public Product CreateProduct(Product product)
        {
            Product NewProduct = new Product() { ProductName = product.ProductName, ProductImage = product.ProductImage, ProductPrice = product.ProductPrice, ProductAvailable = product.ProductAvailable, ProductAddedTime = product.ProductAddedTime, ProductSeller = product.ProductSeller, ProductStock = product.ProductStock, ProductComments = product.ProductComments, ProductCategory = product.ProductCategory, ProductSubCategory = product.ProductSubCategory, ProductBrand = product.ProductBrand };
            string errorMessage = NewProduct.ValidateProduct();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ValidationException(errorMessage);
            }
            else
            {
                Products.Add(NewProduct);
                SaveChanges();
                return NewProduct;
            }
        }

        public Product AddProductToCart(Product productID, Cart cartID, int quantity)
        {
            Cart cart = (Cart)this.Carts.Where(c => c.CartId == cartID.CartId);
            Product product = (Product)this.Products.Where(p => p.ProductId == productID.ProductId);
            CartItem cartItem = new CartItem() { Cart = cart, Product = product, Quantity = quantity };
            SaveChanges();
            return product;
        }

        //PUT
        public Product UpdateProduct(Product product)
        {
            string errorMessage = product.ValidateProduct();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ValidationException(errorMessage);
            }
            else
            {
                Product productToUpdate = (Product)this.Products.Where(p => p.ProductId == product.ProductId);
                productToUpdate.ProductName = product.ProductName;
                productToUpdate.ProductImage = product.ProductImage;
                productToUpdate.ProductPrice = product.ProductPrice;
                productToUpdate.ProductAvailable = product.ProductAvailable;
                productToUpdate.ProductAddedTime = product.ProductAddedTime;
                productToUpdate.ProductSeller = product.ProductSeller;
                productToUpdate.ProductStock = product.ProductStock;
                productToUpdate.ProductComments = product.ProductComments;
                productToUpdate.ProductCategory = product.ProductCategory;
                productToUpdate.ProductSubCategory = product.ProductSubCategory;
                productToUpdate.ProductBrand = product.ProductBrand;
                SaveChanges();
                return productToUpdate;
            }
        }

        //DELETE
        public Product DeleteProduct(Product product)
        {
            Product productToDelete = (Product)this.Products.Where(p => p.ProductId == product.ProductId);
            Products.Remove(productToDelete);
            SaveChanges();
            return productToDelete;
        }

        //CRUD Cart

        // GET
        public Cart GetCartById(int id)
        {
            Cart cart = (Cart)this.Carts.Where(c => c.CartId == id);
            return cart;
        }

        public List<Cart> GetAllCarts()
        {
            List<Cart> carts = this.Carts.ToList();
            return carts;
        }

        public Cart GetCartByUser(User user)
        {
            Cart cart = (Cart)this.Carts.Where(c => c.Owner == user);
            return cart;
        }*/

    }
}
