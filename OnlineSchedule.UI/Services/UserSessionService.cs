using UI.Models;

namespace UI.Services
{
    public class UserSessionService
    {
        public CurrentUser? CurrentUser { get; set; }

        public bool IsAuthenticated => CurrentUser != null;

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
