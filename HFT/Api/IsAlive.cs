﻿using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class IsAlive : ApiBase
    {
        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }
    }
}
