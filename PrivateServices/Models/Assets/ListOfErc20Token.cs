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

    public partial class ListOfErc20Token
    {
        /// <summary>
        /// Initializes a new instance of the ListOfErc20Token class.
        /// </summary>
        public ListOfErc20Token()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ListOfErc20Token class.
        /// </summary>
        public ListOfErc20Token(IList<Erc20Token> items = default(IList<Erc20Token>))
        {
            Items = items;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Items")]
        public IList<Erc20Token> Items { get; set; }

    }
}