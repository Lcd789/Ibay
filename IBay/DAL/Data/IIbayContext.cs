using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.Model;
namespace DAL.Data
{
    public interface IIbayContext
    {
        //CRUD User
        User GetUserById(int id);
        User CreateUser(string UserPseudo, string UserEmail, string UserPassword );

        User DeleteUser(int id);

        User UpdateUser(int userId, string userEmail = null, string userPseudo = null, string userPassword = null);

        // Endpoint spécifique :


        //CRUD Product
        Product GetProductById(int id);

        Product GetProductByName(string name);

        Product CreateProduct(int sellerId, string productName, string productDescription, double productPrice, int productStock);

        Product DeleteProduct(int id);

        Product UpdateProduct(int productId, string productName = null, string productDescription = null, double? productPrice = null, int? productStock = null, bool? available = null);

        public Product SellAProduct(int userId, string productName, string productDescription, double productPrice, int productStock);


        // Cart

        // AddProductToCart ajoute la quantité demandée du produit au panier du User
        // La quantité ne peut pas être négative, erreur 400
        // Si la quantité demandée est supérieure à la quantité disponible, erreur 400
        // Si le produit est disponible, ajouter la quantité demandée au panier du User
        // Si le produit n'est pas disponible, erreur 400
        // Si le User n'existe pas, erreur 404
        // Si le produit n'existe pas, erreur 404
        User AddProductToCart(int userId, int productId, int quantity);

        // RemoveProductFromCart retire la quantité demandée du produit du panier du User
        // La quantité ne peut pas être négative, erreur 400
        // Si la quantité demandée est supérieure à la quantité dans le panier, erreur 400
        // Si le produit est dans le panier, retirer la quantité demandée du panier du User
        // Si le produit n'est pas dans le panier, erreur 400
        // Si le User n'existe pas, erreur 404
        // Si le produit n'existe pas, erreur 404
        User RemoveProductFromCart(int userId, int productId, int quantity);

        // BuyCart va gérer la transaction entre le User et le Seller du produit
        // Si le User n'existe pas, erreur 404
        // Si le panier du User est vide, erreur 400
        // Si le User n'a pas assez d'argent, erreur 400
        // Si le User a assez d'argent, retirer le montant de la transaction de son argent et ajouter le montant à l'argent du Seller. Vider le panier du User donc après la transaction, vider la liste.
        User BuyCart(int userId);

        // GetProductsOnSale retourne la liste des produits du User où il est désigné comme seller
        List<Product> GetProductsOnSale(int userId);
        

    }
}
