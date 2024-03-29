﻿using DateApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public enum PictureType
    {
        MainPhotoPath,
        PhotoPath1,
        PhotoPath2,
        PhotoPath3
    }


    public enum PictureTypeEvent
    {       
        PhotoPath1,
        PhotoPath2,
        PhotoPath3
    }

    public abstract class PictureSaver
    {
        protected PictureSaver picture;
       

        public void setNumber(PictureSaver pic)
        {
            this.picture = pic;
        }

        public abstract void ForwardRequest(PictureType type,SearchDetails details,string Path);
    }


    class MainPhoto : PictureSaver
    {
        public override void ForwardRequest(PictureType type, SearchDetails details,string Path)
        {
            if (type == PictureType.MainPhotoPath)
            {
                details.MainPhotoPath = Path;
              
                
              
            }
            else if (type != null)
            {
                picture.ForwardRequest(type,details,Path);
               
            }
          
        }
    }

    class Photo1 : PictureSaver
    {
        public override void ForwardRequest(PictureType type, SearchDetails details, string Path)
        {
            if (type == PictureType.PhotoPath1)
            {
                details.PhotoPath1 = Path;
                
            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details, Path);
               
            }
           
        }
    }


    class Photo2 : PictureSaver
    {
        public override void ForwardRequest(PictureType type, SearchDetails details, string Path)
        {
            if (type == PictureType.PhotoPath2)
            {
                details.PhotoPath2 = Path;
              
            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details, Path);
               
            }
           
        }
    }



    class Photo3 : PictureSaver
    {
        public override void ForwardRequest(PictureType type, SearchDetails details, string Path)
        {
            if (type == PictureType.PhotoPath3)
            {
                details.PhotoPath3 = Path;
               
            }
            else if (type != null)
            {
                picture.ForwardRequest(type, details, Path);
                
            }
          
        }
    }












}





