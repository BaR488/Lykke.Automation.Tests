// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace LykkeAutomationPrivate.Models.ClientAccount.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for WalletType.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WalletType
    {
        [EnumMember(Value = "Trusted")]
        Trusted,
        [EnumMember(Value = "Trading")]
        Trading
    }
    internal static class WalletTypeEnumExtension
    {
        internal static string ToSerializedValue(this WalletType? value)
        {
            return value == null ? null : ((WalletType)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this WalletType value)
        {
            switch( value )
            {
                case WalletType.Trusted:
                    return "Trusted";
                case WalletType.Trading:
                    return "Trading";
            }
            return null;
        }

        internal static WalletType? ParseWalletType(this string value)
        {
            switch( value )
            {
                case "Trusted":
                    return WalletType.Trusted;
                case "Trading":
                    return WalletType.Trading;
            }
            return null;
        }
    }
}
