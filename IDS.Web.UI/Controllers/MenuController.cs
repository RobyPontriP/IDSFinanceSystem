using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Routing;
using System.Web.Mvc.Razor;

namespace IDS.Web.UI.Controllers
{
    public class MenuController : Controller
    {
        protected string MainMenu = "";

        public MenuController()
        {
            MainMenu = ParseMenuToHTML();
        }

        public static string ParseMenuToHTML()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] != null)
            {
                List<Maintenance.UserMenu> parentMenu = null;
                List<Maintenance.UserMenu> menus = new List<IDS.Maintenance.UserMenu>(System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] as List<Maintenance.UserMenu>);

                if (menus != null && menus.Count > 0)
                {
                    parentMenu = new List<Maintenance.UserMenu>(menus.Where(x => x.MenuLevel == 0).ToList());
                    sb.Append("<ul class=\"nav nav-pills nav-sidebar flex-column nav-child-indent\" data-widget=\"treeview\" role=\"menu\" data-accordion=\"false\">");

                    foreach (Maintenance.UserMenu menu in parentMenu)
                    {
                        //sb.Append("<li class=\"nav-header\">" + menu.MenuName + "</li>");
                        sb.Append("<li class=\"nav-item\">")
                            .Append("<a href=\"#\" class=\"nav-link\">")
                                .Append("<i class=\"nav-icon fas fa-circle\"></i>")
                                    .Append("<p>" + menu.MenuName + "</p>")
                                .Append("<i class=\"right fas fa-angle-left\"></i>")
                            .Append("</a>");

                        if (menus.Where(x => x.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) == menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2)).Count() > 0)
                        {
                            //sb.Append("<ul class=\"nav nav-treeview\">");

                            ProcessChildMenu(sb, menus, menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2), menu.MenuLevel);

                            //sb.Append("</ul>");
                        }

                        sb.Append("</li>");
                    }

                    sb.Append("</ul>");
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private static System.Text.StringBuilder ProcessChildMenu(System.Text.StringBuilder sb, List<Maintenance.UserMenu> menus, string parentCode, int level)
        {
            if (menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00").Count() > 0)
            {
                sb.Append("<ul class=\"nav nav-treeview\">");

                string menuUrl = "";

                foreach (Maintenance.UserMenu menu in menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00" && x.MenuCode.Substring((level + 2) * 2, 2) == "00").ToList())
                {
                    switch (menu.MenuURL != null && !string.IsNullOrEmpty(menu.MenuURL) && menu.MenuURL.StartsWith("~", StringComparison.CurrentCultureIgnoreCase))
                    {
                        case true:
                            //test = (HtmlHelper.GenerateLink(System.Web.HttpContext.Current.Request.RequestContext, System.Web.Routing.RouteTable.Routes, "Coba Link", "Root", "About", "Home", null, null) + "\" class=\"nav-link\">");
                            menuUrl = VirtualPathUtility.ToAbsolute(menu.MenuURL) + "?rpt=" + IDS.Tool.UrlEncryption.EncryptParam(menu.MenuCode);
                            break;
                        default:
                            menuUrl = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("index", menu.Controller, new { Area = menu.Area });
                            break;
                    };

                    sb.Append("<li class=\"nav-item\">")
                        //.Append("<a href=\"" + menu.MenuURL + "\" class=\"nav-link\">")
                        .Append("<a href=\"" + menuUrl + "\" class=\"nav-link\">")
                        .Append("<i class=\"nav-icon fas fa-circle\"></i>")
                        .Append("<p>")
                        .Append(menu.MenuName);

                    if (menus.Where(x => x.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) == menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) && x.MenuCode.Substring((menu.MenuLevel + 1) * 2, 2) != "00").Count() > 0)
                    {
                        sb.Append("<i class=\"right fas fa-angle-left\"></i>")
                        .Append("</p>")
                        .Append("</a>");

                        ProcessSubChildMenu(sb, menus, menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2), menu.MenuLevel);
                    }
                    else
                    {
                        sb.Append("</p>")
                        .Append("</a>");
                    }

                    sb.Append("</li>");
                }

                sb.Append("</ul>");
            }

            return sb;
        }

        private static System.Text.StringBuilder ProcessSubChildMenu(System.Text.StringBuilder sb, List<Maintenance.UserMenu> menus, string parentCode, int level)
        {
            if (menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00").Count() > 0)
            {
                sb.Append("<ul class=\"nav nav-treeview\">");

                string menuUrl = "";

                foreach (Maintenance.UserMenu menu in menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00" && x.MenuCode.Substring((level + 2) * 2, 2) == "00").ToList())
                {
                    switch (menu.MenuURL != null && !string.IsNullOrEmpty(menu.MenuURL) && menu.MenuURL.StartsWith("~", StringComparison.CurrentCultureIgnoreCase))
                    {
                        case true:
                            //test = (HtmlHelper.GenerateLink(System.Web.HttpContext.Current.Request.RequestContext, System.Web.Routing.RouteTable.Routes, "Coba Link", "Root", "About", "Home", null, null) + "\" class=\"nav-link\">");
                            //test = "dddddddddddd";
                            menuUrl = VirtualPathUtility.ToAbsolute(menu.MenuURL) + "?rpt=" + IDS.Tool.UrlEncryption.EncryptParam(menu.MenuCode);
                            break;
                        default:
                            menuUrl = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("index", menu.Controller, new { Area = menu.Area });
                            break;
                    };

                    sb.Append("<li class=\"nav-item\">")
                        //.Append("<a href=\"" + menu.MenuURL + "\" class=\"nav-link\">")
                        //.Append("<a href=\"" + new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("index", menu.Controller, new { Area = menu.Area }) + "\" class=\"nav-link\">")
                        .Append("<a href=\"" + menuUrl + "\" class=\"nav-link\">")
                        .Append("<i class=\"nav-icon fas fa-circle\"></i>")
                        .Append("<p>")
                        .Append(menu.MenuName);


                    if (menus.Where(x => x.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) == menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) && x.MenuCode.Substring((menu.MenuLevel + 1) * 2, 2) != "00").Count() > 0)
                    {
                        sb.Append("<i class=\"right fas fa-angle-left\"></i>")
                        .Append("</p>")
                        .Append("</a>");

                        ProcessSubChildMenu(sb, menus, menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2), menu.MenuLevel);
                    }
                    else
                    {
                        sb.Append("</p>")
                        .Append("</a>");
                    }

                    sb.Append("</li>");
                }

                sb.Append("</ul>");
            }

            return sb;
        }
    }
}