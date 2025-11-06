namespace AIS
{
	public class AppConfiguration
	{
		public string ConnectionString { get; }

		public AppConfiguration(string? connectionString = null)
		{
			ConnectionString = connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";
		}
	}
}
