 

namespace WriteDry.Utils
{
    public static class UserFIO
    {
        public static string GetFIO(User user) => string.Join(" ", user.UserSurname, user.UserName, user.UserPatronymic);
    }
}
