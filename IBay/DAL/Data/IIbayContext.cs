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
        User CreateUser(User user);

        //CRUD Product
        Product GetProductById(int id);
        Product CreateProduct(Product product);

        //CRUD Cart

        Cart GetCartById(int id);

        Cart CreateCart(Cart cart);

    }
}
