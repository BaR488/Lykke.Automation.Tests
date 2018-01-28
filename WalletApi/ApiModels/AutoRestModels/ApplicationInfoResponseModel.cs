// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ApplicationInfoResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationInfoResponseModel
        /// class.
        /// </summary>
        public ApplicationInfoResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationInfoResponseModel
        /// class.
        /// </summary>
        public ApplicationInfoResponseModel(string appVersion = default(string), System.DateTime? countryPhoneCodesLastModified = default(System.DateTime?), string termsOfUseLink = default(string), string informationBrochureLink = default(string), string refundInfoLink = default(string), string supportPhoneNum = default(string), string userAgreementUrl = default(string), MarginSettings marginSettings = default(MarginSettings))
        {
            AppVersion = appVersion;
            CountryPhoneCodesLastModified = countryPhoneCodesLastModified;
            TermsOfUseLink = termsOfUseLink;
            InformationBrochureLink = informationBrochureLink;
            RefundInfoLink = refundInfoLink;
            SupportPhoneNum = supportPhoneNum;
            UserAgreementUrl = userAgreementUrl;
            MarginSettings = marginSettings;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AppVersion")]
        public string AppVersion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CountryPhoneCodesLastModified")]
        public System.DateTime? CountryPhoneCodesLastModified { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TermsOfUseLink")]
        public string TermsOfUseLink { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "InformationBrochureLink")]
        public string InformationBrochureLink { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RefundInfoLink")]
        public string RefundInfoLink { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SupportPhoneNum")]
        public string SupportPhoneNum { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UserAgreementUrl")]
        public string UserAgreementUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MarginSettings")]
        public MarginSettings MarginSettings { get; set; }

    }
}