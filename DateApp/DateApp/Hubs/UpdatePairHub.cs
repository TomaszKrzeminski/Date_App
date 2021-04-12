using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Hubs
{

    [Authorize]
    public class UpdatePairHub : Hub
    {


        private IHubContext<UpdatePairHub> pairContext;

        public UpdatePairHub(IHubContext<UpdatePairHub> pairContext)
        {

            this.pairContext = pairContext;
        }
                                        



    }


    



}

