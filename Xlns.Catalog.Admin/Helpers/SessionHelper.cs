using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xlns.Catalog.Admin.Helpers
{
    public static class SessionHelper
    {
        public static string GetCountryId(this HttpSessionStateBase session) 
        {
            return session["countryId"].ToString();
        }

        public static void SetCountryId(this HttpSessionStateBase session, string countryId)
        {
            session["countryId"] = countryId;
        }
    }
}