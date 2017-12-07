using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounts.Controllers;
using Accounts.Controllers.API;
using AccountModel;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;

namespace AccountTesting
{
    [TestClass]
    public class AccountsTests
    {
        UserController UserController;
        ApiController ApiController;

        Mock<AccountContext> MockedContext;
        AccountContext db;

        #region Data

        public Accounts.Models.AuthUser NewAuthUser()
        {
            return new Accounts.Models.AuthUser
            {
                Email = "Joe@email.az",
                ID = "Joe"
            };
        }

        public User NewUser()
        {
            return new User
            {
                Active = true,
                Authorised = true,
                Email = "Joe@email.az",
                Name = "Joe",
                UserID = "Joe",
                Role = "User"
            };
        }

        public User TokenUser()
        {
            return new User
            {
                Active = true,
                Authorised = true,
                Email = "Joe@email.az",
                Name = "Joe",
                UserID = "Test-UserID-String-1",
                Role = "User"
            };
        }


        #endregion Data

        #region Init

        [TestInitialize]
        public void Initialize()
        {
            //Prepare "database"
            MockedContext = new Mock<AccountContext>();
            MockedContext.Setup(u => u.Users).Returns(new MockDbSet<User>().Object);

            db = MockedContext.Object;

            //Get security token
            var token = TokenGen.UserToken("Staff");
            var testClaims = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            //Set up OrderController
            UserController = new UserController(db);
            UserController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            UserController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));

