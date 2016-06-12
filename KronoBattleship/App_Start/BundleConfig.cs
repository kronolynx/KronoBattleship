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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/ChatScript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/battle").Include(
                    "~/Scripts/jquery-ui-1.11.4.min.js",
                    "~/Scripts/jquery.signalR-2.2.0.min.js",
                    "~/Scripts/handlebars.min.js",
                    "~/Scripts/BattleScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
                    "~/Scripts/jquery.signalR-2.2.0.min.js",
                    "~/Scripts/handlebars.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate*"));


            BundleTable.EnableOptimizations = true;
        }
    }
}
