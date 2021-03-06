﻿using System.Collections.Generic;
using Npgsql;

namespace core.Infrastructure.Database.Migrations
{
	public class EvolveMigrator : IDatabaseMigrator
	{
		public void Migrate (string connectionString, string location, bool isDropAllowed)
		{
			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				Evolve.Evolve evolve = new Evolve.Evolve(connection)
				{
					Locations = new List<string> { location },
					IsEraseDisabled = !isDropAllowed,
				};

				evolve.Migrate();
			}
		}
	}
}
