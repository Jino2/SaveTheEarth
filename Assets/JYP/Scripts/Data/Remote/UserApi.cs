    using System;
    
    public struct UserApi
    {
        public static string BASE_URL = "https://api.jypshop.com";

        
        public static void Register(string id, string password, string name, Action<string> onComplete)
        {
            
        }
        
        public static void Login(string id, string password, Action<string> onComplete)
        {
            
        }
        
        public static void GetUserInfo(string userId, Action<UserInfo> onComplete)
        {
            
        }
    }