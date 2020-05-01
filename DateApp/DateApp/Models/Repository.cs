using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DateApp.Models
{

    public interface IRepository
    {

        SearchDetails GetUserDetails(string UserId);

        bool ChangeUserDetails(UserDetailsModel model);
        bool AddPicture(string UserId, PictureType type, string FilePath);
        bool RemovePicture(string UserId, PictureType type);
        string GetPhoneNumber(string Id);
        bool ChangePhoneNumber(string Id, string PhoneNumber);
        bool ChangeSearchSex(string Sex,string Id);
        bool SetDistance(string Id, int Distance);
        bool SetSearchAge(string UserId, int Age);
        bool SetShowProfile(string Id,bool Show);

    }


    public class Repository : IRepository
    {
        AppIdentityDbContext context;

        public Repository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public bool AddPicture(string UserId, PictureType type, string FilePath)
        {

            PictureSaver main = new MainPhoto();
            PictureSaver photo1 = new Photo1();
            PictureSaver photo2 = new Photo2();
            PictureSaver photo3 = new Photo3();

            main.setNumber(photo1);
            photo1.setNumber(photo2);
            photo2.setNumber(photo3);

            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                main.ForwardRequest(type, details, FilePath);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangePhoneNumber(string Id, string PhoneNumber)
        {

            try
            {
                AppUser user = context.Users.Where(u => u.Id == Id).First();
                user.PhoneNumber = PhoneNumber;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeSearchSex(string Sex,string Id)
        {

            if (Sex != null)
            {
                try
                {
                    SearchDetails model = context.Users.Include(s=>s.Details).Where(u => u.Id == Id).First().Details;
                    model.SearchSex = Sex;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }





        }

        public bool ChangeUserDetails(UserDetailsModel model)
        {

            try
            {

                if (model.DetailsId != 0)
                {

                    SearchDetails details = context.SearchDetails.Find(model.DetailsId);
                    details.MainPhotoPath = model.MainPhotoPath;
                    details.PhotoPath1 = model.PhotoPath1;
                    details.PhotoPath2 = model.PhotoPath2;
                    details.PhotoPath3 = model.PhotoPath3;
                    details.Description = model.Description;
                    details.CityOfResidence = model.CityOfResidence;
                    details.JobPosition = model.JobPosition;
                    details.CompanyName = model.CompanyName;
                    details.School = model.School;
                    details.UserId = model.UserId;

                    AppUser user = context.Users.Find(model.UserId);


                    user.Details = details;

                    context.SaveChanges();
                }
                else
                {
                    SearchDetails details = new SearchDetails();
                    details.MainPhotoPath = model.MainPhotoPath;
                    details.PhotoPath1 = model.PhotoPath1;
                    details.PhotoPath2 = model.PhotoPath2;
                    details.PhotoPath3 = model.PhotoPath3;
                    details.Description = model.Description;
                    details.CityOfResidence = model.CityOfResidence;
                    details.JobPosition = model.JobPosition;
                    details.CompanyName = model.CompanyName;
                    details.School = model.School;
                    details.UserId = model.UserId;


                    AppUser user = context.Users.Find(model.UserId);
                    user.Details = new SearchDetails()
                    {
                        MainPhotoPath = model.MainPhotoPath,
                        PhotoPath1 = model.PhotoPath1,
                        PhotoPath2 = model.PhotoPath2,
                        PhotoPath3 = model.PhotoPath3,
                        Description = model.Description,
                        CityOfResidence = model.CityOfResidence,
                        JobPosition = model.JobPosition,
                        CompanyName = model.CompanyName,
                        School = model.School,
                        UserId = model.UserId,

                    };

                    context.SaveChanges();

                }


                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public string GetPhoneNumber(string UserId)
        {
            try
            {
                string phone = context.Users.Where(u => u.Id == UserId).First().PhoneNumber;

                return phone;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SearchDetails GetUserDetails(string UserId)
        {
            try
            {
                SearchDetails details = context.SearchDetails.Include(u => u.User).Where(s => s.UserId == UserId).First();
                return details;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RemovePicture(string UserId, PictureType type)
        {

            PictureRemover main = new MainPhotoRemove();
            PictureRemover photo1 = new Photo1Remove();
            PictureRemover photo2 = new Photo2Remove();
            PictureRemover photo3 = new Photo3Remove();

            main.setNumber(photo1);
            photo1.setNumber(photo2);
            photo2.setNumber(photo3);

            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                main.ForwardRequest(type, details);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetDistance(string UserId, int Distance)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.SearchDistance = Distance;
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }



        }

        public bool SetSearchAge(string UserId, int Age)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.SearchAge = Age;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public bool SetShowProfile(string UserId, bool Show)
        {
            try
            {
                SearchDetails details = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;
                details.ShowProfile = Show;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