            //Set up Card Controller
            ApiController = new ApiController(db);
            ApiController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            ApiController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));


        }

        #endregion

        #region Tests

        #region API

        #region Get

        [TestMethod]
        public void GetUsersSuccess()
        {
            ApiController.SaveUser(NewUser());
            var response = ApiController.Get();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IEnumerable<User>));
        }

        [TestMethod]
        public void GetUsersDbDown()
        {
            ApiController = new ApiController(null);

            var response = ApiController.Get();

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetUserSuccess()
        {
            ApiController.SaveUser(NewUser());
            var response = ApiController.Get("Joe");

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(User));
        }

        [TestMethod]
        public void GetUserNull()
        {
            ApiController.SaveUser(NewUser());
            var response = ApiController.Get(null);

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetUserDbDown()
        {
            ApiController = new ApiController(null);

            var response = ApiController.Get("Joe");

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetUserNonexistent()
        {
            ApiController.SaveUser(NewUser());
            var response = ApiController.Get("Joey");

            Assert.IsNull(response);
        }

        #endregion Get

        #region Post

        [TestMethod]
        public void SaveUserSuccess()
        {
            var response = (StatusCodeResult)ApiController.SaveUser(NewUser());

            Assert.AreEqual(200, response.StatusCode);
        }

        [TestMethod]
        public void SaveUserNull()
        {
            var response = (StatusCodeResult)ApiController.SaveUser(null);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void SaveUserDbDown()
        {
            ApiController = new ApiController(null);
            var response = (StatusCodeResult)ApiController.SaveUser(NewUser());

            Assert.AreEqual(500, response.StatusCode);
        }

        [TestMethod]
        public void SaveAuthUserSuccess()
        {
            var response = (StatusCodeResult)ApiController.SaveAuthUser(NewAuthUser());

            Assert.AreEqual(200, response.StatusCode);
        }

        [TestMethod]
        public void SaveAuthUserNull()
        {
            var response = (StatusCodeResult)ApiController.SaveAuthUser(null);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void SaveUAuthUserDbDown()
        {
            ApiController = new ApiController(null);
            var response = (StatusCodeResult)ApiController.SaveAuthUser(NewAuthUser());

            Assert.AreEqual(500, response.StatusCode);
        }

        #endregion Post

        #region Delete

        [TestMethod]
        public void DeleteUserSuccess()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)ApiController.Delete("Joe");

            Assert.AreEqual(200, response.StatusCode);
        }

        [TestMethod]
        public void DeleteUserNull()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)ApiController.Delete(null);

            Assert.AreEqual(400, response.StatusCode);

        }

        [TestMethod]
        public void DeleteUserDbDown()
        {
            ApiController.SaveUser(NewUser());

            ApiController = new ApiController(null);

            var response = (StatusCodeResult)ApiController.Delete("Joe");

            Assert.AreEqual(500, response.StatusCode);
        }

        [TestMethod]
        public void DeleteUserNonexistent()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)ApiController.Delete("Joey");

            Assert.AreEqual(400, response.StatusCode);
        }

        #endregion Delete

        #endregion API

        #region Views

        #region Index

        [TestMethod]
        public void IndexSuccess()
        {
            ApiController.SaveUser(NewUser());
            var response = (ViewResult)UserController.Index();

            Assert.IsNotNull(response.Model);
        }

        [TestMethod]
        public void IndexUnauth()
        {
            //Get security token
            var token = TokenGen.UserToken("User");
            var testClaims = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            //Set up OrderController
            UserController = new UserController(db);
            UserController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            UserController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));

            var response = (StatusCodeResult)UserController.Index();

            Assert.AreEqual(403, response.StatusCode);
        }

        [TestMethod]
        public void IndexDbDown()
        {
            UserController = new UserController(null);

            var response = (StatusCodeResult)UserController.Index();
            Assert.AreEqual(500, response.StatusCode);
        }

        #endregion Index

        #region Profile

        [TestMethod]
        public void ProfileSuccess()
        {
            ApiController.SaveUser(NewUser());

            var response = (ViewResult)UserController.Profile("Joe");

            Assert.IsNotNull(response.Model);
            Assert.IsInstanceOfType(response.Model, typeof(User));
        }

        [TestMethod]
        public void ProfileNull()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.Profile(null);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void ProfileNonexistent()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.Profile("Joey");

            Assert.AreEqual(404, response.StatusCode);

        }

        [TestMethod]
        public void ProfileUnauth()
        {
            ApiController.SaveUser(NewUser());

            //Get security token
            var token = TokenGen.UserToken("User");
            var testClaims = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            //Set up OrderController
            UserController = new UserController(db);
            UserController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            UserController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));

            var response = (StatusCodeResult)UserController.Profile("Joe");

            Assert.AreEqual(403, response.StatusCode);
        }

        [TestMethod]
        public void ProfileDbDown()
        {
            ApiController.SaveUser(NewUser());

            UserController = new UserController(null);

            var response = (StatusCodeResult)UserController.Profile("Joe");

            Assert.AreEqual(500, response.StatusCode);
        }
        #endregion Profile

        #region EditProfile

        [TestMethod]
        public void EditProfileSuccess()
        {
            ApiController.SaveUser(TokenUser());

            //Get security token
            var token = TokenGen.UserToken("User");
            var testClaims = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            //Set up OrderController
            UserController = new UserController(db);
            UserController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            UserController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));

            var response = (ViewResult)UserController.EditProfile("Test-UserID-String-1");

            Assert.IsNotNull(response.Model);
        }

        [TestMethod]
        public void EditProfileUnauth()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.EditProfile("Joe");

            Assert.AreEqual(403, response.StatusCode);
        }

        [TestMethod]
        public void EditProfileNull()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.EditProfile(null);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void EditProfileDbDown()
        {
            ApiController.SaveUser(NewUser());

            UserController = new UserController(null);

            var response = (StatusCodeResult)UserController.EditProfile("Joe");

            Assert.AreEqual(500, response.StatusCode);
        }

        [TestMethod]
        public void EditProfilePostSuccess()
        {
            ApiController.SaveUser(TokenUser());

            var response = (RedirectToActionResult)UserController.EditProfilePost(TokenUser());

            Assert.AreEqual("Profile", response.ActionName);
        }

        [TestMethod]
        public void EditProfilePostNull()
        {
            ApiController.SaveUser(TokenUser());

            var response = (StatusCodeResult)UserController.EditProfilePost(null);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void EditProfilePostUnauth()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.EditProfilePost(NewUser());

            Assert.AreEqual(403, response.StatusCode);
        }

        [TestMethod]
        public void EditProfilePostDbDown()
        {
            ApiController.SaveUser(TokenUser());

            UserController = new UserController(null);

            var response = (StatusCodeResult)UserController.EditProfilePost(TokenUser());

            Assert.AreEqual(500, response.StatusCode);
        }

        #endregion EditProfile

        #region ChangeAuth

        [TestMethod]
        public void ChangeAuthSuccess()
        {
            ApiController.SaveUser(NewUser());

            var response = (RedirectToActionResult)UserController.ChangeAuthorisation("Joe").Result;

            Assert.AreEqual("Index",response.ActionName);
        }

        [TestMethod]
        public void ChangeAuthNull()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.ChangeAuthorisation(null).Result;

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void ChangeAuthNonexistent()
        {
            ApiController.SaveUser(NewUser());

            var response = (StatusCodeResult)UserController.ChangeAuthorisation("Joey").Result;

            Assert.AreEqual(404, response.StatusCode);
        }

        [TestMethod]
        public void ChangeAuthUnauth()
        {
            ApiController.SaveUser(NewUser());

            //Get security token
            var token = TokenGen.UserToken("User");
            var testClaims = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            //Set up OrderController
            UserController = new UserController(db);
            UserController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = testClaims
                }
            };
            UserController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + new JwtSecurityTokenHandler().WriteToken(token));

            var response = (StatusCodeResult)UserController.ChangeAuthorisation("Joe").Result;

            Assert.AreEqual(403, response.StatusCode);
        }

        [TestMethod]
        public void ChangeAuthDbDown()
        {
            ApiController.SaveUser(NewUser());

            UserController = new UserController(null);

            var response = (StatusCodeResult)UserController.ChangeAuthorisation("Joe").Result;

            Assert.AreEqual(500, response.StatusCode);
        }

        #endregion ChangeAuth

        #endregion Views

        #endregion Tests
    }
}
