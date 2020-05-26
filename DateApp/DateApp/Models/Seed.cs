using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DateApp.Models;

namespace UndergroundSound.Models

{
    public static class SeedData
    {

        //public static void EnsurePopulated(IApplicationBuilder app)
        public static void EnsurePopulated(AppIdentityDbContext context)
        {


            context.Database.EnsureCreated();


           


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
                        UserName =Name,
                        Surname = Surname,
                        //Sex = Sex,
                        City = City,
                        Dateofbirth =Dateofbirth,
                        Email = Email,                                               
                        EmailConfirmed = false,
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = Email.ToUpper(),
                        NormalizedUserName = Name.ToUpper(),
                    };

                    if (!context.Users.Any(u => u.UserName == User.UserName))
                    {
                        var password = new PasswordHasher<AppUser>();
                        var hashed = password.HashPassword(User, "Sekret123@");
                        User.PasswordHash = hashed;
                        UserStore<AppUser> userStore;

                        userStore = new UserStore<AppUser>(context);

                        userStore.CreateAsync(User).Wait();


                    }
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }






            }

                                                                              

            void AddSearchDetailsToUser(string UserEmail,string Sex="Kobieta")
            {

                try
                {
                    AppUser user = context.Users.Include(s=>s.Details).Where(u => u.Email == UserEmail).First();
                    SearchDetails details = new SearchDetails("Empty",Sex);
                    user.Details = details;
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





            if (!context.Users.Any())
            {

                SeedUser("Tomek", "Kowalski", "Mężczyzna", "Świecie", "U1@gmail.com", new DateTime(1985,8,21));
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

                //AddSearchDetailsToUser("U1@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U2@gmail.com", "Mężczyzna");
                //AddSearchDetailsToUser("U3@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U4@gmail.com", "Męszczyzna");
                //AddSearchDetailsToUser("U5@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U6@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U7@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U8@gmail.com", "Kobieta");
                //AddSearchDetailsToUser("U9@gmail.com", "Męszczyzna");
                //AddSearchDetailsToUser("U1@0gmail.com", "Mężczyzna");
                //AddSearchDetailsToUser("U11@gmail.com", "Męszczyzna");
                //AddSearchDetailsToUser("U12@gmail.com", "Męszczyzna");



                AddCoordinatesToUser("U1@gmail.com", 53.409479, 18.442148);
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


            }


           





            context.SaveChanges();
        }
    }
}
