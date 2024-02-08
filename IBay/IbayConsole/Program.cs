namespace IbayConsole
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseUrl = "https://localhost:7055/api/";

        static async Task Main(string[] args)
        {
            var baseUrl = "https://localhost:7055/api/";
            Console.WriteLine("Appel de l'API en local...");

            try
            {
                var response = await client.GetAsync(baseUrl + "endpoint");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Réponse de l'API : " + content);
                }
                else
                {
                    Console.WriteLine("La requête a échoué avec le code : " + response.StatusCode);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'appel de l'API : " + e.Message);
            }
           /*
            Console.WriteLine("Bienvenue dans l'application Ibay !");
            Console.WriteLine();
            Console.WriteLine("Possédez vous un compte ou vous en créer un ?");
            Console.WriteLine("1. J'ai un compte");
            Console.WriteLine("2. Je n'ai pas de compte");
            Console.Write("Choisissez une action : ");
            string choix = Console.ReadLine();
            if (choix == "1")
            {
                await SeConnecter();
            }
            else if (choix == "2")
            {
                await CreerCompte();
            }
            else
            {
                Console.WriteLine("Choix invalide. Veuillez réessayer.");
            }


            
            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Afficher tous les produits");
                Console.WriteLine("2. Ajouter un produit");
                Console.WriteLine("3. Mettre à jour un produit");
                Console.WriteLine("4. Supprimer un produit");
                Console.WriteLine("5. Quitter");

                Console.Write("Choisissez une action : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        await AfficherTousLesProduits();
                        break;
                    case "2":
                        await AjouterProduit();
                        break;
                    case "3":
                        await MettreAJourProduit();
                        break;
                    case "4":
                        await SupprimerProduit();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        break;
                }
            }
        }*/
            static async Task SeConnecter()
            {
                Console.Write("Entrez votre pseudo : ");
                string pseudo = Console.ReadLine();
                Console.Write("Entrez votre mot de passe : ");
                string motDePasse = Console.ReadLine();

                var content = new FormUrlEncodedContent(new[]
                {
        new KeyValuePair<string, string>("userPseudo", pseudo),
        new KeyValuePair<string, string>("userPassword", motDePasse)
    });

                var response = await client.PostAsync(baseUrl + "User", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Connexion réussie !");
                    var token = await response.Content.ReadAsStringAsync();
                    // Stockez le jeton d'authentification pour l'utiliser dans les requêtes ultérieures
                }
                else
                {
                    Console.WriteLine("Échec de la connexion. Veuillez vérifier vos informations d'identification.");
                }
            }



        }

    }
}


