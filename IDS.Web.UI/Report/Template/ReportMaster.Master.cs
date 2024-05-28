using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Template
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session[Tool.GlobalVariable.SESSION_USER_ID] == null ||
                Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE] == null ||
                Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] == null)
                {
                    Response.Redirect("~/Login");
                }
            }

            // Akses Group dipindahkan ke page report yang mengimplementasikan template ini
            //int access = GetUserAccess();
            //if (access <= 0)
            //    Response.Redirect("~/Error/Error403");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string menu = IDS.Maintenance.UserMenu.ParseMenuToHTML();
            rawMenu.InnerHtml = menu;
        }

        private int GetUserAccess()
        {
            return IDS.Web.UI.Models.GroupAccessLevel.GetGroupAccessLevelByUrl(Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE] as string, Page.Request.Url.AbsolutePath.Replace(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath, "~"));
        }
		
		protected void userSignout_click(object sender, EventArgs e)
        {
            Session[IDS.Tool.GlobalVariable.SESSION_USER_AM_GROUP] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] = null;
            Session[IDS.Tool.GlobalVariable.SESSION_USER_MENU] = null;

            Session.Abandon();
            Response.Redirect("~/login");
        }
    }
}