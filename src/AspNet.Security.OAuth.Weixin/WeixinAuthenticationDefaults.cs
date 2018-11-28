/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Weixin
{
    /// <summary>
    /// Default values for Weixin authentication.
    /// </summary>
    public static class WeixinAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Weixin";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "微信";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-weixin";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Weixin";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        //public const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/qrconnect";
        //企业微信的oauth登录
        public const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
        
        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        //public const string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";
        //企业微信获取API调用令牌
        public const string TokenEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        //public const string UserInformationEndpoint = "https://api.weixin.qq.com/sns/userinfo";
        //企业微信获用户信息
        public const string UserInformationEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";
        
    }
}
