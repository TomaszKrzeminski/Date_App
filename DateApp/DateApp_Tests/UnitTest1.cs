using DateApp.Controllers;
using DateApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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

            RedirectToActionResult result = controller.PairReport("Reason","UserId") as RedirectToActionResult;
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
        public void When_SaveCoordinates_Dont_Fail_Redirects()
        {

            var userManager = IdentityMocking.MockUserManager<AppUser>(_users);
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment
                .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

            Mock<IRepository> repo = new Mock<IRepository>();

            repo.Setup(r => r.SaveCoordinates(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            PairController controller = new PairController(repo.Object, userManager.Object, mockEnvironment.Object, GetUserX);

            RedirectToRouteResult result = controller.Coordinates("10","10") as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "PairPanel");


        }

        [Test]
        public void When_SaveCoordinates_Fails_Returns_Error_Message()
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




    }












}