namespace Tiger.WebApi.Client
{
    public sealed class Constants
    {
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public const string SIGN_METHOD_MD5 = "md5";
        public const string SIGN_METHOD_HMAC = "hmac";
        public const string SIGN_METHOD_HMAC_SHA256 = "hmac-sha256";


        /// <summary>
        /// 请求API方法
        /// </summary>
        public const string REST_METHOD = "Rest-Method";
        /// <summary>
        /// AppKey
        /// </summary>
        public const string REST_APP_KEY = "Rest-AppKey";
        /// <summary>
        /// 签名摘要算法
        /// </summary>
        public const string REST_SIGN_METHOD = "Rest-SignMethod";
        /// <summary>
        /// 签名结果
        /// </summary>
        public const string REST_SIGN = "Rest-Sign";
        /// <summary>
        /// 时间戳
        /// </summary>
        public const string REST_TIMESTAMP = "Rest-Timestamp";
        /// <summary>
        /// 合作伙伴身份标识
        /// </summary>
        public const string REST_PARTNER_ID = "Rest-PartnerId";
    }
}
