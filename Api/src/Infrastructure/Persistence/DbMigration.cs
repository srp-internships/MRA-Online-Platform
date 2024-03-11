namespace Infrastructure.Persistence
{
    public class DbMigration : IMigration
    {
        private readonly ApplicationDbContext context;

        public DbMigration(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Migrate()
        {
            if(context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
        }
    }

    public interface IMigration
    {
        void Migrate();
    }
}
