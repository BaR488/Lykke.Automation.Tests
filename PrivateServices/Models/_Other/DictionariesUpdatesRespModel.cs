// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class DictionariesUpdatesRespModel
    {
        /// <summary>
        /// Initializes a new instance of the DictionariesUpdatesRespModel
        /// class.
        /// </summary>
        public DictionariesUpdatesRespModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DictionariesUpdatesRespModel
        /// class.
        /// </summary>
        public DictionariesUpdatesRespModel(System.DateTime? assetsUpd = default(System.DateTime?))
        {
            AssetsUpd = assetsUpd;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetsUpd")]
        public System.DateTime? AssetsUpd { get; set; }

    }
}
