using UI.Models;

namespace UI.Services
{
    public class UserSessionService
    {
        public CurrentUser? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser != null;

        public bool IsAdmin => string.Equals(CurrentUser?.Role, "Admin", StringComparison.OrdinalIgnoreCase);
        public bool IsEditor => string.Equals(CurrentUser?.Role, "Editor", StringComparison.OrdinalIgnoreCase);
        public bool IsTeacher => string.Equals(CurrentUser?.Role, "Teacher", StringComparison.OrdinalIgnoreCase);
        public bool IsStudent => string.Equals(CurrentUser?.Role, "Student", StringComparison.OrdinalIgnoreCase);
        public bool IsManagement => string.Equals(CurrentUser?.Role, "Management", StringComparison.OrdinalIgnoreCase);

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
