namespace Infrastructure.Identity
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";

        //pattern for controller: "Administrator + ", " + User"

        //public const string AdminOrUser = Admin + ", " + User;
        //public const string UserOrMod = Moderator + ", " + User;

        //public const string AdminOrMod = Admin + ", " + Moderator;

        //public const string AdminOrUserOrMod = User + ", " + Moderator + ", " + Admin;
    }
}