using DAL.Model;
namespace DAL.Data
{
    public interface IIbayContext
    {
        User GetUserById(int id);
        User CreateUser(string userPseudo, string userEmail, string userPassword );

        User DeleteUser(int id);

        User UpdateUser(int userId, string userEmail, string userPseudo, string userPassword);

        IEnumerable<Product> GetProducts(string sortBy, int limit);
        
        Product GetProductById(int id);

        Product GetProductByName(string name);

        Product CreateProduct(int sellerId, string productName, string productDescription, double productPrice, int productStock);

        Product DeleteProduct(int id);

        Product UpdateProduct(int productId, string productName, string productDescription, double? productPrice, int? productStock, bool? available);
        
        User AddProductToCart(int userId, int productId, int quantity);

        User RemoveProductFromCart(int userId, int productId, int quantity);
        
        User BuyCart(int userId);

        IEnumerable<Product> GetProductsOnSale(int userId);
        User UpdateUserMoney(int userId, double money);
    }
}
