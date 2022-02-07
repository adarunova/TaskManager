using System;
using TaskManager.Models;
using TaskManager.Tables;

namespace TaskManager.Services
{
    //[Serializable]
    public static class IdentityService
    {
        public static ProfileType ProfileType { get; set; }

        public static CompanyTable Company { get; set; }

        public static UserTable User { get; set; }

        static IdentityService()
        {
            ProfileType = ProfileType.None;
        }
    }
}
