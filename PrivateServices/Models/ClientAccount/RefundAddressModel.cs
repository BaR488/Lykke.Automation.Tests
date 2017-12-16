// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace LykkeAutomationPrivate.Models.ClientAccount.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RefundAddressModel
    {
        /// <summary>
        /// Initializes a new instance of the RefundAddressModel class.
        /// </summary>
        public RefundAddressModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RefundAddressModel class.
        /// </summary>
        public RefundAddressModel(int validDays, bool sendAutomatically, string address = default(string), string clientId = default(string))
        {
            Address = address;
            ValidDays = validDays;
            SendAutomatically = sendAutomatically;
            ClientId = clientId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ValidDays")]
        public int ValidDays { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SendAutomatically")]
        public bool SendAutomatically { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get; set; }

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
