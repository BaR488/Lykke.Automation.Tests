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

    public partial class EthereumTransactionModelContainer
    {
        /// <summary>
        /// Initializes a new instance of the EthereumTransactionModelContainer
        /// class.
        /// </summary>
        public EthereumTransactionModelContainer()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the EthereumTransactionModelContainer
        /// class.
        /// </summary>
        public EthereumTransactionModelContainer(IList<EthereumTransactionModel> transfers = default(IList<EthereumTransactionModel>))
        {
            Transfers = transfers;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Transfers")]
        public IList<EthereumTransactionModel> Transfers { get; set; }

    }
}