using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class RemoteUserDetail
    {
        string userId;
        public string UserId
        {
            get { return userId; }
        }
        string displayName;
        public string DisplayName
        {
            get { return displayName; }
        }
        string avatarUrl;
        public string AvatarUrl
        {
            get { return avatarUrl; }
        }

        public RemoteUserDetail(string userId, string displayName, string avatarUrl)
        {
            this.userId = userId;
            this.displayName = displayName;
            this.avatarUrl = avatarUrl;
        }
    }

}
