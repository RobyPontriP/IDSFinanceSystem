using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Tool
{
    public class GlobalVariable
    {
        #region User Login Session

        /// <summary>
        /// Nama session untuk data User ID yang login
        /// </summary>
        public const string SESSION_USER_ID = "Login.UserID";
        /// <summary>
        /// Nama session group code untuk user yang login
        /// </summary>
        public const string SESSION_USER_GROUP_CODE = "Logi.UserGroupCode";
        /// <summary>
        /// Nama session branch code untuk user yang login
        /// </summary>
        public const string SESSION_USER_BRANCH_CODE = "Login.UserBranchCode";
        /// <summary>
        /// Nama session untuk check apakah User merupakan User branch holding atau bukan. 0 = Bukan Holding, 1 = Holding
        /// </summary>
        public const string SESSION_USER_BRANCH_HO_STATUS = "Login.HOStatus";
        /// <summary>
        /// Nama session untuk group code dari group AM (Area Manager) jika user group dari user yang login adalah user group AM
        /// </summary>
        public const string SESSION_USER_AM_GROUP = "Login.AM_GROUP";
        /// <summary>
        /// Nama session untuk menu user
        /// </summary>
        public const string SESSION_USER_MENU = "Login.UserMenu";

        #endregion
        
        #region Cache - User Access

        public const string CACHE_USER_GROUP_ACCESS = "UserGroup.Access";
        public const int CACHE_DURATION_USER_GROUP_ACCESS = 720;

        #endregion

        #region Datetime
        public const string DEFAULT_DATE_FORMAT = "dd/MMM/yyyy";
        public const string DEFAULT_DATETIME_FORMAT = "dd/MMM/yyyy HH:mm:ss";
        #endregion
    }
}
