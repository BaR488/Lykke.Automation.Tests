// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GetPrivateWalletBalancesModel
    {
        /// <summary>
        /// Initializes a new instance of the GetPrivateWalletBalancesModel
        /// class.
        /// </summary>
        public GetPrivateWalletBalancesModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetPrivateWalletBalancesModel
        /// class.
        /// </summary>
        public GetPrivateWalletBalancesModel(IList<ApiBalanceRecord> balances = default(IList<ApiBalanceRecord>))
        {
            Balances = balances;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Balances")]
        public IList<ApiBalanceRecord> Balances { get; set; }

    }
}
