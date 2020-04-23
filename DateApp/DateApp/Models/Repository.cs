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
        bool AddPicture( string UserId,PictureType type,string FilePath);
        bool RemovePicture(string UserId, PictureType type);

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

        public SearchDetails GetUserDetails(string UserId)
        {
            try
            {
                AppUser user = context.Users.AsNoTracking().Include(x => x.Details).Where(u => u.Id == UserId).First(); //

                return user.Details;
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
    }
}
