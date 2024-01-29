using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        //Création d'un contexte pour pouvoir utiliser les méthodes du DAL
        private IIbayContext _context;

        public CartController(IIbayContext context)
        {
            _context = context;
        }

        // Méthode pour acheter le panier d'un utilisateur
        [HttpPost("{id}")]
        /// <summary> Achat du panier du User </summary>
        /// <param name="id"> Id du User </param>
        /// <returns> Le User avec le panier acheté </returns>
        /// <response code="200"> Le User avec le panier acheté </response>
        /// <response code="400"> Le panier du User est vide </response>
        /// <response code="400"> Le User n'a pas assez d'argent </response>
        /// <response code="404"> Le User n'existe pas </response>
        /// <response code="404"> Le produit n'existe pas </response>
        /// <response code="404"> Le Seller n'existe pas </response>
        /// <response code="404"> Le produit n'est pas disponible </response>
        public IActionResult BuyCart(int id)
        {
            User user = _context.BuyCart(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{userId}")]
        /// <summary> Ajoute la quantité demandée du produit au panier du User </summary>
        /// <param name="userId"> Id du User </param>
        /// <param name="productId"> Id du produit </param>
        /// <param name="quantity"> Quantité à ajouter </param>
        /// <returns> Le User avec le produit ajouté au panier </returns>
        /// <response code="200"> Le User avec le produit ajouté au panier </response>
        /// <response code="400"> La quantité demandée est supérieure à la quantité disponible </response>
        /// <response code="400"> La quantité demandée est négative </response>
        /// <response code="400"> Le produit n'est pas disponible </response>
        /// <response code="404"> Le User n'existe pas </response>
        /// <response code="404"> Le produit n'existe pas </response>
        public IActionResult AddProductToCart(int userId, int productId, int quantity)
        {
            User user = _context.AddProductToCart(userId, productId, quantity);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        /// <summary> "Retire la quantité demandée du produit du panier du User" </summary>
        /// <param name="userId"> Id du User </param>
        /// <param name="productId"> Id du produit </param>
        /// <param name="quantity"> Quantité à retirer </param>
        /// <returns> Le User avec le produit retiré du panier </returns>
        /// <response code="200"> Le User avec le produit retiré du panier </response>
        /// <response code="400"> La quantité demandée est supérieure à la quantité dans le panier </response>
        /// <response code="400"> La quantité demandée est négative </response>
        /// <response code="400"> Le produit n'est pas dans le panier </response>
        /// <response code="404"> Le User n'existe pas </response>
        /// <response code="404"> Le produit n'existe pas </response>
        /// <response code="404"> Le Seller n'existe pas </response>
        public IActionResult RemoveProductFromCart(int userId, int productId, int quantity)
        {
            User user = _context.RemoveProductFromCart(userId, productId, quantity);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{userId}")]
        /// <summary> "Retourne le panier du User" </summary>
        /// <param name="userId"> Id du User </param>
        /// <returns> Le panier du User </returns>
        /// <response code="200"> Le panier du User </response>
        /// <response code="404"> Le User n'existe pas </response>
        public IActionResult GetCart(int userId)
        {
            User user = _context.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.UserCart.ToList());
        }


    }
}
