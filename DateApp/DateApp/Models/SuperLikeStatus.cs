using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class SuperLikeStatus:ViewComponent
    {

        private IRepository repo;

        public SuperLikeStatus(IRepository repository)
        {
            repo = repository;
        }

        public IViewComponentResult Invoke(string UserId)
        {




        }




    }
}
