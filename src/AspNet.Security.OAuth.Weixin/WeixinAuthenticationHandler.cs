/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Weixin
{
    public class WeixinAuthenticationHandler : OAuthHandler<WeixinAuthenticationOptions>
    {
        public WeixinAuthenticationHandler(
            [NotNull] IOptionsMonitor<WeixinAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity, 
            [NotNull] AuthenticationProperties properties, 
            [NotNull] OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["access_token"] = tokens.AccessToken,
                //["openid"] = tokens.Response.Value<string>("openid")
                ["code"] = Request.Query["code"]

            });

            var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(
            string code, string redirectUri)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>()
            {
                //["appid"] = Options.ClientId,
                //["secret"] = Options.ClientSecret,
                //["code"] = code,
                //["grant_type"] = "authorization_code"
                ["corpid"] = Options.ClientId,
                ["corpsecret"] = Options.ClientSecret,

            });

            var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }
            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var str = AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["appid"] = Options.ClientId,
                //["scope"] = FormatScope(),
                ["scope"] = "snsapi_base",
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties)
            });
            return str + "#wechat_redirect";
        }

        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (queryString == null)
                throw new ArgumentNullException(nameof(queryString));
            int num = uri.IndexOf('#');
            string str1 = uri;
            string str2 = "";
            if (num != -1)
            {
                str2 = uri.Substring(num);
                str1 = uri.Substring(0, num);
            }
            bool flag = str1.IndexOf('?') != -1;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(str1);
            foreach (KeyValuePair<string, string> keyValuePair in queryString)
            {
                stringBuilder.Append(flag ? '&' : '?');
                stringBuilder.Append(UrlEncoder.Default.Encode(keyValuePair.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(UrlEncoder.Default.Encode(keyValuePair.Value));
                flag = true;
            }
            stringBuilder.Append(str2);
            //stringBuilder.Append("#wechat_redirect");//企业微信固定带上
            return stringBuilder.ToString();
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}
