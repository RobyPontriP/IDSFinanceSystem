using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace IDS.Maintenance
{
    public class UserMenu
    {
        [Display(Name = "Menu Number")]
        public int MenuNumber { get; set; }

        [Display(Name = "Menu Code")]
        public string MenuCode { get; set; }

        [Display(Name = "Menu Parent Code")]
        [Required(AllowEmptyStrings = true)]
        public string MenuParentCode { get; set; }

        [Display(Name = "Menu Project")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Menu project is required")]
        public string MenuProject { get; set; }

        [Display(Name = "Menu Level")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Menu level is required")]
        public int MenuLevel { get; set; }

        [Display(Name = "Menu Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Menu name is required")]
        public string MenuName { get; set; }

        [Display(Name = "Menu URL")]
        public string MenuURL { get; set; }

        [Display(Name = "Menu ToolTip")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Menu tooltip is required")]
        public string MenuToolTip { get; set; }

        [Display(Name = "Menu Controller")]
        public string Controller { get; set; }

        [Display(Name = "Menu Area")]
        public string Area { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "dd/MMM/YYYY HH:mm:ss", ConvertEmptyStringToNull = true, NullDisplayText = "")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(DataFormatString = "dd/MMM/YYYY HH:mm:ss", ConvertEmptyStringToNull = true, NullDisplayText = "")]
        public DateTime LastUpdate { get; set; }

        public UserMenu()
        {
        }

        public static List<UserMenu> GetParentMenu()
        {
            List<UserMenu> parentMenu = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT * FROM mntWebMenu where MenuLevel = 0 ORDER BY MenuCode";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        parentMenu = new List<UserMenu>();

                        while (dr.Read())
                        {
                            UserMenu menu = new UserMenu();
                            menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                            menu.MenuCode = dr["MenuCode"].ToString();
                            menu.MenuProject = dr["MenuProject"].ToString();
                            menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                            menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                            menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                            menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();
                            menu.Area = dr["Area"] as string;
                            menu.Controller = dr["Controller"] as string;

                            parentMenu.Add(menu);
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }

                    db.Close();
                }
            }

            return parentMenu;
        }

        public static List<UserMenu> GetChildMenu(string menuCode, string groupCode)
        {
            List<UserMenu> menuList = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelMenu";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, menuCode);
                db.AddParameter("@Grp", System.Data.SqlDbType.VarChar, groupCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        menuList = new List<UserMenu>();

                        while (dr.Read())
                        {
                            UserMenu menu = new UserMenu();
                            menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                            menu.MenuCode = dr["MenuCode"].ToString();
                            menu.MenuProject = dr["MenuProject"].ToString();
                            menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                            menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                            menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                            menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();
                            menu.Area = dr["Area"] == DBNull.Value ? "" : dr["Area"].ToString();
                            menu.Controller = dr["Controller"] == DBNull.Value ? "" : dr["Controller"].ToString();

                            menuList.Add(menu);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return menuList;
        }

        public static List<UserMenu> GetChildMenu(List<UserMenu> menuList, string menuCode, string groupCode)
        {
            if (menuList == null)
                menuList = new List<UserMenu>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelMenu";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, menuCode);
                db.AddParameter("@Grp", System.Data.SqlDbType.VarChar, groupCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserMenu menu = new UserMenu();
                            menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                            menu.MenuCode = dr["MenuCode"].ToString();
                            menu.MenuProject = dr["MenuProject"].ToString();
                            menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                            menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                            menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                            menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();
                            menu.Area = dr["Area"] == DBNull.Value ? "" : dr["Area"].ToString();
                            menu.Controller = dr["Controller"] == DBNull.Value ? "" : dr["Controller"].ToString();

                            menuList.Add(menu);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return menuList;
        }
        
        #region Pindahin ke Controller
        public static string ParseMenuToHTML()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] != null)
            {
                List<Maintenance.UserMenu> parentMenu = null;
                List<Maintenance.UserMenu> menus = new List<UserMenu>(System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] as List<Maintenance.UserMenu>);

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

        public static string ParseMenuToHTML(UrlHelper urlHelper)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] != null)
            {
                List<Maintenance.UserMenu> parentMenu = null;
                List<Maintenance.UserMenu> menus = new List<UserMenu>(System.Web.HttpContext.Current.Session[Tool.GlobalVariable.SESSION_USER_MENU] as List<Maintenance.UserMenu>);

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
                            //test = "dddddddddddd";
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
        #endregion


        public int InsUpDel(IDS.Tool.PageActivity ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if (ExecCode == Tool.PageActivity.Insert)
                    {
                        if (MenuLevel == 0)
                        {
                            cmd.CommandText = "SELECT DISTINCT ISNULL(MAX(LEFT(MenuCode, 2)), '00') FROM mntWebMenu";
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Open();

                            MenuCode = Convert.ToString(cmd.ExecuteScalar());

                            if (MenuCode == "00")
                                MenuCode = "01";

                            MenuCode = MenuCode.PadRight(14, '0');
                        }
                        else
                        {
                            MenuCode = MenuParentCode.Substring(0, MenuLevel * 2);

                            cmd.CommandText = "SELECT MAX(MenuCode) FROM mntWebMenu WHERE MenuCode LIKE @menuCode + '%' AND MenuLevel = @level";
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@menuCode", System.Data.SqlDbType.VarChar, MenuCode);
                            cmd.AddParameter("@level", System.Data.SqlDbType.TinyInt, MenuLevel);
                            cmd.Open();

                            MenuCode = Convert.ToString(cmd.ExecuteScalar());

                            if (MenuCode == "00")
                                MenuCode = "01";

                            MenuCode = MenuCode.PadRight(14, '0');
                        }
                    }

                    cmd.CommandText = "MntSaveMenu";
                    cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);

                    if (ExecCode == Tool.PageActivity.Insert)
                    {
                        cmd.AddParameter("@MenuNumber", System.Data.SqlDbType.BigInt, DBNull.Value);
                    }                        
                    else
                        cmd.AddParameter("@MenuNumber", System.Data.SqlDbType.BigInt, MenuNumber);

                    cmd.AddParameter("@MenuCode", System.Data.SqlDbType.VarChar, MenuCode);
                    cmd.AddParameter("@MenuName", System.Data.SqlDbType.VarChar, MenuName);
                    cmd.AddParameter("@MenuProject", System.Data.SqlDbType.VarChar, MenuProject);
                    cmd.AddParameter("@MenuLevel", System.Data.SqlDbType.TinyInt, MenuLevel);
                    cmd.AddParameter("@MenuUrl", System.Data.SqlDbType.VarChar, MenuURL);
                    cmd.AddParameter("@ControllerName", System.Data.SqlDbType.VarChar, Controller);
                    cmd.AddParameter("@AreaName", System.Data.SqlDbType.VarChar, Area);
                    cmd.AddParameter("@MenuToolTip", System.Data.SqlDbType.VarChar, MenuToolTip);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Currency code is already exists. Please choose other Currency code.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    throw;
                }
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        
        public static List<UserMenu> GetAllUserMenu()
        {
            List<UserMenu> menuList = new List<UserMenu>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                //db.CommandText = "MntSelMenuNoAccess";
                db.CommandText = "MntSelGenMenu";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        menuList = new List<UserMenu>();
                        
                        while (dr.Read())
                        {
                            UserMenu menu = new UserMenu();
                            menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                            menu.MenuCode = dr["MenuCode"].ToString();
                            menu.MenuProject = dr["MenuProject"].ToString();
                            menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                            menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                            menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                            menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();

                            menuList.Add(menu);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return menuList;
        }

        public static UserMenu GetUserMenuByMenuNumber(int menuNumber)
        {
            IDS.Maintenance.UserMenu menu = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelGenMenu";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@MenuProject", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@MenuLevel", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@MenuNumber", System.Data.SqlDbType.Int, menuNumber);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        menu = new UserMenu();
                        menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                        menu.MenuCode = dr["MenuCode"].ToString();
                        menu.MenuProject = dr["MenuProject"].ToString();
                        menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                        menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                        menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                        menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return menu;
        }

        public static List<UserMenu> GetUserMenuByProjectAndLevel(string menuProjectName, int menuLevel)
        {
            List<UserMenu> list = new List<UserMenu>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelGenMenu"; 
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@MenuProject", System.Data.SqlDbType.VarChar, menuProjectName);
                db.AddParameter("@MenuLevel", System.Data.SqlDbType.TinyInt, menuLevel);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserMenu menu = new UserMenu();
                            menu.MenuNumber = Convert.ToInt32(dr["MenuNumber"]);
                            menu.MenuCode = dr["MenuCode"].ToString();
                            menu.MenuProject = dr["MenuProject"].ToString();
                            menu.MenuLevel = Convert.ToInt32(dr["MenuLevel"]);
                            menu.MenuName = (dr["MenuName"] == DBNull.Value) ? "" : dr["MenuName"].ToString();
                            menu.MenuURL = dr["MenuUrl"] == DBNull.Value ? "" : dr["MenuUrl"].ToString();
                            menu.MenuToolTip = dr["MenuToolTip"] == DBNull.Value ? "" : dr["MenuToolTip"].ToString();
                            menu.Area = dr["Area"] as string;
                            menu.Controller = dr["Controller"] as string;

                            list.Add(menu);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static int GetMenuNumberFromMenuName(string MenuName)
        {
            int return_ = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT TOP 1 MenuNumber FROM MntWebMenu where MenuName=@MenuName";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@MenuName", System.Data.SqlDbType.VarChar, MenuName);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = Tool.GeneralHelper.NullToInt(dr["MenuNumber"], 0);
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return return_;
        }


        public static bool DeleteMenuFromMenuNumber(string MenuNumber, string MenuCode, string MenuName)
        {
            bool return_ = false;
            int result = 0;
            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {

                    cmd.CommandText = "DELETE FROM MntWebMenu  WHERE MenuNumber = @MenuNumber and MenuCode=@MenuCode and MenuName=@MenuName";
                    cmd.AddParameter("@MenuNumber", System.Data.SqlDbType.VarChar, MenuNumber);
                    cmd.AddParameter("@MenuCode", System.Data.SqlDbType.VarChar, MenuCode);
                    cmd.AddParameter("@MenuName", System.Data.SqlDbType.VarChar, MenuName);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                    return_ = true;
                }
                catch (SqlException sex)
                {
                    return false;
                }
                cmd.Close();
            }
            return return_;
        }

    }
}