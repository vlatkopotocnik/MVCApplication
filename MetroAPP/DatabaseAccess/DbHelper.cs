using System.Configuration;

namespace MetroAPP.DatabaseAccess
{
    public class DbHelper
    {
        public static string ConnString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            }
        }
    }
}