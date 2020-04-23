using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
   

    public abstract class PictureRemover
    {
        protected PictureRemover picture;
        
        protected string PicturePath { get { return "/AppPictures/photo.png"; } }
       

        public void setNumber(PictureRemover pic)
        {
            this.picture = pic;
        }

        public abstract void ForwardRequest(PictureType type, SearchDetails details);
    }

    class MainPhotoRemove : PictureRemover
    {

      

        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.MainPhotoPath)
            {
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

       


        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath1)
            {
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



        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath2)
            {
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
        

        public override void ForwardRequest(PictureType type, SearchDetails details)
        {
            if (type == PictureType.PhotoPath3)
            {
                details.PhotoPath3 = PicturePath;
               
            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details);
                
            }
            
        }
    }





















}
