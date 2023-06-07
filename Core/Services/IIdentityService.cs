using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public interface IIdentityService
    {
        public string GetUserID { get; }
    }
}
