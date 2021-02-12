using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{









    public abstract class PictureRemover
    {
        protected PictureRemover picture;

        public IHostingEnvironment env;

        protected string PicturePath { get { return "/AppPictures/photo.png"; } }

        public bool deletefile(string fname)
        {
            bool action = false;
            fname = fname.Replace("/Home/GetPicture/", "");
            string _imageToBeDeleted = Path.Combine(env.ContentRootPath, "UserImages\\", fname);
            if ((System.IO.File.Exists(_imageToBeDeleted)))
            {

                try
                {

                    System.IO.File.Delete(_imageToBeDeleted);

                }
                catch (Exception ex)
                {

                }


            }
            return action;
        }


        public void setNumber(PictureRemover pic)
        {
            this.picture = pic;
        }

        public abstract void ForwardRequest(PictureType type, SearchDetails details);
    }

    class MainPhotoRemove : PictureRemover
    {

        public MainPhotoRemove(IHostingEnvironment env)
        {
            this.env = env;
        }

        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.MainPhotoPath)
            {
                deletefile(details.MainPhotoPath);
                details.MainPhotoPath = PicturePath;


            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details);

            }

        }
    }

    class Photo1Remove : PictureRemover
    {

        public Photo1Remove(IHostingEnvironment env)
        {
            this.env = env;
        }


        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath1)
            {
                deletefile(details.PhotoPath1);
                details.PhotoPath1 = PicturePath;

            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details);

            }

        }
    }


    class Photo2Remove : PictureRemover
    {
        public Photo2Remove(IHostingEnvironment env)
        {
            this.env = env;
        }


        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath2)
            {
                deletefile(details.PhotoPath2);
                details.PhotoPath2 = PicturePath;

            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details);

            }

        }
    }



    class Photo3Remove : PictureRemover
    {
        public Photo3Remove(IHostingEnvironment env)
        {
            this.env = env;
        }

        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath3)
            {
                deletefile(details.PhotoPath3);
                details.PhotoPath3 = PicturePath;

            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details);

            }

        }
    }





















}
