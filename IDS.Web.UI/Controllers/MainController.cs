using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Controllers
{
    public class MainController : MenuController
    {
        // GET: Main
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "login");

            #region Generate Menu
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //if (Session[Tool.GlobalVariable.SESSION_USER_MENU] != null)
            //{
            //    List<Maintenance.UserMenu> menus = Maintenance.UserMenu.GetParentMenu();
            //    List<Maintenance.UserMenu> parentMenu = new List<Maintenance.UserMenu>(menus);

            //    foreach (Maintenance.UserMenu menu in menus.Where(x => x.MenuLevel == 0).ToList())
            //    {
            //        Maintenance.UserMenu.GetChildMenu(menus, menu.MenuCode.Substring(0, 2), Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE].ToString());
            //    }

            //    if (menus.Count > 0)
            //    {
            //        sb.Append("<ul class=\"nav nav-pills nav-sidebar flex-column\" data-widget=\"treeview\" role=\"menu\" data-accordion=\"false\">");

            //        foreach (Maintenance.UserMenu menu in parentMenu)
            //        {
            //            sb.Append("<li class=\"nav-header\">" + menu.MenuName + "</li>");

            //            if (menus.Where(x => x.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2) == menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2)).Count() > 0)
            //            {
            //                ProcessChildMenu(sb, menus, menu.MenuCode.Substring(0, (menu.MenuLevel + 1) * 2), menu.MenuLevel);
            //            }
            //        }
            //        sb.Append("</nav>");
            //    }

            //    ViewBag.UserMenu = sb.ToString();

            //}
            #endregion
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            ViewBag.UserMenu = MainMenu;
        
            return View();
        }

        private System.Text.StringBuilder ProcessChildMenu(System.Text.StringBuilder sb, List<Maintenance.UserMenu> menus, string parentCode, int level)
        {
            if (menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00" ).Count() > 0)
            {
                foreach (Maintenance.UserMenu menu in menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00" && x.MenuCode.Substring((level + 2) * 2, 2) == "00").ToList())
                {
                    sb.Append("<li class=\"nav-item\">")
                        .Append("<a href=\"" + menu.MenuURL + "\" class=\"nav-link\">")
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
            }

            return sb;
        }

        private System.Text.StringBuilder ProcessSubChildMenu(System.Text.StringBuilder sb, List<Maintenance.UserMenu> menus, string parentCode, int level)
        {
            if (menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00").Count() > 0)
            {
                sb.Append("<ul class=\"nav nav-treeview\">");

                foreach (Maintenance.UserMenu menu in menus.Where(x => x.MenuCode.Substring(0, (level + 1) * 2) == parentCode && x.MenuCode.Substring((level + 1) * 2, 2) != "00" && x.MenuCode.Substring((level + 2) * 2, 2) == "00").ToList())
                {
                    sb.Append("<li class=\"nav-item\">")
                        .Append("<a href=\"" + menu.MenuURL + "\" class=\"nav-link\">")
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

        public ActionResult Signout()
        {
            Session[IDS.Tool.GlobalVariable.SESSION_USER_AM_GROUP] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_MENU] = null;

            Session.Abandon();
            return RedirectToAction("index", "Login", new { area = "" });
        }
    }
}