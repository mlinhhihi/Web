using System.Web;
using System.Web.Mvc;

namespace QLBanHangGauBong_65131773
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
