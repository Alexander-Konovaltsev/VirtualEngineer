namespace VirtualEngineer.Services
{
    public static class SessionService
    {
        public static string AccessToken { get; private set; }

        public static bool IsAuthorized => !string.IsNullOrEmpty(AccessToken);

        public static void SetToken(string token)
        {
            AccessToken = token;
        }

        public static void Clear()
        {
            AccessToken = null;
        }
    }
}
