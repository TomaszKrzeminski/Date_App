using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DateApp.Models;
using System.Security.Claims;

namespace UndergroundSound.Models

{
    public static class SeedData
    {

        //public static void EnsurePopulated(IApplicationBuilder app)
        public static void EnsurePopulated(AppIdentityDbContext context)
        {


            context.Database.EnsureCreated();




            void SeedRoles()
            {




                try
                {
                   

                   







                    var roleStore = new RoleStore<IdentityRole>(context);

                    if (!context.Roles.Any(r => r.Name == "Administrator"))
                    {

                        roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
                    }



                    if (!context.Roles.Any(r => r.Name == "UserRole"))
                    {

                        roleStore.CreateAsync(new IdentityRole { Name = "UserRole", NormalizedName = "UserRole" });
                    }
                   

                   




                    context.SaveChanges();



                }
                catch (Exception ex)
                {

                }
            }



            


            void SeedAdmin(string Name, string Surname, string Sex, string City, string Email, DateTime Dateofbirth)
            {


                try
                {
                    DateTime Now = DateTime.Now;
                    TimeSpan ts = Now - Dateofbirth;
                    int age = ts.Days / 365;

                    var User = new AppUser(Sex)
                    {
                        Age = age,
                        Email = Email,
                        FirstName=Name,
                        Surname = Surname,
                        //Sex = Sex,
                        City = City,
                        Dateofbirth = Dateofbirth,
                        UserName = Email,
                        EmailConfirmed = false,
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = Email.ToUpper(),
                        NormalizedUserName = Email.ToUpper(),
                    };

                    if (!context.Users.Any(u => u.UserName == User.UserName))
                    {
                        var password = new PasswordHasher<AppUser>();
                        var hashed = password.HashPassword(User, "Sekret123@");
                        User.PasswordHash = hashed;
                        UserStore<AppUser> userStore;

                        userStore = new UserStore<AppUser>(context);

                        userStore.CreateAsync(User).Wait();
                        ////////
                        Claim claim = new Claim(ClaimTypes.Email, User.Email);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(claim);
                        userStore.AddClaimsAsync(User, claims);
                        userStore.AddToRoleAsync(User, "Administrator").Wait();

                    }
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }






            }








            void SeedUser( string Name,string Surname,string Sex,string City,string Email,DateTime Dateofbirth )
            {


                try
                {
                    DateTime Now = DateTime.Now;
                    TimeSpan ts = Now - Dateofbirth;
                    int age = ts.Days / 365;

                    var User = new AppUser(Sex)
                    {   
                        Age=age,
                        FirstName =Name,
                        Email=Email,
                        Surname = Surname,
                        //Sex = Sex,
                        City = City,
                        Dateofbirth =Dateofbirth,
                        UserName = Email,                                               
                        EmailConfirmed = false,
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = Email.ToUpper(),
                        NormalizedUserName = Email.ToUpper(),
                    };

                    if (!context.Users.Any(u => u.UserName == User.UserName))
                    {
                        var password = new PasswordHasher<AppUser>();
                        var hashed = password.HashPassword(User, "Sekret123@");
                        User.PasswordHash = hashed;
                        UserStore<AppUser> userStore;

                        userStore = new UserStore<AppUser>(context);

                        //userStore.CreateAsync(User).Wait();
                        userStore.CreateAsync(User).Wait();
                        ////////
                        Claim claim = new Claim(ClaimTypes.Email, User.Email);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(claim);
                        userStore.AddClaimsAsync(User, claims).Wait();
                        userStore.AddToRoleAsync(User, "UserRole").Wait();

                    }
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }






            }



          



            void AddNotificationCheckToUser(string UserEmail)
            {

                try
                {
                    AppUser user = context.Users.Include(s => s.Notification).Where(u => u.Email == UserEmail).First();
                    NotificationCheck notification = new NotificationCheck(false, DateTime.Now);
                    user.Notification = notification;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }



            }

            void AddNotificationCheckToAdmin(string UserEmail)
            {

                try
                {
                    AppUser user = context.Users.Include(s => s.Notification).Where(u => u.Email == UserEmail).First();
                    NotificationCheck notification = new NotificationCheck(true, new DateTime(2100,12,12));
                    user.Notification = notification;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }



            }




            void AddCoordinatesToUser(string UserEmail,double Latitude,double Longitude)
            {
                try
                {
                    AppUser user = context.Users.Where(u => u.Email == UserEmail).First();
                    Coordinates c = new Coordinates(user.Id,Longitude,Latitude);                    
                    user.coordinates = c;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }
            }


       




        void AddEvents(string EventName,string City,string ZipCode,string Latitude,string Longitude,string UserEmail,DateTime time=new DateTime())
        {

            try
            {
                    Event Event = new Event();
                    Event.EventName = EventName;
                    Event.City = City;
                    Event.Longitude = Longitude;
                    Event.Latitude = Latitude;
                    Event.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
                    Event.Date = time;
                    Event.ZipCode = ZipCode;
                    Event.OrganizerEmail = UserEmail;

                    AppUser user= context.Users.Where(u => u.Email == UserEmail).First();
                   

                    EventUser eventUser = new EventUser();
                    eventUser.Event = Event;
                    eventUser.AppUser = user;

                    user.EventUser.Add(eventUser);
                    context.SaveChanges();


                }
            catch (Exception ex)
            {

            }



        }



            if (!context.Users.Any())
            {

                SeedRoles();
               
               SeedAdmin("ADMIN", "ADMIN", "Mężczyzna", "Świecie", "ADMIN@gmail.com", new DateTime(1985, 8, 21));

                SeedUser("Tomek", "Kowalski", "Mężczyzna", "Świecie", "zdalnerepo1985@gmail.com", new DateTime(1985,8,21));
                SeedUser("Ada", "Kowalska", "Kobieta", "Świecie", "U2@gmail.com", new DateTime(1985, 8, 21));
                SeedUser("Janusz", "Świerczyński", "Mężczyzna", "Świecie", "U3@gmail.com", new DateTime(1950, 8, 21));
                SeedUser("Martyna", "Kawka", "Kobieta", "Bydgoszcz", "U4@gmail.com", new DateTime(1990, 8, 21));
                SeedUser("Jacek", "Szmelter", "Mężczyzna", "Chełmno", "U5@gmail.com", new DateTime(1985, 8, 21));
                SeedUser("Mariusz", "Brown", "Mężczyzna", "Chełmno", "U6@gmail.com", new DateTime(1975, 8, 21));
                SeedUser("Adek", "Teller", "Mężczyzna", "Chełmno", "U7@gmail.com", new DateTime(1980, 8, 21));
                SeedUser("Lolek", "Goldman", "Mężczyzna", "Bydgoszcz", "U8@gmail.com", new DateTime(1990, 8, 21));
                SeedUser("Weronika", "Rossati", "Kobieta", "Świecie", "U9@gmail.com", new DateTime(1999, 8, 21));
                SeedUser("Ania", "Przybylska", "Kobieta", "Bydgoszcz", "U10@gmail.com", new DateTime(2001, 8, 21));
                SeedUser("Karolina", "Świerczyński", "Kobieta", "Świecie", "U11@gmail.com", new DateTime(1950, 8, 21));
                SeedUser("Kasia", "Przybylska", "Kobieta", "Bydgoszcz", "U12@gmail.com", new DateTime(1992, 8, 21));



                AddCoordinatesToUser("ADMIN@gmail.com", 00.000, 00.000);
                AddCoordinatesToUser("zdalnerepo1985@gmail.com", 53.409479, 18.442148);
                AddCoordinatesToUser("U2@gmail.com", 53.408353, 18.414253);
                AddCoordinatesToUser("U3@gmail.com", 53.405232, 18.406958);
                AddCoordinatesToUser("U4@gmail.com", 53.116472, 18.007548);
                AddCoordinatesToUser("U5@gmail.com", 53.349537, 18.423952);
                AddCoordinatesToUser("U6@gmail.com", 53.347993, 18.418282);
                AddCoordinatesToUser("U7@gmail.com", 53.351428, 18.432541);
                AddCoordinatesToUser("U8@gmail.com", 53.131409, 18.018191);
                AddCoordinatesToUser("U9@gmail.com", 53.416999, 18.458456);
                AddCoordinatesToUser("U10@gmail.com", 53.125177, 18.067801);
                AddCoordinatesToUser("U11@gmail.com", 53.416999, 18.458456);
                AddCoordinatesToUser("U12@gmail.com", 53.140677, 18.028920);



                AddNotificationCheckToAdmin("ADMIN@gmail.com");
                AddNotificationCheckToUser("zdalnerepo1985@gmail.com");
                AddNotificationCheckToUser("U2@gmail.com");
                AddNotificationCheckToUser("U3@gmail.com");
                AddNotificationCheckToUser("U4@gmail.com");
                AddNotificationCheckToUser("U5@gmail.com");
                AddNotificationCheckToUser("U6@gmail.com");
                AddNotificationCheckToUser("U7@gmail.com");
                AddNotificationCheckToUser("U8@gmail.com");
                AddNotificationCheckToUser("U9@gmail.com");
                AddNotificationCheckToUser("U10@gmail.com");
                AddNotificationCheckToUser("U11@gmail.com");
                AddNotificationCheckToUser("U12@gmail.com");

                AddEvents("Wydarzenie 1", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 11));
                AddEvents("Wydarzenie 2", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 12));
                AddEvents("Wydarzenie 3", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 13));
                AddEvents("Wydarzenie 4", "Chełmno", "86-200", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 14));
                AddEvents("Wydarzenie 5", "Chełmno", "86-200", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 15));
                AddEvents("Wydarzenie 6", "Chełmno", "86-200", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 16));
                AddEvents("Wydarzenie 7", "Grudziądz", "86-300", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 17));

                AddEvents("Wydarzenie 8", "Grudziądz", "86-300", "53.4072518", "18.4455253", "U3@gmail.com", new DateTime(2020, 12, 11));
                AddEvents("Wydarzenie 9", "Grudziądz", "86-300", "53.4072518", "18.4455253", "U3@gmail.com", new DateTime(2020, 12, 12));
                AddEvents("Wydarzenie 10", "Toruń", "87-100", "53.4072518", "18.4455253", "U3@gmail.com", new DateTime(2020, 12, 13));
                AddEvents("Wydarzenie 14", "Toruń", "87-100", "53.4072518", "18.4455253", "U4@gmail.com", new DateTime(2020, 12, 14));
                AddEvents("Wydarzenie 15", "Toruń", "87-100", "53.4072518", "18.4455253", "U4@gmail.com", new DateTime(2020, 12, 15));
                AddEvents("Wydarzenie 16", "Bydgoszcz", "85-000", "53.4072518", "18.4455253", "U4@gmail.com", new DateTime(2020, 12, 6));
                AddEvents("Wydarzenie 17", "Osie", "86-150", "53.4072518", "18.4455253", "U4@gmail.com", new DateTime(2020, 12, 17));

                AddEvents("Wydarzenie 29.11", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 11, 29));
                AddEvents("Wydarzenie 30.11", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 11, 30));
                AddEvents("Wydarzenie 1.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 1));
                AddEvents("Wydarzenie 2.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 2));
                AddEvents("Wydarzenie 3.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 3));
                AddEvents("Wydarzenie 6.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 6));
                AddEvents("Wydarzenie 7.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 7));

                AddEvents("Wydarzenie 2.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 2));
                AddEvents("Wydarzenie 3.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 3));
                AddEvents("Wydarzenie 10.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 10));
                AddEvents("Wydarzenie 13.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 13));
                AddEvents("Wydarzenie 14.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 14));
                AddEvents("Wydarzenie 15.12", "Świecie", "86-100", "53.4072518", "18.4455253", "U2@gmail.com", new DateTime(2020, 12, 15));


            }








            context.SaveChanges();
        }
    }
}
