// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PinSecurityCheckResultModel
    {
        /// <summary>
        /// Initializes a new instance of the PinSecurityCheckResultModel
        /// class.
        /// </summary>
        public PinSecurityCheckResultModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PinSecurityCheckResultModel
        /// class.
        /// </summary>
        public PinSecurityCheckResultModel(bool? passed = default(bool?), ApiAppSettingsModel settings = default(ApiAppSettingsModel))
        {
            Passed = passed;
            Settings = settings;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Passed")]
        public bool? Passed { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Settings")]
        public ApiAppSettingsModel Settings { get; set; }

    }
}