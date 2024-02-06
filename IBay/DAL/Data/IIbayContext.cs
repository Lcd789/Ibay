using DAL.Model;
namespace DAL.Data
{
    public interface IIbayContext
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        User CreateUser(string userPseudo, string userEmail, string userPassword );
        User DeleteUser(int id);
        User GetUserByEmail(string  userEmail);
        User UpdateUser(int userId, string userEmail, string userPseudo, string userPassword);
        User UpdateUserMoney(int userId, double userMoney);
        IEnumerable<Product> GetProductSortedBy(SortCategory sortCategory, int limit);
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        Product GetProductByName(string name);
        Product CreateProduct(int sellerId, string productName, string productDescription, ProductType productType,
            double productPrice, int productStock);
        Product DeleteProduct(int id);
        Product UpdateProduct(int productId, string productName, string productDescription, ProductType productType,
            double? productPrice, int? productStock, bool? available);
        void AddProductToCart(int userId, int productId, int quantity);
        User RemoveProductFromCart(int userId, int productId, int quantity);
        User BuyCart(int userId);
        IEnumerable<Cart> GetCart(int userId);
    }
}
