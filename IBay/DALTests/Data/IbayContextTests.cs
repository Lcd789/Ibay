using System.ComponentModel.DataAnnotations;
using DAL.Data;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DALTests.Data
{
    [TestClass]
    public class IbayContextTests
    {
        private IbayContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<IbayContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            return new IbayContext(options);
        }
        
        [TestMethod]
        public void shouldCreateUser_whenUserIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com";
            const string userPassword = "test#123TEST";

            // Act
            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            // Assert
            Assert.IsNotNull(newUser);
            Assert.AreEqual(userPseudo, newUser.UserPseudo);
            Assert.AreEqual(userEmail, newUser.UserEmail);
            Assert.AreEqual(userPassword, newUser.UserPassword);
        }
        
        [TestMethod]
        public void shouldCreateUserWithUserRoleStandardUser_whenUserIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com18";
            const string userPassword = "test#123TEST";

            // Act
            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            // Assert
            Assert.IsNotNull(newUser);
            Assert.AreEqual(UserRole.StandardUser, newUser.UserRole);
        }
        
        [TestMethod]
        public void shouldCreateUserWithCreationDate_whenUserIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com19";
            const string userPassword = "test#123TEST";

            // Act
            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            // Assert
            Assert.IsNotNull(newUser);
            Assert.IsNotNull(newUser.CreationDate);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void shouldThrowInvalidOperationException_whenEmailAlreadyExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "TestUser1";
            const string userEmail = "test@example.com2";
            const string userPassword1 = "test#123TEST";
            const string userPseudo2 = "TestUser2";
            const string userPassword2 = "test#123TEST2";

            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
            ibayContext.CreateUser(userPseudo2, userEmail, userPassword2);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserPseudoIsMoreThan40Characters()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "ATestUserWithMoreThanFortyCharactersShouldThrowAnException";
            const string userEmail = "test@example.com3";
            const string userPassword1 = "test#123TEST";

            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserPseudoIsLessThan3Characters()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "a";
            const string userEmail = "test@example.com4";
            const string userPassword1 = "test#123TEST";

            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserPasswordIsLessThan8Characters()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "TestPseudo";
            const string userEmail = "test@example.com5";
            const string userPassword1 = "Failed";

            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserPasswordIsMoreThan255Characters()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "TestPseudo";
            const string userEmail = "test@example.com6";
            const string userPassword1 = "ThisIsAnExampleOfAVeryLongPasswordThatCouldBeUsedToDemonstrate" +
                                         "ALengthLimitOf255CharactersForPasswordsButInPracticeSuchALongP" +
                                         "asswordWouldBeImpracticalAndHardToRemember!ThisIsAnExampleOfA" +
                                         "VeryLongPasswordThatExceedsThe255CharacterLimitForDemonstratio" +
                                         "nPurposesButInRealLifeUsingSuchALongPasswordWouldBeInconvenient" +
                                         "AndChallengingToRemember!#+=à";
            
            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserPasswordIsNotCorrect()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "TestPseudo";
            const string userEmail = "test@example.com7";
            const string userPassword1 = "NotCorrect123";
            
            // Act
            ibayContext.CreateUser(userPseudo1, userEmail, userPassword1);
        }
        
        [TestMethod]
        public void shouldRetrieveUserById_whenUserExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com8";
            const string userPassword = "test#123TEST";

            // Act
            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            // Assert
            var retrievedUser = ibayContext.GetUserById(userId);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(userId, retrievedUser.UserId);
            Assert.AreEqual(userPseudo, retrievedUser.UserPseudo);
            Assert.AreEqual(userEmail, retrievedUser.UserEmail);
            Assert.AreEqual(userPassword, retrievedUser.UserPassword);
        }
        
        [TestMethod]
        public void shouldReturnNull_whenUserIdDoesNotExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            // Act
            var retrievedUser = ibayContext.GetUserById(nonExistentUserId);

            // Assert
            Assert.IsNull(retrievedUser);
        }
        
        [TestMethod]
        public void shouldRetrieveUserByPseudo_whenUserExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser123456";
            const string userEmail = "test@example.com9";
            const string userPassword = "test#123TEST";

            // Act
            ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            // Assert
            var retrievedUser = ibayContext.GetUserByPseudo(userPseudo);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(userPseudo, retrievedUser.UserPseudo);
            Assert.AreEqual(userEmail, retrievedUser.UserEmail);
            Assert.AreEqual(userPassword, retrievedUser.UserPassword);
        }
        
        [TestMethod]
        public void shouldReturnNull_whenUserPseudoDoesNotExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            var nonExistentUserPseudo = "abc";

            // Act
            var retrievedUser = ibayContext.GetUserByPseudo(nonExistentUserPseudo);

            // Assert
            Assert.IsNull(retrievedUser);
        }
        
        [TestMethod]
        public void shouldUpdateUserSuccessfully_whenEverythingIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com10";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            const string updatedEmail = "updated@example.com1";
            const string updatedPseudo = "UpdatedUser";
            const string updatedPassword = "updated#456TEST";

            // Act
            var updatedUser = ibayContext.UpdateUser(userId, updatedEmail, updatedPseudo, updatedPassword);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(userId, updatedUser.UserId);
            Assert.AreEqual(updatedEmail, updatedUser.UserEmail);
            Assert.AreEqual(updatedPseudo, updatedUser.UserPseudo);
            Assert.AreEqual(updatedPassword, updatedUser.UserPassword);
        }
        
        [TestMethod]
        public void shouldReturnUnchangedUser_whenNoFieldsUpdated()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com11";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            // Act
            var updatedUser = ibayContext.UpdateUser(userId, string.Empty, string.Empty, string.Empty);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(userId, updatedUser.UserId);
            Assert.AreEqual(userEmail, updatedUser.UserEmail);
            Assert.AreEqual(userPseudo, updatedUser.UserPseudo);
            Assert.AreEqual(userPassword, updatedUser.UserPassword);
        }
        
        [TestMethod]
        public void shouldReturnNull_whenUserIsNotFound()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int wrongId = 123;
            
            // Act
            var updatedUser = ibayContext.UpdateUser(wrongId, string.Empty, string.Empty, string.Empty);

            // Assert
            Assert.IsNull(updatedUser);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void shouldThrowInvalidOperationException_whenUpdatingWithDuplicateEmail()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo1 = "TestUser1";
            const string userEmail1 = "test@example.com12";
            const string userPassword1 = "test#123TEST";
            const string userPseudo2 = "TestUser2";
            const string userEmail2 = "test@example.com13";
            const string userPassword2 = "test#123TEST2";

            ibayContext.CreateUser(userPseudo1, userEmail1, userPassword1);

            var user2 = ibayContext.CreateUser(userPseudo2, userEmail2, userPassword2);
            var userId2 = user2.UserId;

            // Act
            ibayContext.UpdateUser(userId2, userEmail1, "updatedPseudo", "updated#789TEST");
        }
        
        [TestMethod]
        public void shouldSetUpdatedDate_whenUserIsUpdated()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com14";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            const string updatedEmail = "updated@example.com2";
            const string updatedPseudo = "UpdatedUser";
            const string updatedPassword = "updated#456TEST";

            // Act
            var updatedUser = ibayContext.UpdateUser(userId, updatedEmail, updatedPseudo, updatedPassword);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(userId, updatedUser.UserId);
            Assert.AreEqual(updatedEmail, updatedUser.UserEmail);
            Assert.AreEqual(updatedPseudo, updatedUser.UserPseudo);
            Assert.AreEqual(updatedPassword, updatedUser.UserPassword);
            Assert.IsNotNull(updatedUser.UpdatedDate);
        }
        
        [TestMethod]
        public void shouldDeleteUserSuccessfully_whenUserExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com15";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            // Act
            var deletedUser = ibayContext.DeleteUser(userId);

            // Assert
            Assert.IsNotNull(deletedUser);
            Assert.AreEqual(userId, deletedUser.UserId);
            Assert.IsFalse(ibayContext.Users.Any(u => u.UserId == userId));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenDeletingNonExistentUser()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            // Act
            ibayContext.DeleteUser(nonExistentUserId);
        }

        [TestMethod]
        public void shouldChangeUserRoleSuccessfully_whenUserExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com16";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            // Act
            var updatedUser = ibayContext.ChangeUserRole(userId, UserRole.Admin);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(userId, updatedUser.UserId);
            Assert.AreEqual(UserRole.Admin, updatedUser.UserRole);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenChangingRoleOfNonExistentUser()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            // Act
            ibayContext.ChangeUserRole(nonExistentUserId, UserRole.Admin);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void shouldThrowArgumentException_whenChangingRoleWithInvalidRole()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com17";
            const string userPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(userPseudo, userEmail, userPassword);
            var userId = newUser.UserId;

            const UserRole invalidRole = (UserRole)100;

            // Act
            ibayContext.ChangeUserRole(userId, invalidRole);
        }
        
        [TestMethod]
        public void shouldCreateProductSuccessfully_whenProductIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string productDescription = "Description of TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            // Act
            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, productDescription, productPrice, productStock);

            // Assert
            Assert.IsNotNull(newProduct);
            Assert.AreEqual(productName, newProduct.ProductName);
            Assert.AreEqual(productDescription, newProduct.ProductDescription);
            Assert.AreEqual(productPrice, newProduct.ProductPrice);
            Assert.AreEqual(productStock, newProduct.ProductStock);
            Assert.AreEqual(newUser.UserId, newProduct.SellerId);
            Assert.IsNotNull(newProduct.AddedTime);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenSellerNotFound()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentSellerId = 999;

            // Act
            ibayContext.CreateProduct(nonExistentSellerId, "TestProduct", "Description", 99.99, 10);
        }
        
        [TestMethod]
        public void shouldCreateProductWithAddedTime_whenProductIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com2";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string productDescription = "Description of TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            // Act
            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, productDescription, productPrice, productStock);

            // Assert
            Assert.IsNotNull(newProduct);
            Assert.IsNotNull(newProduct.AddedTime);
        }
        
        [TestMethod]
        public void shouldRetrieveProductById_whenProductExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com3";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string productDescription = "Description of TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, productDescription, productPrice, productStock);
            var productId = newProduct.ProductId;

            // Act
            var retrievedProduct = ibayContext.GetProductById(productId);

            // Assert
            Assert.IsNotNull(retrievedProduct);
            Assert.AreEqual(productId, retrievedProduct.ProductId);
            Assert.AreEqual(productName, retrievedProduct.ProductName);
            Assert.AreEqual(productDescription, retrievedProduct.ProductDescription);
            Assert.AreEqual(productPrice, retrievedProduct.ProductPrice);
            Assert.AreEqual(productStock, retrievedProduct.ProductStock);
            Assert.AreEqual(newUser.UserId, retrievedProduct.SellerId);
            Assert.IsNotNull(retrievedProduct.AddedTime);
        }
        
        [TestMethod]
        public void shouldReturnNull_whenProductIdDoesNotExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentProductId = 999;

            // Act
            var retrievedProduct = ibayContext.GetProductById(nonExistentProductId);

            // Assert
            Assert.IsNull(retrievedProduct);
        }
        
        [TestMethod]
        public void shouldRetrieveProductByName_whenProductExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser123";
            const string sellerEmail = "seller@example.com4";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct123";
            const string productDescription = "Description of TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            ibayContext.CreateProduct(newUser.UserId, productName, productDescription, productPrice, productStock);

            // Act
            var retrievedProduct = ibayContext.GetProductByName(productName);

            // Assert
            Assert.IsNotNull(retrievedProduct);
            Assert.AreEqual(productName, retrievedProduct.ProductName);
            Assert.AreEqual(productDescription, retrievedProduct.ProductDescription);
            Assert.AreEqual(productPrice, retrievedProduct.ProductPrice);
            Assert.AreEqual(productStock, retrievedProduct.ProductStock);
            Assert.AreEqual(newUser.UserId, retrievedProduct.SellerId);
            Assert.IsNotNull(retrievedProduct.AddedTime);
        }
        
        [TestMethod]
        public void shouldReturnNull_whenProductNameDoesNotExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const string nonExistentProductName = "abc";

            // Act
            var retrievedProduct = ibayContext.GetProductByName(nonExistentProductName);

            // Assert
            Assert.IsNull(retrievedProduct);
        }
        
        [TestMethod]
        public void shouldUpdateProductAvailabilitySuccessfully_whenProductIsValid()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com5";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string initialProductDescription = "Description of TestProduct";
            const double initialProductPrice = 99.99;
            const int initialProductStock = 10;

            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, initialProductDescription, initialProductPrice, initialProductStock);
            var productId = newProduct.ProductId;

            // Act
            const string updatedProductName = "UpdatedProduct";
            const string updatedProductDescription = "Updated Description";
            const double updatedProductPrice = 149.99;
            const int updatedProductStock = 20;
            const bool updatedProductAvailability = false;

            var updatedProduct = ibayContext.UpdateProduct(productId, updatedProductName, updatedProductDescription, updatedProductPrice, updatedProductStock, updatedProductAvailability);

            // Assert
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(updatedProductName, updatedProduct.ProductName);
            Assert.AreEqual(updatedProductDescription, updatedProduct.ProductDescription);
            Assert.AreEqual(updatedProductPrice, updatedProduct.ProductPrice);
            Assert.AreEqual(updatedProductStock, updatedProduct.ProductStock);
            Assert.AreEqual(updatedProductAvailability, updatedProduct.Available);
        }
        
        [TestMethod]
        public void shouldNotUpdateProductData_whenNoChanges()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com6";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string initialProductDescription = "Description of TestProduct";
            const double initialProductPrice = 99.99;
            const int initialProductStock = 10;

            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, initialProductDescription, initialProductPrice, initialProductStock);
            var productId = newProduct.ProductId;

            // Act
            var updatedProduct = ibayContext.UpdateProduct(productId, null!, null!, null, null, null);

            // Assert
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(productName, updatedProduct.ProductName);
            Assert.AreEqual(initialProductDescription, updatedProduct.ProductDescription);
            Assert.AreEqual(initialProductPrice, updatedProduct.ProductPrice);
            Assert.AreEqual(initialProductStock, updatedProduct.ProductStock);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductIdNotFoundForUpdate()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const int nonExistentProductId = 999;

            // Act
            ibayContext.UpdateProduct(nonExistentProductId, "UpdatedProduct", "Updated Description", 149.99, 20, false);
        }

        [TestMethod]
        public void shouldUpdateProductUpdatedTime_whenProductIsUpdated()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com7";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string initialProductDescription = "Description of TestProduct";
            const double initialProductPrice = 99.99;
            const int initialProductStock = 10;

            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, initialProductDescription, initialProductPrice, initialProductStock);
            var productId = newProduct.ProductId;

            var initialUpdateTime = newProduct.UpdatedTime;

            // Act
            var updatedProduct = ibayContext.UpdateProduct(productId, "UpdatedProduct", "Updated Description", 149.99, 20, false);

            // Assert
            Assert.IsNotNull(updatedProduct);
            Assert.AreNotEqual(initialUpdateTime, updatedProduct.UpdatedTime);
        }

        [TestMethod]
        public void shouldDeleteProductSuccessfully_whenProductIdExist()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com8";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const string productDescription = "Description of TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var newProduct = ibayContext.CreateProduct(newUser.UserId, productName, productDescription, productPrice, productStock);
            var productId = newProduct.ProductId;

            // Act
            var deletedProduct = ibayContext.DeleteProduct(productId);

            // Assert
            Assert.IsNotNull(deletedProduct);
            Assert.AreEqual(productId, deletedProduct.ProductId);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductIdNotFoundForDelete()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const int nonExistentProductId = 999;

            // Act
            ibayContext.DeleteProduct(nonExistentProductId);
        }

        [TestMethod]
        public void shouldRetrieveProductsOnSaleForUser_whenProductsExists()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com9";
            const string sellerPassword = "test#123TEST";

            var newUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName1 = "Product1";
            const string productName2 = "Product2";
            const double productPrice = 99.99;
            const int productStock = 10;

            ibayContext.CreateProduct(newUser.UserId, productName1, "Description1", productPrice, productStock);
            ibayContext.CreateProduct(newUser.UserId, productName2, "Description2", productPrice, productStock);

            // Act
            var productsOnSale = ibayContext.GetProductsOnSale(newUser.UserId);

            // Assert
            var onSale = productsOnSale as Product[] ?? productsOnSale.ToArray();
            
            Assert.IsNotNull(productsOnSale);
            Assert.AreEqual(2, onSale.Length);
            Assert.IsTrue(onSale.Any(p => p.ProductName == productName1));
            Assert.IsTrue(onSale.Any(p => p.ProductName == productName2));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserNotFoundWhenTryingToGetProductsOnSale()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const int nonExistentUserId = 999;

            // Act
            ibayContext.GetProductsOnSale(nonExistentUserId);
        }
        
        [TestMethod]
        public void shouldBuyCartSuccessfully_whenEverythingIsOk()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com10";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com11";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product1 = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);
            var product2 = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description2", productPrice, productStock);

            ibayContext.UpdateUserMoney(buyerUser.UserId, 2000);

            ibayContext.AddProductToCart(buyerUser.UserId, product1.ProductId, 1);
            ibayContext.AddProductToCart(buyerUser.UserId, product2.ProductId, 1);

            // Act
            var buyerAfterPurchase = ibayContext.BuyCart(buyerUser.UserId);

            // Assert
            Assert.IsNotNull(buyerAfterPurchase);
            Assert.AreEqual(0, buyerUser.UserCart.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserNotFoundForBuyCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            // Act
            ibayContext.BuyCart(nonExistentUserId);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenCartIsEmpty()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com12";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            // Act
            ibayContext.BuyCart(buyerUser.UserId);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductNotAvailable()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com13";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com14";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;
            
            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 1);

            product.Available = false;
            ibayContext.SaveChanges();

            // Act
            ibayContext.BuyCart(buyerUser.UserId);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenNotEnoughMoney()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com15";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com16";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 9999.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 1);
            
            ibayContext.UpdateUserMoney(buyerUser.UserId, 5000);
            
            // Act
            ibayContext.BuyCart(buyerUser.UserId);
        }
        
        [TestMethod]
        public void shouldAddProductToCartSuccessfully_whenProductIsOk()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com17";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com18";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            // Act
            var userAfterAddingProduct = ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 1);

            // Assert
            Assert.IsNotNull(userAfterAddingProduct);
            Assert.AreEqual(1, buyerUser.UserCart.Count);
            Assert.AreEqual(product, buyerUser.UserCart[0]);
            Assert.AreEqual(productStock - 1, product.ProductStock);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserNotFound()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            const int productId = 1;
            const int quantity = 1;

            // Act
            ibayContext.AddProductToCart(nonExistentUserId, productId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductNotFound()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com19";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const int nonExistentProductId = 999;
            const int quantity = 1;

            // Act
            ibayContext.AddProductToCart(buyerUser.UserId, nonExistentProductId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductNotAvailableToAddToCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com20";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com21";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            product.Available = false;
            ibayContext.SaveChanges();

            const int quantity = 1;

            // Act
            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenQuantityIsNotPositiveToAdd()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com22";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com23";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            const int quantity = 0;

            // Act
            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenNotEnoughStock()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com24";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com25";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 1;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            const int quantity = 2;

            // Act
            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, quantity);
        }
        
        [TestMethod]
        public void shouldRemoveProductFromCartSuccessfully_whenProductIsInCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com26";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com27";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 2);

            // Act
            var userAfterRemovingProduct = ibayContext.RemoveProductFromCart(buyerUser.UserId, product.ProductId, 1);

            // Assert
            Assert.IsNotNull(userAfterRemovingProduct);
            Assert.AreEqual(1, buyerUser.UserCart.Count);
            Assert.AreEqual(productStock - 1, product.ProductStock);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserNotFoundForRemovingProductInCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;

            const int productId = 1;
            const int quantity = 1;

            // Act
            ibayContext.RemoveProductFromCart(nonExistentUserId, productId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenProductNotFoundInCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com28";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const int productId = 1;
            const int quantity = 1;

            // Act
            ibayContext.RemoveProductFromCart(buyerUser.UserId, productId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenQuantityIsNotPositiveForRemove()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com29";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com30";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 10;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 2);

            const int quantity = 0;

            // Act
            ibayContext.RemoveProductFromCart(buyerUser.UserId, product.ProductId, quantity);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenNotEnoughStockInCart()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string buyerPseudo = "BuyerUser";
            const string buyerEmail = "buyer@example.com31";
            const string buyerPassword = "test#123TEST";

            var buyerUser = ibayContext.CreateUser(buyerPseudo, buyerEmail, buyerPassword);

            const string sellerPseudo = "SellerUser";
            const string sellerEmail = "seller@example.com32";
            const string sellerPassword = "test#123TEST";

            var sellerUser = ibayContext.CreateUser(sellerPseudo, sellerEmail, sellerPassword);

            const string productName = "TestProduct";
            const double productPrice = 99.99;
            const int productStock = 1;

            var product = ibayContext.CreateProduct(sellerUser.UserId, productName, "Description1", productPrice, productStock);

            ibayContext.AddProductToCart(buyerUser.UserId, product.ProductId, 1);

            const int quantity = 2;

            // Act
            ibayContext.RemoveProductFromCart(buyerUser.UserId, product.ProductId, quantity);
        }
        
        [TestMethod]
        public void shouldUpdateUserMoneySuccessfully_whenAmountIsPositive()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com49";
            const string userPassword = "test#123TEST";

            var user = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            const double initialMoney = 100.0;
            ibayContext.UpdateUserMoney(user.UserId, initialMoney);

            // Act
            var updatedUser = ibayContext.UpdateUserMoney(user.UserId, 50.0);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(initialMoney + 50.0, updatedUser.UserMoney);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenAddingZeroAmount()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const double zeroAmount = 0.0;
            const int userId = 1;

            // Act
            ibayContext.UpdateUserMoney(userId, zeroAmount);
        }
        
        [TestMethod]
        public void shouldUpdateUserMoneySuccessfully_whenAmountIsNegative()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com50";
            const string userPassword = "test#123TEST";

            var user = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            const double initialMoney = 100.0;
            ibayContext.UpdateUserMoney(user.UserId, initialMoney);

            const double negativeAmount = -50.0;

            // Act
            ibayContext.UpdateUserMoney(user.UserId, negativeAmount);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenAddingNegativeAmountWithoutEnoughBalance()
        {
            // Arrange
            var ibayContext = GetMemoryContext();

            const string userPseudo = "TestUser";
            const string userEmail = "test@example.com51";
            const string userPassword = "test#123TEST";

            var user = ibayContext.CreateUser(userPseudo, userEmail, userPassword);

            const double initialMoney = 100.0;
            ibayContext.UpdateUserMoney(user.UserId, initialMoney);

            const double negativeAmount = -200.0;

            // Act
            ibayContext.UpdateUserMoney(user.UserId, negativeAmount);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void shouldThrowValidationException_whenUserNotFoundForUpdatingBalance()
        {
            // Arrange
            var ibayContext = GetMemoryContext();
            const int nonExistentUserId = 999;
            const double amount = 50.0;

            // Act
            ibayContext.UpdateUserMoney(nonExistentUserId, amount);
        }
    }
}
