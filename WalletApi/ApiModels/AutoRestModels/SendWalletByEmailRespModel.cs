// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SendWalletByEmailRespModel
    {
        /// <summary>
        /// Initializes a new instance of the SendWalletByEmailRespModel class.
        /// </summary>
        public SendWalletByEmailRespModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SendWalletByEmailRespModel class.
        /// </summary>
        public SendWalletByEmailRespModel(string email = default(string))
        {
            Email = email;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

    }
}
