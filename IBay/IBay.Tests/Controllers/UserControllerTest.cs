using IBay.Controllers;
using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace IBay.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void Create_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<IIbayContext>();
            var controller = new UserController(mockContext.Object);

            // Act
            var result = controller.Create("testPseudo", "testEmail", "testPassword");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Get_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<IIbayContext>();
            mockContext.Setup(c => c.GetUserById(It.IsAny<int>())).Returns(new User());
            var controller = new UserController(mockContext.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Get_ReturnsNotFoundResult()
        {
            // Arrange
            var mockContext = new Mock<IIbayContext>();
            mockContext.Setup(c => c.GetUserById(It.IsAny<int>())).Returns((User)null);
            var controller = new UserController(mockContext.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // Ajoutez des méthodes de test similaires pour les autres actions du contrôleur UserController.
    }
}
