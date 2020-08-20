using DateApp.Controllers;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{


    public class IdentityMocking
    {

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }








    }



    public class HomeControllerTests
    {
        async Task<AppUser> GetUserX()
        {
            return new AppUser() { Id = "1", UserName = "Test User" };
        }

        private List<AppUser> _users = new List<AppUser>()
         {
      new AppUser(){UserName="User1",Id="UserId1" } ,
      new AppUser(){UserName="User2",Id="UserId2" }
         };




        [Test]
        public void When_Number_Is_0_Type_Is_MainPhotoPath()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object);

            PictureType type = controller.GetPictureType("0");

            Assert.AreEqual(PictureType.MainPhotoPath, type);




        }

        [Test]
        public void When_Number_Is_2_Type_Is_PhotoPath2()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object);

            PictureType type = controller.GetPictureType("2");

            Assert.AreEqual(PictureType.PhotoPath2, type);



        }

        [Test]
        public void When_Bool_Is_True_SetShowProfile_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
           
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SetShowProfile(It.IsAny<string>(), true)).Returns(true);

            

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object,GetUserX);

            RedirectToRouteResult result = controller.SetShowProfile(true) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");


        }

        [Test]
        public void When_Bool_Is_False_SetShowProfile_Shows_ErrorView()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.SetShowProfile(It.IsAny<string>(), false)).Returns(false);
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.SetShowProfile(false) as ViewResult;

            Assert.AreEqual(result.ViewName, "Error");


        }

        [Test]
        public void When_Bool_Is_True_SetAge_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);

            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SetSearchAge(It.IsAny<string>(), It.IsAny<int>())).Returns(true);



            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.SetAge(1) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");


        }

        [Test]
        public void When_Bool_Is_False_SetAge_Shows_ErrorView()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.SetSearchAge(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.SetAge(99) as ViewResult;

            Assert.AreEqual(result.ViewName, "Error");


        }

        [Test]
        public void When_Bool_Is_True_SetDistance_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);

            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SetDistance(It.IsAny<string>(), It.IsAny<int>())).Returns(true);



            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.SetDistance(1) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");


        }

        [Test]
        public void When_Bool_Is_False_SetDistance_Shows_ErrorView()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.SetDistance(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.SetDistance(99) as ViewResult;

            Assert.AreEqual(result.ViewName, "Error");


        }

        [Test]
        public void When_Bool_Is_True_SetSearchSex_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);

            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.ChangeSearchSex(It.IsAny<string>(), It.IsAny<string>())).Returns(true);



            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.SetSearchSex("Set") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");


        }

        [Test]
        public void When_Bool_Is_False_SetSearchSex_Shows_ErrorView()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.ChangeSearchSex(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.SetSearchSex("Set") as ViewResult;

            Assert.AreEqual(result.ViewName, "Error");


        }

        [Test]
        public void ChangePhoneNumber_Returns_Model()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.GetPhoneNumber(It.IsAny<string>())).Returns("997");
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.ChangePhoneNumber() as ViewResult;
            ChangePhoneNumberView model = result.Model as ChangePhoneNumberView;

            Assert.AreEqual(model.PhoneNumber , "997");


        }







    }






}