using UI.Models;

namespace UI.Services
{
    public class UserSessionService
    {
        public CurrentUser? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser != null;

        public bool IsAdmin => CurrentUser?.Role == "Admin";
        public bool IsEditor => CurrentUser?.Role == "Editor";
        public bool IsTeacher => CurrentUser?.Role == "Teacher";
        public bool IsStudent => CurrentUser?.Role == "Student";
        public bool IsManagement => CurrentUser?.Role == "Management";

        public void SetUser(CurrentUser user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
