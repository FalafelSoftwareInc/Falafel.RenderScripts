using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace System.Web.Mvc
{
    public enum AssetHierarchy
    {
        Layout = 0,
        Page = 1,
        Partial = 2
    }

    public static class HtmlHelperExtensions
    {
        private class Asset
        {
            public AssetHierarchy AssetHierarchy { get; set; }
            public string Url { get; set; }
        }

        private static List<Asset> Scripts
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("user-request-script-items"))
                {
                    HttpContext.Current.Items["user-request-script-items"] = new List<Asset>();
                }

                return (HttpContext.Current.Items["user-request-script-items"] as List<Asset>);
            }
        }

        private static List<Asset> Styles
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("user-request-style-items"))
                {
                    HttpContext.Current.Items["user-request-style-items"] = new List<Asset>();
                }

                return (HttpContext.Current.Items["user-request-style-items"] as List<Asset>);
            }
        }

        /// <summary>
        /// Renders the scripts onto the page that were added by the view and partial views.
        /// </summary>
        /// <param name="html">The HTMLHelper</param>
        /// <returns></returns>
        public static MvcHtmlString RenderScripts(this HtmlHelper html)
        {
            var sb = new StringBuilder();
            foreach (var script in Scripts.OrderBy(i => i.AssetHierarchy))
            {
                sb.AppendLine(String.Format("<script src=\"{0}\"></script>", UrlHelper.GenerateContentUrl(script.Url, html.ViewContext.HttpContext)));
            }
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Renders the stylesheets onto the page that were added by the view and partial views.
        /// </summary>
        /// <param name="html">The HTMLHelper</param>
        /// <returns></returns>
        public static MvcHtmlString RenderStyleSheets(this HtmlHelper html)
        {
            var sb = new StringBuilder();
            //Styles.Reverse();
            foreach (var style in Styles.OrderBy(i => i.AssetHierarchy))
            {
                sb.AppendLine(String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\">", UrlHelper.GenerateContentUrl(style.Url, html.ViewContext.HttpContext)));
            }
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Adds a script to be emitted by the RenderScripts method.
        /// </summary>
        /// <param name="html">The HTMLHelper.</param>
        /// <param name="scriptUrl">The script URL.</param>
        /// <param name="scriptUrl">The display priority.</param>
        public static void AddScript(this HtmlHelper html, string scriptUrl, AssetHierarchy hierarchy = AssetHierarchy.Page)
        {
            Scripts.Add(new Asset { Url = scriptUrl  , AssetHierarchy = hierarchy});
        }

        /// <summary>
        /// Adds a stylesheet to be emitted by the RenderStyleSheets method.
        /// </summary>
        /// <param name="html">The HTMLHelper</param>
        /// <param name="styleUrl">The stylesheet URL.</param>
        /// <param name="scriptUrl">The display priority.</param>
        /// 
        public static void AddStyleSheet(this HtmlHelper html, string styleUrl, AssetHierarchy hierarchy = AssetHierarchy.Page)
        {
            Styles.Add(new Asset { Url = styleUrl, AssetHierarchy = hierarchy });
        }
    }
}
