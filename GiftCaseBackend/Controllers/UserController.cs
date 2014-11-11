using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
using com.shephertz.app42.paas.sdk.csharp.social;
using GiftCaseBackend.Models;

namespace GiftCaseBackend.Controllers
{
    public class UserController : ApiController
    {
        /*
         * Instructions:
         * How you can access the REST api provided by this controler:
         * use the url:  /api/User/NameOfTheAction?parameterName=value&parameter2=value
         * 
         * Name of the action is the method name. If you want to change the url into something different
         * than the name of the action put an attribute: [ActionName("NewName")] above the method
         * 
         * The results returned by REST api are either in JSON or XML format. The format depends on what the requester
         * specified he would like to receive. If not specified it defaults to JSON.
         * 
         * The server will automaticaly try to map what methods should be called with what type of request. If you
         * want to specify it manualy you can put attributes above the method, eg: [HttpGet],[HttpPost]
         */




        /// <summary>
        /// Links facebook account with BaaS
        /// URL example:
        /// http://localhost:22467/api/User/LoginFacebook?userName=ana&accessToken=someGarbage&deviceToken=whatever
        /// </summary>
        /// <param name="userName">User's user name</param>
        /// <param name="accessToken">Facebook access token</param>
        /// <param name="deviceToken">A token identifying the device user is login in from</param>
        /// <returns>status message</returns>
        [HttpGet]
        public User LoginFacebook(string userName, string accessToken, string deviceToken)
        {
            try
            {
                var social = BaaS.SocialService.LinkUserFacebookAccount(userName, accessToken);
                var push =  BaaS.PushNotificationService.StoreDeviceToken(userName, deviceToken, DeviceType.ANDROID);

                return new User()
                {
                    UserName = userName, FacebookAccessToken = accessToken, 
                    Friends = GetFacebookFriendList(userName).ToList(),
                    Status = UserStatus.Registered,
                    
                };
            }
            catch (App42Exception ex)
            {
                SocialExceptionHandling(ex);
                return null;
            }
        }
        /// <summary>
        /// URL example:
        /// http://localhost:22467/api/User/GetFacebookFriendList?userName=ana
        /// </summary>
        /// <param name="userName">Facebook userName of the user</param>
        /// <returns>list of friends as JSON or XML depending on which type the get call said it preferred</returns>
        [HttpGet]
        public IEnumerable<Friend> GetFacebookFriendList(string userName)
        {
            try
            {
                var social =BaaS.SocialService.GetFacebookFriendsFromLinkUser(userName);
                return social.GetFriendList().Select(x=>
                    new User()
                    {
                        UserName = x.name
                    });
            }
            catch(App42Exception ex)
            {
                SocialExceptionHandling(ex);

                // for testing purposes
                return TestRepository.Friends;
            }
        }

        /// <summary>
        /// Recommends some gifts
        /// URL example:
        /// http://localhost:22467/api/User/GetGiftRecommendation?friendUserName=ana
        /// </summary>
        /// <param name="friendUsername">Name of the friend to whom to recommend a gift for</param>
        /// <returns>List of gift recommendations</returns>
        public IEnumerable<Gift> GetGiftRecommendation(string friendUsername)
        {
            return TestRepository.Gifts.Take(3);
        }

        /// <summary>
        /// Recommends some gifts for a friend from specified gift category
        /// URL example:
        /// http://localhost:22467/api/User/GetGiftRecommendation?friendUserName=ana&category=Book
        /// </summary>
        /// <param name="friendUsername">Name of the friend to whom to recommend a gift for</param>
        /// <param name="category">Category of gift</param>
        /// <returns>List of gift recommendations</returns>
        public IEnumerable<Gift> GetGiftRecommendation(string friendUsername, GiftCategory category)
        {
            return TestRepository.Gifts.Where(x=>x.Category==category);
        }

        /// <summary>
        /// 1400 - BAD REQUEST - The Request parameters are invalid 
        /// 1401 - UNAUTHORIZED - Client is not authorized 
        /// 1500 - INTERNAL SERVER ERROR - Internal Server Error. Please try again 
        /// 3800 - NOT FOUND - Twitter App Credentials(ConsumerKey / ConsumerSecret) does not exist. 
        /// 3802 - NOT FOUND - Twitter User Access Credentials does not exist. Please use linkUserTwitterAccount API to link the User Twitter account. 
        /// 3803 - BAD REQUEST - The Twitter Access Credentials are invalid." + &lt;Exception Message&gt;. 
        /// 3804 - NOT FOUND - Facebook App Credentials(ConsumerKey/ConsumerSecret) does not exist. 
        /// 3805 - BAD REQUEST - The Facebook Access Credentials are invalid + &lt;Received Facebook Exception Message&gt;. 
        /// 3806 - NOT FOUND - Facebook User Access Credentials does not exist. Please use linkUserFacebookAccount API to link the User facebook account. 
        /// 3807 - NOT FOUND - LinkedIn App Credentials(ApiKey/SecretKey) does not exist. 
        /// 3808 - BAD REQUEST - The Access Credentials are invalid + &lt;Exception Message&gt;. 
        /// 3809 - NOT FOUND - LinkedIn User Access Credentials does not exist. Please use linkUserLinkedInAccount API to link the User LinkedIn account. 
        /// 3810 - NOT FOUND - Social App Credentials do not exist. 
        /// 3811 - NOT FOUND - User Social Access Credentials do not exist. Please use linkUserXXXXXAccount API to link the User Social account. 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string SocialExceptionHandling(App42Exception ex)
        {
            int appErrorCode = ex.GetAppErrorCode();
            int httpErrorCode = ex.GetHttpErrorCode();
            if (appErrorCode == 3800)
            {
                // Handle here for Not Found (Twitter App Credentials(ConsumerKey / ConsumerSecret) does not exist.)  
            }
            else if (appErrorCode == 3801)
            {
                // Handle here for Bad Request (The request is Unauthorized with the provided credentials.)  
            }
            else if (appErrorCode == 3802)
            {
                // Handle here for Not Found (Twitter User Access Credentials does not exist. Please use linkUserTwitterAccount API to link the User Twitter account.)  
            }
            else if (appErrorCode == 3803)
            {
                // Handle here for Bad Request (The Twitter Access Credentials are invalid.)  
            }
            else if (appErrorCode == 1401)
            {
                // handle here for Client is not authorized  
            }
            else if (appErrorCode == 1500)
            {
                // handle here for Internal Server Error  
            }
            return ex.GetMessage();    
        }
    }
}
