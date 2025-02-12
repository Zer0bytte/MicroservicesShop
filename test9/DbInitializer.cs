using InstagramDMs.API.Data;

namespace InstagramDMs.API
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();


        }
    }
}
