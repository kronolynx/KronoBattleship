using System.Web;
using System.Web.Optimization;

namespace KronoBattleship
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.validate*",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/ChatScript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/Site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
