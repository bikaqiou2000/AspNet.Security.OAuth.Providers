using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCClient21.Controllers
{
    /// <summary>
    /// 模拟企业微信登陆三步接口
    /// </summary>
    public class MockEntWeiXinController : Controller
    {
        [HttpGet("~/connect/oauth2/authorize")]
        public IActionResult authorize(
            [FromQuery] string appid 
            ,[FromQuery] string redirect_uri
            ,[FromQuery] string response_type 
            ,[FromQuery] string scope
            ,[FromQuery] string state = "")
        {
            string nextUrl =$"{redirect_uri}?action=get&code=eh3CZBgG333qs9EdaPbCAP1VaOrjuNkiAZHTWgaWsZQ";
            if (!string.IsNullOrEmpty(state))
            {
                nextUrl += $"&state={state}";
            }
           
            return Redirect(nextUrl);
        }

        [HttpGet("~/cgi-bin/gettoken")]
        public IActionResult gettoken(
            [FromQuery] string corpid
            , [FromQuery] string corpsecret)
        {
            var res = Json(new
            {
                errcode = 0,
                errmsg = "ok",
                access_token = "accesstoken000001",
                expires_in = 7200
            });
            return res;
        }

        [HttpGet("~/cgi-bin/user/getuserinfo")]
        public IActionResult getuserinfo(
            [FromQuery] string access_token
            , [FromQuery] string code)
        {
            var res = Json(new
            {
                errcode = 0,
                errmsg = "ok",
                UserId = "user1",
                OpenId = "openid1",
                DeviceId = "DEVICEID1"
            });
            return res;
        }

    }
}