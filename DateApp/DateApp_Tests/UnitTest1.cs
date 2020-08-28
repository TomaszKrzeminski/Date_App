using DateApp.Controllers;
using DateApp.Hubs;
using DateApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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
            return new AppUser() { Id = "Id1", UserName = "Test User" };
        }

        private List<AppUser> _users = new List<AppUser>()
         {
      new AppUser(){UserName="User1",Id="Id1" } ,
      new AppUser(){UserName="User2",Id="Id2" }
         };


        FileStream GetFileStr(string Path)
        {

            return new FileStream(Path, FileMode.Create);

        }

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



            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

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

            Assert.AreEqual(model.PhoneNumber, "997");


        }

        [Test]
        public void ChangePhoneNumber_Redirects_When_Action_Is_Correct()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");


            Mock<IRepository> repo = new Mock<IRepository>();
            repo.Setup(r => r.ChangePhoneNumber("Id1", "997")).Returns(true);
            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);
            ChangePhoneNumberView change = new ChangePhoneNumberView();
            change.PhoneNumber = "997";
            change.UserId = "Id1";

            RedirectToRouteResult result = controller.ChangePhoneNumber(change) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");

        }

        [Test]
        public void When_Details_In_Panel_Are_Null_Redirect()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = null;
            repo.Setup(r => r.GetUserDetails("Id1")).Returns(details);
            repo.Setup(r => r.GetCoordinates("Id1")).Returns(new Coordinates());


            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.Panel() as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");

        }



        [Test]
        public void When_Details_In_Panel_Arent_Null_Return_Model()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails("Empty");
            details.User = new AppUser();
            details.User.UserName = "None";
            details.User.Email = "None";
            details.User.PhoneNumber = "None";
            details.User.Surname = "None";
            Coordinates coordinates = new Coordinates() { Latitude = 9999, Longitude = 8888 };
            repo.Setup(r => r.GetUserDetails("Id1")).Returns(details);
            repo.Setup(r => r.GetCoordinates("Id1")).Returns(coordinates);

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);
            ViewResult result = controller.Panel() as ViewResult;
            PanelViewModel model = result.Model as PanelViewModel;

            Assert.AreEqual(model.settingsmodel.Coordinates.Latitude, "9999.0000000");
            Assert.AreEqual(model.settingsmodel.Coordinates.Longitude, "8888.0000000");

        }


        [Test]
        public void When_ChangeUserDetails_Succes_PictureAdder_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            UserDetailsModel model = new UserDetailsModel() { };

            repo.Setup(r => r.ChangeUserDetails(model)).Returns(true);

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.PictureAdder(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");
        }


        [Test]
        public void When_ChangeUserDetails_Fails_PictureAdder_Shows_Error()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            UserDetailsModel model = null;

            repo.Setup(r => r.ChangeUserDetails(model)).Returns(false);

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.PictureAdder(null) as ViewResult;

            string Text = result.Model.ToString();

            Assert.AreEqual(Text, "Dodanie zmian się nie powiodło");

        }


        [Test]
        public void When_Number_Is_Null_RemovePicture_Shows_Error_With_Message()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();



            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.RemovePicture(null) as ViewResult;

            string Text = result.Model.ToString();

            Assert.AreEqual(Text, "Usunięcie zdjęcia się nie powiodło");

        }

        [Test]
        public void When_Number_Isnt_Null_RemovePicture_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.RemovePicture(It.IsAny<string>(), It.IsAny<PictureType>())).Returns(true);

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.RemovePicture("3") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");

        }




        [Test]
        public void When_File_Is_Null_AddPictureAsync_Redirects()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.RemovePicture(It.IsAny<string>(), It.IsAny<PictureType>())).Returns(true);

            HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.AddPictureAsync(null, "3").Result as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Panel");


        }

        //[Test]
        //public void When_File_Isnt_Null_And_File_Type_Is_JPG_AddPictureAsync_Redirects()
        //{

        //    var file = new Mock<IFormFile>();
        //    var sourceImg = File.OpenRead(@"\Users\tomek\Desktop\Date_App\DateApp\DateApp\wwwroot\Images\QQ.jpg");
        //    var ms = new MemoryStream();
        //    var writer = new StreamWriter(ms);
        //    writer.Write(sourceImg);
        //    writer.Flush();
        //    ms.Position = 0;
        //    var fileName = "QQ.jpg";
        //    file.Setup(f => f.FileName).Returns(fileName).Verifiable();
        //    file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
        //        .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
        //        .Verifiable();

        //    file.Setup(f => f.Length).Returns(10);

        //    var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
        //    var mockEnvironment = new Mock<IHostingEnvironment>();

        //    mockEnvironment
        //        .Setup(m => m.EnvironmentName)
        //        .Returns("Hosting:UnitTestEnvironment");


        //    mockEnvironment.Setup(m => m.WebRootPath).Returns("WebPath");

        //    Mock<IRepository> repo = new Mock<IRepository>();

        //    repo.Setup(r => r.AddPicture(It.IsAny<string>(), It.IsAny<PictureType>(), It.IsAny<string>())).Returns(true);



        //    HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

        //    RedirectToRouteResult result = controller.AddPictureAsync(file.Object, "3").Result as RedirectToRouteResult;
        //    Assert.AreEqual(result.RouteValues["action"], "Panel");


        //}

        //[Test]
        //public void When_File_Isnt_Null_But_File_Type_Isnt_JPG_AddPictureAsync_Redirects()
        //{

        //    var file = new Mock<IFormFile>();
        //    var sourceImg = File.OpenRead(@"\Users\tomek\Desktop\Date_App\DateApp\DateApp\wwwroot\Images\testPNG.png");
        //    var ms = new MemoryStream();
        //    var writer = new StreamWriter(ms);
        //    writer.Write(sourceImg);
        //    writer.Flush();
        //    ms.Position = 0;
        //    var fileName = "QQ.png";
        //    file.Setup(f => f.FileName).Returns(fileName).Verifiable();
        //    file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
        //        .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
        //        .Verifiable();



        //    var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
        //    var mockEnvironment = new Mock<IHostingEnvironment>();

        //    mockEnvironment
        //        .Setup(m => m.EnvironmentName)
        //        .Returns("Hosting:UnitTestEnvironment");

        //    Mock<IRepository> repo = new Mock<IRepository>();

        //    repo.Setup(r => r.AddPicture(It.IsAny<string>(), It.IsAny<PictureType>(),It.IsAny<string>())).Returns(true);

        //    HomeController controller = new HomeController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

        //    ViewResult result = controller.AddPictureAsync(file.Object,"3").Result as ViewResult;

        //    string Text = result.Model.ToString();

        //    Assert.AreEqual(Text, "Zdjęcie musi być w formacie jpg");


        //}


    }

    public class PairControllerTests
    {
        async Task<AppUser> GetUserX()
        {
            return new AppUser() { Id = "Id1", UserName = "Test User" };
        }


        private List<AppUser> _users = new List<AppUser>()
         {
      new AppUser(){UserName="User1",Id="Id1" } ,
      new AppUser(){UserName="User2",Id="Id2" }
         };

        [Test]
        public void When_Exception_Is_Thrown_getValue_returns_0()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();
            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            double value = controller.getValue("xxx");

            Assert.AreEqual(value, 0);




        }

        [Test]
        public void When_getValue_Changes_String_To_Value_Returns_Double()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");
            Mock<IRepository> repo = new Mock<IRepository>();
            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            double value = controller.getValue("100.9");

            Assert.AreEqual(value, 100.9);


        }


        [Test]
        public void When_ReportUser_Is_True_PairReport_Redirects_To_Pair()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.ReportUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToActionResult result = controller.PairReport("Reason", "UserId") as RedirectToActionResult;
            Assert.AreEqual(result.ActionName, "GoToPair");


        }

        [Test]
        public void When_ReportUser_Is_False_PairReport_Redirects_To_Error()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.ReportUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);


            ViewResult result = controller.PairReport("Reason", "UserId") as ViewResult;

            string Text = result.Model.ToString();

            Assert.AreEqual(Text, "Problem nie można zgłosić użytkownika");

        }


        [Test]
        public void When_PairCancel_Is_True_PairCancel_Redirects_To_Pair()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.PairCancel(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.PairCancel("Reason") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "PairPanel");


        }


        [Test]
        public void When_PairCancel_Ends_False_PairCancel_Shows_ErrorMessage()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.PairCancel(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.PairCancel("Reason") as ViewResult;

            string Text = result.Model.ToString();

            Assert.AreEqual(Text, "Problem z usunięciem pary");


        }

        [Test]
        public void When_SaveCoordinates_Dont_Fail_Redirects_In_Coordinates()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SaveCoordinates(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.Coordinates("10", "10") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "PairPanel");


        }

        [Test]
        public void When_SaveCoordinates_Fails_Returns_Error_MessageIn_Coordinates()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SaveCoordinates(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.Coordinates("10", "10") as ViewResult;

            string Text = result.Model.ToString();

            Assert.AreEqual(Text, "Zapisanie lokalizacji nie powiodło się sprawdz przeglądarkę");




        }


        [Test]
        public void If_GetMatchViews_Returns_Null_Than_ShowNextMatch_Returns_Empty_Model()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();


            MatchAction action = new MatchAction() { };
            MatchView match = null;
            List<MatchView> list = new List<MatchView>();
            list.Add(match);


            repo.Setup(r => r.MatchAction2(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(action);
            repo.Setup(r => r.GetMatchViews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(list);



            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            PartialViewResult result = controller.ShowNextMatch("10", "Cancel") as PartialViewResult;

            PairPartialViewModel model = (PairPartialViewModel)result.Model;

            Assert.AreEqual(model.match.PairMail, "");
            Assert.AreEqual(model.match.PairId, "");
            Assert.AreEqual(model.match.PairMainPhotoPath, "");

        }


        [Test]
        public void If_GetMatchViews_Returns_Object_Than_ShowNextMatch_Returns_Not_Empty_Model()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            Coordinates c1 = new Coordinates() { Latitude = 10.5, Longitude = 8.5 };
            MatchAction action = new MatchAction() { };
            MatchView match = new MatchView() { PairId = "2" };
            List<MatchView> list = new List<MatchView>();
            list.Add(match);


            repo.Setup(r => r.MatchAction2(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(action);
            repo.Setup(r => r.GetMatchViews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(list);
            repo.Setup(r => r.GetCoordinates(It.IsAny<string>())).Returns(c1);


            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            PartialViewResult result = controller.ShowNextMatch("10", "Cancel") as PartialViewResult;

            PairPartialViewModel model = (PairPartialViewModel)result.Model;

            Assert.AreEqual(model.match.PairId, "2");


        }

        [Test]
        public void UpdateMatches_Returns_Model()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();


            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPath" };
            AppUser user = new AppUser() { UserName = "UserName", Surname = "Surname" };
            MatchView matchView = new MatchView() { };
            MatchView matchView1 = new MatchView() { };
            MatchView matchView2 = new MatchView() { };
            MatchView matchView3 = new MatchView() { };
            List<MatchView> listMatch = new List<MatchView>();
            listMatch.Add(matchView);
            listMatch.Add(matchView1);
            listMatch.Add(matchView2);
            listMatch.Add(matchView3);


            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetMatchViews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(listMatch);


            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            PartialViewResult result = controller.UpdateMatches() as PartialViewResult;

            PairOptionsViewModel model = (PairOptionsViewModel)result.Model;

            Assert.AreEqual(model.list.Count, 4);
            Assert.AreEqual(model.UserMainPhotoPath, "MainPath");
            Assert.AreEqual(model.UserName, "UserName Surname");


        }

        [Test]
        public void If_GoToPair_details_Are_Null_Than_Redirects()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();


            SearchDetails details = null;
            AppUser user = new AppUser() { Id = "PairId1", UserName = "UserName", Surname = "Surname" };

            repo.Setup(r => r.GetUserDetails("PairId1")).Returns(details);
            repo.Setup(r => r.GetUser("PairId1")).Returns(user);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.GoToPair("PairId1") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "PairPanel");
            Assert.AreEqual(result.RouteValues["controller"], "Pair");

        }

        [Test]
        public void If_GoToPair_details_Arent_Null_Than_Function_Returns_Model()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();


            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPath" };
            AppUser user = new AppUser() { Id = "PairId1", UserName = "UserName", Surname = "Surname" };

            repo.Setup(r => r.GetUserDetails("PairId1")).Returns(details);
            repo.Setup(r => r.GetUser("PairId1")).Returns(user);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.GoToPair("PairId1") as ViewResult;

            PairDetailsViewModel model = (PairDetailsViewModel)result.Model;

            Assert.AreEqual(model.Surname, "Surname");
            Assert.AreEqual(model.MainPhotoPath, "MainPath");


        }


        [Test]
        public void When_select_Equals_Pair_In_PairPanel_PairOptions_arent_null()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();


            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPath" };
            AppUser user = new AppUser() { Id = "PairId1", UserName = "UserName", Surname = "Surname" };
            List<DateApp.Models.Match> list = new List<DateApp.Models.Match>();
            DateApp.Models.Match match = new DateApp.Models.Match() { };
            list.Add(match);
            List<MatchView> listViews = new List<MatchView>();
            MatchView matchView = new MatchView();
            listViews.Add(matchView);
            Coordinates coordinates = new Coordinates() { Latitude = 10.5, Longitude = 10.5 };

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetMatches(It.IsAny<string>())).Returns(list);
            repo.Setup(r => r.SearchForMatches(It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetMatchViews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(listViews);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetCoordinates(It.IsAny<string>())).Returns(coordinates);



            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.PairPanel("Pair") as ViewResult;

            PairViewModel model = (PairViewModel)result.Model;


            Assert.NotNull(model.pairoptions.UserName);






        }

        [Test]
        public void When_select_isnt_Pair_In_PairPanel_MessageOptions_arent_null()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails userSearchDetails = new SearchDetails() { MainPhotoPath = "PhotoPath", User = new AppUser() { Id = "101", UserName = "UserName", Surname = "Surname" } };
            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPath" };
            AppUser user = new AppUser() { Id = "PairId1", UserName = "UserName", Surname = "Surname" };
            List<Message> list = new List<Message>();
            Message message1 = new Message() { MessageText = "MessageText", Time = new System.DateTime(2020, 8, 6), SenderId = "1", ReceiverId = "2" };
            Message message2 = new Message() { Time = new System.DateTime(2020, 8, 7), SenderId = "2", ReceiverId = "2" };
            Message message3 = new Message() { Time = new System.DateTime(2020, 8, 8), SenderId = "3", ReceiverId = "2" };
            Message message4 = new Message() { Time = new System.DateTime(2020, 8, 9), SenderId = "4", ReceiverId = "2" };
            list.Add(message1);
            list.Add(message2);
            list.Add(message3);
            list.Add(message4);



            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetAllMessages(It.IsAny<string>())).Returns(list);
            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(userSearchDetails);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            ViewResult result = controller.PairPanel("") as ViewResult;

            PairViewModel model = (PairViewModel)result.Model;


            Assert.NotNull(model.messageOptionsview.UserName);

        }


    }

    public class MessageControllerTests
    {

        async Task<AppUser> GetUserX()
        {
            return new AppUser() { Id = "Id1", UserName = "Test User" };
        }


        private List<AppUser> _users = new List<AppUser>()
         {
      new AppUser(){UserName="User1",Id="Id1" } ,
      new AppUser(){UserName="User2",Id="Id2" }
         };


        [Test]
        public void Method_WriteMessage_Returns_Partial_WriteMessage()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);

            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            PartialViewResult result = controller.WriteMessage("ReceiverId") as PartialViewResult;


            string partialName = result.ViewName;

            Assert.AreEqual(partialName, "WriteMessage");




        }


        [Test]
        public void CheckOffLine_Calls_UpdateChatList_Remove_Once()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients2.Setup(clients => clients.All).Returns(mockClientProxy2.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub2.Setup(x => x.Clients).Returns(() => mockClients2.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);

            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            controller.CheckOffline();


            mockClients2.Verify(clients => clients.All, Times.Once);

            mockClientProxy2.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                   "UpdateChatList_Remove", It.Is<object[]>(o => o != null),
                    default(CancellationToken)), Times.Once);






        }



        [Test]
        public void CheckOnLine_Calls_UpdateChatList_Add_Twice()
        {
            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients2.Setup(clients => clients.All).Returns(mockClientProxy2.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub2.Setup(x => x.Clients).Returns(() => mockClients2.Object);

            hub2.Setup(x => x.Clients.User(It.IsAny<string>())).Returns(mockClientProxy2.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);

            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            OnlineObject Object = new OnlineObject();
            List<string> names = new List<string>() { "Name1", "Name2", "Name3" };

            Object.names = names;
            Object.ChatUserId = "10";
            Object.UserId = "20";


            controller.CheckOnline(Object);


            mockClientProxy2.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                   "UpdateChatList_Add", It.Is<object[]>(o => o != null),
                    default(CancellationToken)), Times.AtLeast(2));


        }


        [Test]
        public void SelectPage_Returns_Partial_WriteMessage()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };

            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            PartialViewResult result = controller.SelectPage("Next", "1", "ReceiverId") as PartialViewResult;


            string partialName = result.ViewName;

            Assert.AreEqual(partialName, "WriteMessage");


        }

        [Test]
        public void RefreshReceivers_Returns_Model_Count_3()
        {


            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };

            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);



            List<Message> listMes = new List<Message>();
            Message message1 = new Message() { MessageText = "Text Message 1", MessageId = 1, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId1", ReceiverId = "ReceiverId1" };
            Message message2 = new Message() { MessageText = "Text Message 2", MessageId = 2, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId2", ReceiverId = "ReceiverId2" };
            Message message3 = new Message() { MessageText = "Text Message 3", MessageId = 3, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId3", ReceiverId = "ReceiverId3" };

            listMes.Add(message1);
            listMes.Add(message2);
            listMes.Add(message3);



            AppUser user = new AppUser() { Surname = "Surname", UserName = "UserName", Id = "UserId" };

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetAllMessages(It.IsAny<string>())).Returns(listMes);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            PartialViewResult result = controller.RefreshReceivers("ReceiverId") as PartialViewResult;


            List<MessageShort> model = (List<MessageShort>)result.Model;


            Assert.AreEqual(model.Count, 3);


        }


        [Test]
        public void SendMessage_Returns_Model_With_MessageId()
        {


            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();
            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);
            hub.Setup(x => x.Clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);



            var mockClientProxy2 = new Mock<IClientProxy>();
            var mockClients2 = new Mock<IHubClients>();
            mockClients2.Setup(clients => clients.All).Returns(mockClientProxy2.Object);
            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub2.Setup(x => x.Clients).Returns(() => mockClients2.Object);



            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };

            List<Message> list = new List<Message>();
            Message message0 = new Message();

            list.Add(message0);



            List<Message> listMes = new List<Message>();
            Message message1 = new Message() { MessageText = "Text Message 1", MessageId = 1, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId1", ReceiverId = "ReceiverId1" };
            Message message2 = new Message() { MessageText = "Text Message 2", MessageId = 2, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId2", ReceiverId = "ReceiverId2" };
            Message message3 = new Message() { MessageText = "Text Message 3", MessageId = 3, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId3", ReceiverId = "ReceiverId3" };

            listMes.Add(message1);
            listMes.Add(message2);
            listMes.Add(message3);



            AppUser user = new AppUser() { Surname = "Surname", UserName = "UserName", Id = "UserId" };

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetAllMessages(It.IsAny<string>())).Returns(listMes);


            repo.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            Message message = new Message() { MessageText = "Text Message 3", MessageId = 3, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId3", ReceiverId = "ReceiverId3" };

            PartialViewResult result = controller.SendMessage(message) as PartialViewResult;


            MessageViewModel model = (MessageViewModel)result.Model;


            Assert.AreEqual(model.message.ReceiverId, "ReceiverId3");


        }

        [Test]
        public void SendMessage_Fires_UpdateChat_Users_And_UpdateChat_WriteMessage_Once()
        {


            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();
            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);
            hub.Setup(x => x.Clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);



            var mockClientProxy2 = new Mock<IClientProxy>();
            var mockClients2 = new Mock<IHubClients>();
            mockClients2.Setup(clients => clients.All).Returns(mockClientProxy2.Object);
            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub2.Setup(x => x.Clients).Returns(() => mockClients2.Object);



            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };

            List<Message> list = new List<Message>();
            Message message0 = new Message();

            list.Add(message0);



            List<Message> listMes = new List<Message>();
            Message message1 = new Message() { MessageText = "Text Message 1", MessageId = 1, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId1", ReceiverId = "ReceiverId1" };
            Message message2 = new Message() { MessageText = "Text Message 2", MessageId = 2, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId2", ReceiverId = "ReceiverId2" };
            Message message3 = new Message() { MessageText = "Text Message 3", MessageId = 3, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId3", ReceiverId = "ReceiverId3" };

            listMes.Add(message1);
            listMes.Add(message2);
            listMes.Add(message3);



            AppUser user = new AppUser() { Surname = "Surname", UserName = "UserName", Id = "UserId" };

            repo.Setup(r => r.GetUserDetails(It.IsAny<string>())).Returns(details);
            repo.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);
            repo.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
            repo.Setup(r => r.GetAllMessages(It.IsAny<string>())).Returns(listMes);


            repo.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            Message message = new Message() { MessageText = "Text Message 3", MessageId = 3, Checked = false, Time = new System.DateTime(2020, 8, 25), SenderId = "SenId3", ReceiverId = "ReceiverId3" };

            PartialViewResult result = controller.SendMessage(message) as PartialViewResult;


            mockClientProxy.Verify(
               clientProxy => clientProxy.SendCoreAsync(
                  "UpdateChat_WriteMessage", It.Is<object[]>(o => o != null), default(CancellationToken)), Times.AtLeast(1));

            mockClientProxy.Verify(
               clientProxy => clientProxy.SendCoreAsync(
                  "UpdateChat_Users", It.IsAny<object[]>(), default(CancellationToken)), Times.AtLeast(1));

        }


        [Test]
        public void MessageStart_Redirects_To_PairPanel_Action()
        {



            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            List<Message> list = new List<Message>();
            Message message = new Message();

            list.Add(message);

            repo.Setup(r => r.StartChat(It.IsAny<string>(), "Id")).Returns(true);
           

            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);

            RedirectToRouteResult result = controller.MessageStart("Id") as RedirectToRouteResult;


           

            Assert.AreEqual(result.RouteValues["action"], "PairPanel");










        }

        [Test]
        public void SettingMessageView_Returns_Two_TotalPages_When_There_Are_6_Messages()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            Message message = new Message();
            Message message1 = new Message();
            Message message2 = new Message();
            Message message3 = new Message();
            Message message4 = new Message();
            Message message5 = new Message();


            List<Message> list = new List<Message>() {message,message1,message2,message3,message4,message5 };
           

            

            repo.Setup(r => r.StartChat(It.IsAny<string>(), "Id")).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);
            MessageViewModel result = controller.SettingMessageView("x", "x", "x", "x",new SearchDetails() {User=new AppUser() {UserName="Username",Email="Email" },MainPhotoPath="Path" },true ) as MessageViewModel;

            Assert.AreEqual(result.info.TotalPages,2);



        }



        [Test]
        public void SettingMessageView_Sets_TotalPages_As_CurrentPage_When_setLastPage_Is_True()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                        .Returns("Hosting:UnitTestEnvironment");

            var mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub = new Mock<IHubContext<MessageHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);




            var mockClientProxy2 = new Mock<IClientProxy>();

            var mockClients2 = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var hub2 = new Mock<IHubContext<CheckConnectionHub>>();
            hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

            Mock<IRepository> repo = new Mock<IRepository>();

            SearchDetails details = new SearchDetails() { MainPhotoPath = "MainPhotoPath", User = new AppUser() { UserName = "UserName", Surname = "Surname", Id = "Id" } };
            Message message = new Message();
            Message message1 = new Message();
            Message message2 = new Message();
            Message message3 = new Message();
            Message message4 = new Message();
            Message message5 = new Message();


            List<Message> list = new List<Message>() { message, message1, message2, message3, message4, message5 };




            repo.Setup(r => r.StartChat(It.IsAny<string>(), "Id")).Returns(true);
            repo.Setup(r => r.GetChat(It.IsAny<string>(), It.IsAny<string>())).Returns(list);


            MessageController controller = new MessageController(repo.Object, userManager.Object, mockEnvironment.Object, hub.Object, hub2.Object, GetUserX);
            MessageViewModel result = controller.SettingMessageView("x", "x", "x", "x", new SearchDetails() { User = new AppUser() { UserName = "Username", Email = "Email" }, MainPhotoPath = "Path" }, true) as MessageViewModel;

            int totalPages = result.info.TotalPages;
            int currentPage = result.info.CurrentPage;

            Assert.AreEqual(totalPages,currentPage);



        }




    }

    public class AccountControllerTests
    {

        public class FakeUserManager : UserManager<AppUser>
        {
            public FakeUserManager()
                : base(new Mock<IUserStore<AppUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<AppUser>>().Object,
                  new IUserValidator<AppUser>[0],
                  new IPasswordValidator<AppUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<AppUser>>>().Object)
            { }

            public override Task<IdentityResult> CreateAsync(AppUser user, string password)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public override Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public override Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
            {
                return Task.FromResult(Guid.NewGuid().ToString());
            }

        }



        public class FakeSignInManager : SignInManager<AppUser>
        {
            public FakeSignInManager()
                    : base(new FakeUserManager(),
                         new Mock<IHttpContextAccessor>().Object,
                         new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                         new Mock<IOptions<IdentityOptions>>().Object,
                         new Mock<ILogger<SignInManager<AppUser>>>().Object,
                         new Mock<IAuthenticationSchemeProvider>().Object)
            { }
        }




        public class FakeSignInResult: Microsoft.AspNetCore.Identity.SignInResult
        {

           

            public FakeSignInResult()
            {
                Succeeded = true;
               
            }


        }




        async Task<AppUser> GetUserX()
        {
            return new AppUser() { Id = "Id1", UserName = "Test User" };
        }

        private List<AppUser> _users = new List<AppUser>()
         {
      new AppUser(){UserName="User1",Id="Id1" } ,
      new AppUser(){UserName="User2",Id="Id2" }
         };

        [Test]
        public void Login_Sets_ViewBag_To_ReturnUrl()
        {

            Mock<FakeSignInManager> sign = new Mock<FakeSignInManager>();
            Mock<FakeUserManager> manager = new Mock<FakeUserManager>();

            AccountController controller = new AccountController(manager.Object, sign.Object);

            ViewResult result = controller.Login("ReturnUrl") as ViewResult;


            Assert.AreEqual("ReturnUrl", result.ViewData["returnUrl"]);


        }

        [Test]
        public void LogOut_Redirects_To_Login_Action()
        {
            Mock<FakeSignInManager> sign = new Mock<FakeSignInManager>();
            Mock<FakeUserManager> manager = new Mock<FakeUserManager>();           

            AccountController controller = new AccountController(manager.Object, sign.Object);          


            var Result = controller.Logout().Result as RedirectToActionResult ;

            Assert.AreEqual(Result.ActionName, "Login");
            Assert.AreEqual(Result.ControllerName, "Account");

        }


        [Test]
        public void Login_With_LoginModel_Redirects_When_User_Isnt_Null()
        {
            LoginModel model = new LoginModel() { Email = "Email", Password = "password" };
            Mock<FakeSignInManager> sign = new Mock<FakeSignInManager>();
            Mock<FakeUserManager> manager = new Mock<FakeUserManager>();

            AppUser user = new AppUser() { UserName = "Name", Email = "Email" };

            FakeSignInResult result = new FakeSignInResult();
                      

            manager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            sign.Setup(s => s.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((Microsoft.AspNetCore.Identity.SignInResult)result));


            AccountController controller = new AccountController(manager.Object, sign.Object);


            var Result = controller.Login(model).Result as RedirectToRouteResult ;


            Assert.AreEqual(Result.RouteValues["action"], "Panel");
            Assert.AreEqual(Result.RouteValues["controller"], "Home");



        }


        [Test]
        public void Login_With_LoginModel_Returns_Model_When_ModelState_Is_Not_Valid()
        {

            LoginModel model = new LoginModel() { Email = "Email", Password = "password" };
            Mock<FakeSignInManager> sign = new Mock<FakeSignInManager>();
            Mock<FakeUserManager> manager = new Mock<FakeUserManager>();
            AppUser user = null;          
            
            manager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));         


            AccountController controller = new AccountController(manager.Object, sign.Object);


            var Result = controller.Login(model).Result as ViewResult;

            LoginModel login = (LoginModel)Result.Model;
           

            Assert.AreEqual(login.Email,model.Email);
            Assert.AreEqual(login.Password, model.Password);
        }





    }







}