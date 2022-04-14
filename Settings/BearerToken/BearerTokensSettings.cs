namespace SuperAgentCore.Settings.BearerToken
{
    public class BearerTokensSettings
    {
        public string Key { set; get; }
        public string Issuer { set; get; }
        public int AccessTokenExpirationMinutes { set; get; }
        public int RefreshTokenExpirationMinutes { set; get; }
    }
}
