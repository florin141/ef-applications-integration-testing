﻿using Globalmantics.DAL;
using Globalmantics.DAL.Migrations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Globalmantics.IntegrationTests
{
	[SetUpFixture]
	public class TestSetup
	{
		[OneTimeSetUp]
		public void SetUpDatabase()
		{
			ExecuteSqlCommand(Master, $@"
					CREATE DATABASE [Globalmantics]
					ON (NAME = 'Globalmantics',
					FILENAME = '{Filename}')");

			var migration = new MigrateDatabaseToLatestVersion<GlobalmanticsContext, GlobalmanticsConfiguration>();
			migration.InitializeDatabase(new GlobalmanticsContext());
		}

		[OneTimeTearDown]
		public void TearDownDatabase()
		{
			var fileNames = ExecuteSqlQuery(Master, @"
					SELECT [physical_name] FROM [sys].[master_files]
					WHERE [database_id] = DB_ID('Globalmantics')",
					row => (string)row["physical_name"]);

			ExecuteSqlCommand(Master, @"
					ALTER DATABASE [Globalmantics] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
					EXEC sp_detach_db 'Globalmantics'");

			fileNames.ForEach(File.Delete);
		}

		private static void ExecuteSqlCommand(
			SqlConnectionStringBuilder connectionStringBuilder,
			string commandText)
		{
			using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = commandText;
					command.ExecuteNonQuery();
				}
			}
		}

		private static List<T> ExecuteSqlQuery<T>(
			SqlConnectionStringBuilder connectionStringbuilder,
			string queryText,
			Func<SqlDataReader, T> read)
		{
			var result = new List<T>();
			using (var connection = new SqlConnection(connectionStringbuilder.ConnectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = queryText;
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(read(reader));
						}
					}
				}
			}

			return result;
		}

		private static SqlConnectionStringBuilder Master => new SqlConnectionStringBuilder
		{
			DataSource = @"(LocalDB)\MSSQLLocalDB",
			InitialCatalog = "master",
			IntegratedSecurity = true
		};

		private static string Filename => Path.Combine(
			Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
			"Globalmantics.mdf");
	}
}
