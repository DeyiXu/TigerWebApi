namespace Tiger.WebApi.Core.Constants
{
	public sealed class CommonConstant
	{
		public const string SIGN_METHOD_MD5 = "md5";
		public const string SIGN_METHOD_HMAC = "hmac";

		public const string PACKAGE_NAME = "Packages";
		public const string PACKAGE_SERVICE_NAME = "Service";
		public const string PACKAGE_COMMON_NAME = "Common";
		public const string PACKAGE_SERVER_NAME = "Server";

        public const string ENVIRONMENT_PACKAGE_WATCHER = "TIGERWEBAPI_PACKAGE_WATCHER";
        public const string CONTENT_TYPE = "application/json";
        public const string CONFIG_FILENAME = "TigerWebApiConfig.json";
	}
}
