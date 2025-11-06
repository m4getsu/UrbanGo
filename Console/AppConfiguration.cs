namespace ConsoleApp
{
    public class AppConfiguration : IConfiguration
    {
        public string ConnectionString { get; }

        public AppConfiguration(string? connectionString = null)
        {
            ConnectionString = connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";
        }
    }
}
