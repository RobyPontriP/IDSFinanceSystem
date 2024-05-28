using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult index()
        {
            try
            {
                //if (IDSLicensing.License.ValidateLicense("FISCUS", this.Server.MapPath("~") + "\\"))
                if (true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorLicense", "Error");
                }
            }
            catch
            {
                return RedirectToAction("ErrorLicense", "Error");
            }
            
        }

        [HttpPost]
        public ActionResult index(Models.UserLogin login)
        {           
            if (User != null)
            {
                ViewBag.ValidationResult = "";
                bool isValid = false;

                if (string.IsNullOrWhiteSpace(login.UserID) || string.IsNullOrWhiteSpace(login.Password))
                {
                    ViewBag.ValidationResult = "Incorrect user ID or Password";
                    isValid = false;
                }
                else
                {
                    Tool.clsCryptho crypt = new Tool.clsCryptho();
                    string encryptPassword = crypt.Encrypt(login.Password, "ids");
                    Maintenance.User user = Maintenance.User.UserLogin(login.UserID, encryptPassword);
                    
                    if (user != null)
                    {
                        if (user.Status == Maintenance.UserStatus.InActive)
                        {
                            ViewBag.ValidationResult = "Your account is not active or has been block. Please contact your administrator.";
                            isValid = false;

                            return View(login);
                        }
                        else if (user.Akumulasi > 10)
                        {
                            ViewBag.ValidationResult = "Your account has been block cause of failed login attempt. Please contact your administrator.";
                            isValid = false;

                            return View(login);
                        }
                        else if (!string.IsNullOrWhiteSpace(user.ExpiredCode))
                        {
                            ViewBag.ValidationResult = "Your account has been expired. Please contact your administrator.";
                            isValid = false;

                            return View(login);
                        }
                        else
                        {
                            Session[Tool.GlobalVariable.SESSION_USER_ID] = user.UserID;
                            Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE] = user.UserGroup.GroupCode;
                            Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] = user.Branch.BranchCode;
                            Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] = user.Branch.HOStatus;
                            // TODO: Disesuaikan dengan user.BranchCode.
                            //Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] = ;
                            
                            // Retrieve User Group Menu
                            List<Maintenance.UserMenu> userMenus = Maintenance.UserMenu.GetParentMenu();
                            List<Maintenance.UserMenu> userMenusClone = new List<Maintenance.UserMenu>(userMenus);

                            Dictionary<string, List<IDS.Maintenance.GroupAccess>> cache = IDS.Tool.InMemoryCache.GetInstance().GetOrSet(IDS.Tool.GlobalVariable.CACHE_USER_GROUP_ACCESS, user.UserGroup.GroupCode, () => IDS.Maintenance.GroupAccess.GetGroupAccess(user.UserGroup.GroupCode), IDS.Tool.GlobalVariable.CACHE_DURATION_USER_GROUP_ACCESS);

                            if (cache == null)
                            {
                                // TODO: Redirect ke UnAuthorize
                            }

                            if (userMenus != null)
                            {
                                foreach (Maintenance.UserMenu menu in userMenusClone)
                                {
                                    userMenus = Maintenance.UserMenu.GetChildMenu(userMenus, menu.MenuCode.Substring(0, 2), user.UserGroup.GroupCode);
                                }
                            }

                            Session[Tool.GlobalVariable.SESSION_USER_MENU] = userMenus;

                            return RedirectToAction("index", "Main");
                        }
                    }
                    else
                    {
                        isValid = false;
                        ViewBag.ValidationResult = "Incorrect user ID or Password";

                        return View(login);
                    }
                }
            }

            return View();
        }
        
    }
}