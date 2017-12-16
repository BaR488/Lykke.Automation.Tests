// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class HistoryRecordModel
    {
        /// <summary>
        /// Initializes a new instance of the HistoryRecordModel class.
        /// </summary>
        public HistoryRecordModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the HistoryRecordModel class.
        /// </summary>
        public HistoryRecordModel(string id = default(string), System.DateTime? dateTime = default(System.DateTime?), ApiBalanceChangeModel cashInOut = default(ApiBalanceChangeModel), ApiTradeOperation trade = default(ApiTradeOperation), ApiTransfer transfer = default(ApiTransfer), ApiCashOutAttempt cashOutAttempt = default(ApiCashOutAttempt), ApiCashOutCancelled cashOutCancelled = default(ApiCashOutCancelled), ApiCashOutDone cashOutDone = default(ApiCashOutDone), ApiLimitTradeEvent limitTradeEvent = default(ApiLimitTradeEvent))
        {
            Id = id;
            DateTime = dateTime;
            CashInOut = cashInOut;
            Trade = trade;
            Transfer = transfer;
            CashOutAttempt = cashOutAttempt;
            CashOutCancelled = cashOutCancelled;
            CashOutDone = cashOutDone;
            LimitTradeEvent = limitTradeEvent;
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
        [JsonProperty(PropertyName = "DateTime")]
        public System.DateTime? DateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CashInOut")]
        public ApiBalanceChangeModel CashInOut { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Trade")]
        public ApiTradeOperation Trade { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Transfer")]
        public ApiTransfer Transfer { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CashOutAttempt")]
        public ApiCashOutAttempt CashOutAttempt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CashOutCancelled")]
        public ApiCashOutCancelled CashOutCancelled { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CashOutDone")]
        public ApiCashOutDone CashOutDone { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LimitTradeEvent")]
        public ApiLimitTradeEvent LimitTradeEvent { get; set; }

    }
}
