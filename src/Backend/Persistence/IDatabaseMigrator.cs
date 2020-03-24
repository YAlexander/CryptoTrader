namespace Persistence
{
    public interface IDatabaseMigrator
    {
		void Migrate (string connectionString, string location, bool isisDropAllowed);
    }
}
