using System.Web.Optimization;

namespace Psps.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/assets/jquery/1.11.2/js/jquery-1.11.2.js",
                "~/Content/assets/jquery-validation/1.11.1/js/jquery.validate.min.js",
                "~/Content/assets/jquery-validation/1.11.1/js/additional-methods.min.js",
                "~/Content/assets/jquery-ui/1.10.3/js/jquery-ui-1.10.3.custom.min.js",
                "~/Content/assets/bootstrap/3.1.1/js/bootstrap.min.js",
                "~/Content/assets/moment/2.8.4/js/moment.js",
                "~/Content/assets/jqGrid/4.6.0/js/jquery.jqGrid.src.js",
                "~/Content/assets/jqGrid/4.6.0/js/i18n/grid.locale-en.js",
                "~/Content/assets/jquery-slimScroll/1.3.1/js/jquery.slimscroll.js",
                "~/Content/assets/select2/3.5.0/js/select2.js",
                "~/Content/assets/datepicker/1.3.1/js/bootstrap-datepicker.js",
                "~/Content/assets/bootstrap-timepicker/0.2.3/js/bootstrap-datetimepicker.js",
                "~/Content/assets/bootbox/4.1.0/js/bootbox.js",
                "~/Content/assets/jquery-placeholder/2.0.7/js/jquery.placeholder.js",
                "~/Content/assets/blockUI/2.70.0/js/jquery.blockUI.js",
                "~/Content/assets/jquery-goup/0.1.6/js/jquery.goup.min.js",
                "~/Content/assets/notifIt/1.1.0/js/notifIt.js",
                "~/Content/assets/jquery.form/3.51.0/js/jquery.form.js",
                "~/Content/assets/spellchecker/0.2.4/js/jquery.spellchecker.js",
                "~/Content/assets/jquery.inputmask/3.2.7/jquery.inputmask.bundle.js", 
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.js",
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.extensions.js",
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.date.extensions.js",
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.numeric.extensions.js",
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.phone.extensions.js",
                //"~/Content/assets/jquery.inputmask/3.1.61/js/jquery.inputmask.regex.extensions.js",
                "~/Content/assets/tongwen/tongwen_core.js",
                "~/Content/assets/tongwen/tongwen_table_t2s.js",
                "~/Content/assets/summernote/0.8.1/js/summernote.js",
                "~/Content/assets/summernote/0.8.1/js/summernote-ext-editor.js",
                "~/Content/js/data.js",
                "~/Content/js/main.js",
                "~/Content/js/jqGrid.columnTemplates.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/assets/font-awesome/3.2.1/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/assets/summernote/0.8.1/css/summernote.css",  new CssRewriteUrlTransform())
                .Include(
                    "~/Content/assets/bootstrap/3.1.1/css/bootstrap.css",
                    "~/Content/assets/jquery-ui/1.10.3/css/jquery-ui-1.10.3.full.min.css",
                    "~/Content/assets/jqGrid/4.6.0/css/ui.jqgrid.css",
                    "~/Content/assets/select2/3.5.0/css/select2.css",
                    "~/Content/assets/datepicker/1.3.1/css/datepicker3.css",
                    "~/Content/assets/bootstrap-timepicker/0.2.3/css/bootstrap-datetimepicker.css",
                    "~/Content/assets/notifIt/1.1.0/css/notifIt.css",
                    "~/Content/assets/ace/1.1.2/css/ace.css",
                    "~/Content/css/main.css",
                    "~/Content/css/orange-skin.css",
                    "~/Content/css/non-responsive.css",
                    "~/Content/assets/spellchecker/0.2.4/css/jquery.spellchecker.css"));

            bundles.Add(new StyleBundle("~/bundles/css-ie8").Include(
                "~/Content/assets/ace/1.1.2/css/ace-ie.css"));

            bundles.Add(new StyleBundle("~/bundles/js-ie8").Include(
                "~/Content/assets/respond/1.4.2/js/respond.min.js",
                "~/Content/assets/html5shiv/3.7.0/js/html5shiv.js"));
        }
    }
}