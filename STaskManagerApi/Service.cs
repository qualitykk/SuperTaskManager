using STaskManagerLibrary;

namespace STaskManagerApi
{
    internal static class Service
    {
        internal static class Auth
        {
            public static bool IsActive()
            {
                throw new NotImplementedException();
            }
            public static async Task<AuthResponse> TryLogon(string user, string pass)
            {
                throw new NotImplementedException();
            }

            public static async Task<AuthResponse> Verify(string auth, int userid)
            {
                throw new NotImplementedException();
            }
        }
    }
}
