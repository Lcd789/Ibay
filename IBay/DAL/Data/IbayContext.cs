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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "server=bc81bou7c6hqehj9n497-mysql.services.clever-cloud.com;" +
                                       "user=ukkodujekl2bm0ya;" +
                                       "password=0uT89hoAL5YM644TecQ7;" +
                                       "database=bc81bou7c6hqehj9n497\n";
                
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
                
                optionsBuilder.UseMySql(connectionString, serverVersion)
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }

        // User

        public User CreateUser(string UserPseudo, string UserEmail, string UserPassword)
        {
            // Il me faut créer un cart pour chaque utilisateur pour le remplir ensuite lorsqu'il voudra ajouter des produits dans son panier
            
            
            User newUser = new User()
            {
                UserPseudo = UserPseudo,
                UserEmail = UserEmail,
                UserPassword = UserPassword,
                UserMoney = 0,
                UserRole = UserRole.StandardUser,
                UserCart = new List<Product>(),
                UserProducts = new List<Product>(),
                CreationDate = DateTime.Now,
                UpdatedDate = null,

            };

            string errorMessage = newUser.ValidateUser();
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
            return Users.SingleOrDefault(u => u.UserId == userId);
        }

        public User GetUserByPseudo(string userPseudo)
        {
            return Users.SingleOrDefault(u => u.UserPseudo == userPseudo);
        }

        public User UpdateUser(int userId, string userEmail = null, string userPseudo = null, string userPassword = null)
        {
            // Récupérer l'utilisateur à mettre à jour en fonction de son ID
            User userToUpdate = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToUpdate == null)
            {
                return null; // Retourner null pour indiquer que l'utilisateur n'a pas été trouvé
            }

            // Mettre à jour les propriétés fournies
            if (userEmail != null)
            {
                userToUpdate.UserEmail = userEmail;
            }
            if (userPseudo != null)
            {
                userToUpdate.UserPseudo = userPseudo;
            }
            if (userPassword != null)
            {
                userToUpdate.UserPassword = userPassword;
            }
            userToUpdate.UpdatedDate = DateTime.Now;

            // Valider l'utilisateur
            string errorMessage = userToUpdate.ValidateUser();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errorMessage);
            }

            // Sauvegarder les modifications
            SaveChanges();

            return userToUpdate;
        }



        public User DeleteUser(int userId)
        {
            User userToDelete = Users.SingleOrDefault(u => u.UserId == userId);
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
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User userToChangeRole = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToChangeRole == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Vérifier si le rôle fourni est valide
            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new ArgumentException("Invalid user role");
            }

            // Mettre à jour le rôle de l'utilisateur
            userToChangeRole.UserRole = role;

            // Sauvegarder les modifications
            SaveChanges();

            return userToChangeRole;
        }






        // Product

        public Product CreateProduct(int sellerId, string productName, string productDescription, double productPrice, int productStock)
        {
            // Créer un nouveau produit
            Product newProduct = new Product()
            {
                ProductName = productName,
                ProductDescription = productDescription,
                ProductPrice = productPrice,
                ProductStock = productStock,
                AddedTime = DateTime.Now,
                UpdatedTime = null, // Mettre à jour la date de modification
                SellerId = sellerId, // L'ID du vendeur
            };

            // Ajouter le produit à la base de données
            Products.Add(newProduct);
            // Sauvegarder les modifications
            SaveChanges();
            return newProduct;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure la relation entre User et Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.AddedProducts)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.Cascade selon vos besoins
        }


        public Product GetProductById(int productId)
        {
            return Products.SingleOrDefault(p => p.ProductId == productId);
        }

        public Product GetProductByName(string productName)
        {
            return Products.SingleOrDefault(p => p.ProductName == productName);
        }

        public Product UpdateProduct(int productId, string productName = null, string productDescription = null, double? productPrice = null, int? productStock = null, bool? available = null)
        {
            // Récupérer le produit à mettre à jour en fonction de son ID
            Product productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToUpdate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            // Mettre à jour les propriétés fournies
            if (productName != null)
            {
                productToUpdate.ProductName = productName;
            }
            if (productDescription != null)
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
            // Mettre à jour la date de mise à jour
            productToUpdate.UpdatedTime = DateTime.Now;

            // Sauvegarder les modifications
            SaveChanges();

            return productToUpdate;
        }


        public Product DeleteProduct(int productId)
        {
            Product productToDelete = Products.SingleOrDefault(p => p.ProductId == productId);
            if (productToDelete == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }
            Products.Remove(productToDelete);
            SaveChanges();
            return productToDelete;
        }

        // CART :


        // method to sell a product
        // The price of the product is given by the seller
        // The stock of the product is given by the seller
        // The product is available by default
        // The product is added to the database with the current time
        // The product is added to the database with the seller's ID
        // If the product is bought, the stock is decreased
        // If the stock is 0, the product is not available anymore
        // If the product is bought, the seller's money is increased by the price of the product
        // If the product is bought, the buyer's money is decreased by the price of the product

        public Product SellAProduct(int userId, string productName, string productDescription, double productPrice, int productStock)
        {
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User seller = Users.FirstOrDefault(u => u.UserId == userId);
            if (seller == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Créer un nouveau produit
            Product newProduct = new Product()
            {
                ProductName = productName,
                ProductDescription = productDescription,
                ProductPrice = productPrice,
                ProductStock = productStock,
                Available = true,
                AddedTime = DateTime.Now, // Utiliser DateTime.Now pour obtenir l'heure actuelle
                Seller = seller
            };

            // Ajouter le produit à la base de données
            Products.Add(newProduct);

            // Sauvegarder les modifications
            SaveChanges();

            // Retourner le nouveau produit ajouté
            return newProduct;
        }

        // GetProductsOnSale retourne la liste des produits du User où il est désigné comme seller
        public List<Product> GetProductsOnSale(int userId)
        {
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User userToGetProductsOnSale = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToGetProductsOnSale == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Récupérer la liste des produits du vendeur
            List<Product> products = Products.Where(p => p.SellerId == userId).ToList();

            // Retourner la liste des produits
            return products;
        }


        // BuyCart va gérer la transaction entre le User et le Seller du produit
        // Si le User n'existe pas, erreur 404
        // Si le panier du User est vide, erreur 400
        // Si le User n'a pas assez d'argent, erreur 400
        // Si le User a assez d'argent, retirer le montant de la transaction de son argent et ajouter le montant à l'argent du Seller. Vider le panier du User donc après la transaction, vider la liste.
        public User BuyCart(int userId)
        {
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User userToBuyCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToBuyCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Récupérer le panier de l'utilisateur
            List<Product> cartToBuy = userToBuyCart.UserCart;

            // Vérifier que le panier n'est pas vide
            if (cartToBuy.Count == 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Cart is empty");
            }

            // Vérifier que tous les produits du panier sont disponibles
            foreach (Product product in cartToBuy)
            {
                if (!product.Available)
                {
                    throw new System.ComponentModel.DataAnnotations.ValidationException("Product not available");
                }
            }

            // Calculer le prix total du panier
            double totalPrice = cartToBuy.Sum(p => p.ProductPrice);

            // Vérifier si l'utilisateur a assez d'argent pour acheter le panier
            if (userToBuyCart.UserMoney < totalPrice)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough money");
            }

            // Retirer le prix du panier du solde de l'utilisateur
            userToBuyCart.UserMoney -= totalPrice;

            // Vider le panier de l'utilisateur
            cartToBuy.Clear();

            // Sauvegarder les modifications
            SaveChanges();

            // Retourner l'utilisateur mis à jour
            return userToBuyCart;
        }

        // AddProductToCart ajoute la quantité demandée du produit au panier du User
        // La quantité ne peut pas être négative, erreur 400
        // Si la quantité demandée est supérieure à la quantité disponible, erreur 400
        // Si le produit est disponible, ajouter la quantité demandée au panier du User
        // Si le produit n'est pas disponible, erreur 400
        // Si le User n'existe pas, erreur 404
        // Si le produit n'existe pas, erreur 404
        public User AddProductToCart(int userId, int productId, int quantity)
        {
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User userToAddProductToCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToAddProductToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Récupérer le produit dont l'ID est donné en paramètre
            Product productToAddToCart = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToAddToCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found");
            }

            // Vérifier que le produit est disponible
            if (!productToAddToCart.Available)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not available");
            }

            // Vérifier que la quantité est positive
            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

            // Vérifier que le produit est en stock
            if (productToAddToCart.ProductStock < quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough stock");
            }

            // Ajouter le produit au panier de l'utilisateur
            userToAddProductToCart.UserCart.Add(productToAddToCart);

            // Retirer la quantité du stock du produit
            productToAddToCart.ProductStock -= quantity;

            // Sauvegarder les modifications
            SaveChanges();

            // Retourner l'utilisateur mis à jour
            return userToAddProductToCart;
        }

        // RemoveProductFromCart retire la quantité demandée du produit du panier du User
        // La quantité ne peut pas être négative, erreur 400
        // Si la quantité demandée est supérieure à la quantité dans le panier, erreur 400
        // Si le produit est dans le panier, retirer la quantité demandée du panier du User
        // Si le produit n'est pas dans le panier, erreur 400
        // Si le User n'existe pas, erreur 404
        // Si le produit n'existe pas, erreur 404
        public User RemoveProductFromCart(int userId, int productId, int quantity)
        {
            // Récupérer l'utilisateur dont l'ID est donné en paramètre
            User userToRemoveProductFromCart = Users.FirstOrDefault(u => u.UserId == userId);
            if (userToRemoveProductFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("User not found");
            }

            // Récupérer le produit dont l'ID est donné en paramètre
            Product productToRemoveFromCart = userToRemoveProductFromCart.UserCart.FirstOrDefault(p => p.ProductId == productId);
            if (productToRemoveFromCart == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Product not found in cart");
            }

            // Vérifier que la quantité à retirer est positive
            if (quantity <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Quantity must be positive");
            }

            // Vérifier que la quantité demandée n'est pas supérieure à celle dans le panier
            if (productToRemoveFromCart.ProductStock < quantity)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Not enough stock in cart");
            }

            // Retirer la quantité demandée du produit du panier de l'utilisateur
            productToRemoveFromCart.ProductStock -= quantity;

            // Si la quantité du produit dans le panier atteint zéro, retirer le produit du panier
            if (productToRemoveFromCart.ProductStock == 0)
            {
                userToRemoveProductFromCart.UserCart.Remove(productToRemoveFromCart);
            }

            // Sauvegarder les modifications
            SaveChanges();

            // Retourner l'utilisateur mis à jour
            return userToRemoveProductFromCart;
        }

    }
}
