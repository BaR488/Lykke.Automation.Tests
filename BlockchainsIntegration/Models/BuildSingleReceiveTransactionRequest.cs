﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class BuildSingleReceiveTransactionRequest
    {
        public string operationId { get; set; }
        public string sendTransactionHash { get; set; }
    }
}
