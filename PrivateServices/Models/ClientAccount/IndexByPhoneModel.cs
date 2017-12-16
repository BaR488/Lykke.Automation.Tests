// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace LykkeAutomationPrivate.Models.ClientAccount.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class IndexByPhoneModel
    {
        /// <summary>
        /// Initializes a new instance of the IndexByPhoneModel class.
        /// </summary>
        public IndexByPhoneModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IndexByPhoneModel class.
        /// </summary>
        public IndexByPhoneModel(string clientId = default(string), string phoneNumber = default(string), string previousPhoneNumber = default(string))
        {
            ClientId = clientId;
            PhoneNumber = phoneNumber;
            PreviousPhoneNumber = previousPhoneNumber;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PreviousPhoneNumber")]
        public string PreviousPhoneNumber { get; set; }

    }
}
