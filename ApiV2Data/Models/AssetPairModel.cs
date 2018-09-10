// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AssetPairModel
    {
        /// <summary>
        /// Initializes a new instance of the AssetPairModel class.
        /// </summary>
        public AssetPairModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AssetPairModel class.
        /// </summary>
        public AssetPairModel(string id, int accuracy, string baseAssetId, int invertedAccuracy, string name, string quotingAssetId)
        {
            Id = id;
            Accuracy = accuracy;
            BaseAssetId = baseAssetId;
            InvertedAccuracy = invertedAccuracy;
            Name = name;
            QuotingAssetId = quotingAssetId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Accuracy")]
        public int Accuracy { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BaseAssetId")]
        public string BaseAssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "InvertedAccuracy")]
        public int InvertedAccuracy { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "QuotingAssetId")]
        public string QuotingAssetId { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Id");
            }
            if (BaseAssetId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "BaseAssetId");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (QuotingAssetId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "QuotingAssetId");
            }
        }
    }
}