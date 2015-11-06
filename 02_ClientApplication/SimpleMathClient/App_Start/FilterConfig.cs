namespace SimpleMathClient
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// FilterConfig class
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// RegisterGlobalFilters static method
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}