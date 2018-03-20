// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CapabilitiesResponse
    {
        /// <summary>
        /// Initializes a new instance of the CapabilitiesResponse class.
        /// </summary>
        public CapabilitiesResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CapabilitiesResponse class.
        /// </summary>
        public CapabilitiesResponse(bool isTransactionsRebuildingSupported, bool? areManyInputsSupported = default(bool?),
            bool? areManyOutputsSupported = default(bool?), string contractVersion = default(string), bool isTestingTransfersSupported = default(bool))
        {
            ContractVersion = contractVersion;
            IsTransactionsRebuildingSupported = isTransactionsRebuildingSupported;
            AreManyInputsSupported = areManyInputsSupported;
            AreManyOutputsSupported = areManyOutputsSupported;
            IsTestingTransfersSupported = isTestingTransfersSupported;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "contractVersion")]
        public string ContractVersion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isTransactionsRebuildingSupported")]
        public bool IsTransactionsRebuildingSupported { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "areManyInputsSupported")]
        public bool? AreManyInputsSupported { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "areManyOutputsSupported")]
        public bool? AreManyOutputsSupported { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isTestingTransfersSupported")]
        public bool IsTestingTransfersSupported { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
