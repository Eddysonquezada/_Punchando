using System.Web;
using System.Web.Optimization;

namespace Proyecto_Monlic
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Content/js/vendor").Include(
                           "~/Content/js/vendor/bootstrap.min.js",
                           "~/Content/js/vendor/bootstrap.js",
                           "~/Content/js/vendor/datepicker.js",
                           "~/Content/js/vendor/plugins.js",
                           "~/Content/js/vendor/main.js",
                           "~/Content/js/vendor/jquery.dataTables.js",
                           "~/Content/js/vendor/dataTables.bootstrap4.js"
                           ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/bootstrap-theme.min.css",
                        "~/Content/css/fontAwesome.css",
                        "~/Content/css/light-box.css",
                        "~/Content/css/templatemo-main.css",
                        "~/Content/css/dataTables.bootstrap4.css"));
        }
    }
}
