using SDK.BaseCore;
using Utils;
using TEngine;

namespace SDK.Purchase
{
    public static class PurchaseUtils
    {
        public static void SetCacheItemId(string itemId)
        {
            StorageUtils.SetString(GameDefines.SettingKeys.UwCacheItemID, itemId);
        }

        public static string GetCurrencyCode(string productId)
        {
            return SDKManager.Instance.GetPurchaseCurrencyCode(productId);
        }

        public static string GetLocalPrice(string productId)
        {
            return SDKManager.Instance.GetPurchaseLocalPrice(productId);
        }

        public static void QueryPurchaseOrder()
        {
            SDKManager.Instance.QueryPurchaseOrder();
        }

        #region 价格处理-照搬LastFortress

        public static void SetSkuInfoFromNative(string data)
        {
            // 设置缺省Key
            string defaultKey = "uw_1";
            // 存储缺省值
            var defaultValue = "";
            if (data.IsNullOrEmpty())
            {
                StorageUtils.SetString(defaultKey, "");
                StorageUtils.SetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, 0);
                StorageUtils.SetString(GameDefines.SettingKeys.UwPriceSymbol, "");
                StorageUtils.SetString(GameDefines.SettingKeys.UwCurrencyCodeKey, "");
            }
            else
            {
                string[] vector1;
                var vector0 = data.Split('|');
                if (vector0 is {Length: 2})
                {
                    StorageUtils.SetString(GameDefines.SettingKeys.UwCurrencyCodeKey, vector0[0]);
                    vector1 = vector0[1].Split(';');
                }
                else
                {
                    vector1 = data.Split(';');
                }

                if (vector1 is {Length: > 0})
                {
                    foreach (var per in vector1)
                    {
                        var vector2 = per.Split(':');
                        if (vector2 == null || vector2.Length < 2)
                        {
                            continue;
                        }

                        var productId = vector2[0];
                        var price = vector2[1];

                        StorageUtils.SetString(productId, price);
                        if (productId == defaultKey)
                        {
                            defaultValue = price;
                        }
                    }
                }

                if (defaultValue.IsNullOrEmpty())
                {
                    return;
                }

                //下面是为了计算汇率存起来
                string checkString = "0123456789,. ";
                string symbol = "";
                string valuestring = "";
                int index = 0;
                int symIndex = 0;
                bool symBack = false;
                var tempSub = defaultValue.ToCharArray();
                if (tempSub.Length > 0)
                {
                    for (int i = 0; i < tempSub.Length;)
                    {
                        if (checkString.Contains(tempSub[i].ToString()))
                        {
                            ++symIndex;
                            ++i;
                            while (i < tempSub.Length)
                            {
                                if (!checkString.Contains(tempSub[i].ToString()))
                                {
                                    break;
                                }

                                ++symIndex;
                                ++i;
                            }

                            if (symbol.IsNullOrEmpty())
                            {
                                symbol = defaultValue.Substring(symIndex, tempSub.Length - symIndex);
                                valuestring = defaultValue.Substring(index, symIndex);
                                symBack = true;
                            }
                        }
                        else
                        {
                            symIndex = i;
                            ++index;
                            ++i;
                            while (i < tempSub.Length)
                            {
                                if (checkString.Contains(tempSub[i].ToString()))
                                {
                                    break;
                                }

                                ++index;
                                ++i;
                            }

                            symbol = defaultValue.Substring(symIndex, index);
                            if (index < tempSub.Length)
                                valuestring = defaultValue.Substring(index, tempSub.Length - index);
                            symBack = false;
                        }
                    }
                }

                if (valuestring.Contains(","))
                {
                    valuestring = valuestring.Replace(",", ""); //逗号应该只是划分千分位 不应该替换成“.”
                }

                if (valuestring.Contains(" "))
                {
                    valuestring = valuestring.Replace(" ", "");
                }

                bool right = true;
                string check_temp = "1234567890.";
                foreach (var per in valuestring)
                {
                    if (!check_temp.Contains(per.ToString()))
                    {
                        right = false;
                        break;
                    }
                }

                if (right)
                {
                    float value = valuestring.ToFloat();
                    float percent = value / 0.99f;

                    StorageUtils.SetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, percent);
                    StorageUtils.SetString(GameDefines.SettingKeys.UwPriceSymbol, symbol);
                    StorageUtils.SetBool(GameDefines.SettingKeys.UwPriceSymbolBack, symBack);
                }
            }
        }

        public static string GetPriceText(string dollar, string productId, bool withCurrencyCode = true)
        {
            if (withCurrencyCode)
            {
                return Internal_GetPriceText(dollar, productId);
            }

            var str = Internal_GetPriceText(dollar, productId);
            var currencyCode = GetCurrencyCode(productId);

#if UNITY_EDITOR
            currencyCode = "US";
#endif

            if (string.IsNullOrEmpty(currencyCode))
            {
                return str;
            }

            return str.Replace(currencyCode, string.Empty).Replace(" ", string.Empty);
        }

        //只获取价格 纯数字字符串（当地货币）
        public static string GetPriceValueText(string dollar, string productId)
        {
            string returnString = "";
            if (!string.IsNullOrEmpty(productId))
            {
                if (!string.IsNullOrEmpty(dollar))
                {
                    returnString = GetLocalCurrencyFromNative_ValueString(productId);
                }
            }
            else
            {
                float retRate = StorageUtils.GetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, 0);
                if (retRate != 0)
                {
                    float tempDollar = dollar.ToFloat() * retRate;
                    returnString = tempDollar.ToString("N");
                }
            }
            
            if (string.IsNullOrEmpty(returnString))
            {
                float tempDollar = dollar.ToFloat();
                if (PlatformUtils.IsAndroidPlatform())
                {
                    string tempPrice = "";
                    if (!productId.IsNullOrEmpty())
                    {
                        tempPrice = GetLocalCurrencyFromNative_ValueString(productId);
                    }

                    if (tempPrice.IsNullOrEmpty())
                    {
                        var exchangeRate = StorageUtils.GetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, 0);
                        if (exchangeRate > 0)
                        {
                            var payValue = tempDollar;
                            payValue *= exchangeRate;
                            
                            return payValue.ToString("N");
                        }
                    }
                    else
                    {
                        return tempPrice;
                    }
                }
            }
            else
            {
                return returnString;
            }

            return dollar;
        }

        private static string GetLocalCurrencyFromNative(string productId)
        {
            return StorageUtils.GetString(productId, "");
        }
        
        private static string GetLocalCurrencyFromNative_ValueString(string productId)
        {
            var defaultValue = StorageUtils.GetString(productId, "");
            if (defaultValue.IsNullOrEmpty())
            {
                return "";
            }

            //下面是为了计算汇率存起来
            string checkString = "0123456789,. ";
            string symbol = "";
            string valuestring = "";
            int index = 0;
            int symIndex = 0;
            // bool symBack = false;
            var tempSub = defaultValue.ToCharArray();
            if (tempSub.Length > 0)
            {
                for (int i = 0; i < tempSub.Length;)
                {
                    if (checkString.Contains(tempSub[i].ToString()))
                    {
                        ++symIndex;
                        ++i;
                        while (i < tempSub.Length)
                        {
                            if (!checkString.Contains(tempSub[i].ToString()))
                            {
                                break;
                            }

                            ++symIndex;
                            ++i;
                        }

                        if (symbol.IsNullOrEmpty())
                        {
                            symbol = defaultValue.Substring(symIndex, tempSub.Length - symIndex);
                            valuestring = defaultValue.Substring(index, symIndex);
                            // symBack = true;
                        }
                    }
                    else
                    {
                        symIndex = i;
                        ++index;
                        ++i;
                        while (i < tempSub.Length)
                        {
                            if (checkString.Contains(tempSub[i].ToString()))
                            {
                                break;
                            }

                            ++index;
                            ++i;
                        }

                        symbol = defaultValue.Substring(symIndex, index);
                        if (index < tempSub.Length)
                            valuestring = defaultValue.Substring(index, tempSub.Length - index);
                        // symBack = false;
                    }
                }
            }

            if (valuestring.IsNullOrEmpty())
            {
                return "";
            }
            return valuestring;
        }

        private static string Internal_GetPriceText(string dollar, string productId)
        {
            string returnString = "";
            if (!string.IsNullOrEmpty(productId))
            {
                if (!string.IsNullOrEmpty(dollar))
                {
                    returnString = GetLocalCurrencyFromNative(productId);
                }
            }
            else
            {
                float retRate = StorageUtils.GetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, 0);
                string retSymbol = StorageUtils.GetString(GameDefines.SettingKeys.UwPriceSymbol, "");
                if (retRate != 0)
                {
                    float tempDollar = dollar.ToFloat() * retRate;
                    returnString = retSymbol + tempDollar.ToString("N");
                }
            }

            string payCurrency = "US $";
            if (string.IsNullOrEmpty(returnString))
            {
                float tempDollar = dollar.ToFloat();
                if (PlatformUtils.IsAndroidPlatform())
                {
                    string tempPrice = "";
                    if (!productId.IsNullOrEmpty())
                    {
                        tempPrice = GetLocalCurrencyFromNative(productId);
                    }

                    if (tempPrice.IsNullOrEmpty())
                    {
                        var symbol = StorageUtils.GetString(GameDefines.SettingKeys.UwPriceSymbol, "");
                        var exchangeRate = StorageUtils.GetFloat(GameDefines.SettingKeys.UwPriceExchangeRate, 0);
                        var symbolBack = StorageUtils.GetBool(GameDefines.SettingKeys.UwPriceSymbolBack);
                        if (!symbol.IsNullOrEmpty() && exchangeRate > 0)
                        {
                            var payValue = tempDollar;
                            payValue *= exchangeRate;
                            if (symbolBack == false)
                            {
                                return symbol + payValue.ToString("N");
                            }

                            return payValue.ToString("N") + symbol;
                        }
                    }
                    else
                    {
                        return tempPrice;
                    }
                }
            }
            else
            {
                return returnString;
            }

            return payCurrency + dollar;
        }

        #endregion
    }
}