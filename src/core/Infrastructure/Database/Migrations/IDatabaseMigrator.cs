namespace core.Infrastructure.Database.Migrations
{
	public interface IDatabaseMigrator
	{
		void Migrate (string connectionString, string location, bool isisDropAllowed);
	}
}
