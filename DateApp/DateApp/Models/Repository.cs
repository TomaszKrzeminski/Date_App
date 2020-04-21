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
        bool AddPicture(SearchDetails details,string UserId);

    }


    public class Repository : IRepository
    {
        AppIdentityDbContext context;

        public Repository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public bool AddPicture(SearchDetails details, string UserId)
        {

            try
            {

                SearchDetails d = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First().Details;

                d = details;

                context.SaveChanges();


                return true;
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return false;

            }
        }

        public SearchDetails GetUserDetails(string UserId)
        {
            try
            {
                AppUser user = context.Users.Include(x => x.Details).Where(u => u.Id == UserId).First();
                
                return user.Details;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
