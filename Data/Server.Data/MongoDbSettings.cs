namespace Server.Data
{
  public class MongoDbSettings
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public bool IsSSL { get; set; }
  }
}
