namespace Demo.Adventures.Database
{
    public class RepositoryConfig
    {
        public const string ConfigurationSectionName = "Repository";

        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}