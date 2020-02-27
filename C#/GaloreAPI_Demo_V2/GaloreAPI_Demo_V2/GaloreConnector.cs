using IdentityModel.Client;
using kognifai.ClientCredentials;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GaloreAPIDemoV2
{
    public class GaloreConnector
    {
        private const string AUTHORIZATION_HEADER = "Authorization";
        private const string APIMSUBKEY_HEADER = "Ocp-Apim-Subscription-Key";
        private readonly string AuthorityUrl;
        private readonly string ClientId;
        private readonly string ClientSecret;
        private readonly string GaloreBaseUrl;
        private readonly string GrantType;
        private readonly string Scope;
        private readonly string UserId;
        private readonly string TenantId;
        private readonly string Resource;
        private readonly string APIMSubscriptionKey;

        public GaloreConnector()
        {
            AuthorityUrl = ConfigurationManager.AppSettings["AuthorityUrl"];
            ClientId = ConfigurationManager.AppSettings["ClientId"];
            ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            GaloreBaseUrl = ConfigurationManager.AppSettings["GaloreBaseUrl"];
            GrantType = ConfigurationManager.AppSettings["GrantType"];
            Scope = ConfigurationManager.AppSettings["Scope"];
            UserId = ConfigurationManager.AppSettings["UserId"];
            TenantId = ConfigurationManager.AppSettings["TenantId"];
            Resource = ConfigurationManager.AppSettings["Resource"];
            APIMSubscriptionKey = ConfigurationManager.AppSettings["APIMSubscriptionKey"];
        }

        public async Task<HttpClient> GetClient()
        {
            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy { BypassProxyOnLocal = true }
            };
            var httpClient = new HttpClient(handler);
            if (!bool.TryParse(ConfigurationManager.AppSettings["OnPremise"], out var onPremise) || onPremise) 
            {
                await ApplyAccessToken(httpClient);
            }
            else
            {
                await ApplyADAccessToken(httpClient);
            }
            return httpClient;
        }

        protected async Task ApplyAccessToken(HttpClient httpClient)
        {
            try
            {
                httpClient.BaseAddress = new Uri(GaloreBaseUrl);
                var tokenRequest = new ClientCredentialsTokenRequest()
                {
                    Address = AuthorityUrl,
                    ClientId = ClientId,
                    ClientSecret = ClientSecret,
                    GrantType = GrantType,
                    Scope = Scope
                };
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy { BypassProxyOnLocal = true }
                };
                using HttpClient tokenClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(AuthorityUrl)
                };
                var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync(tokenRequest);
                if (tokenResponse != null && tokenResponse.IsError == false)
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, $"Bearer {tokenResponse.AccessToken}");
                }
                else
                {
                    throw new Exception(tokenResponse?.Error ?? "Token request error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task ApplyADAccessToken(HttpClient httpClient)
        {
            try
            {
                httpClient.BaseAddress = new Uri(GaloreBaseUrl);
                ClientCredentialsOptions options = new ClientCredentialsOptions()
                {
                    AuthorityUrl = AuthorityUrl + TenantId + "/",
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                };

                IClientCredentialsManager clientCredentialsManager = new ClientCredentialsManager(options);
                var token = await clientCredentialsManager.GetAccessTokenAsync(Resource);

                if (httpClient.DefaultRequestHeaders.Contains(AUTHORIZATION_HEADER))
                {
                    httpClient.DefaultRequestHeaders.Remove(AUTHORIZATION_HEADER);
                }
                httpClient.DefaultRequestHeaders.Add(APIMSUBKEY_HEADER, APIMSubscriptionKey);
                httpClient.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, $"Bearer {token}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
