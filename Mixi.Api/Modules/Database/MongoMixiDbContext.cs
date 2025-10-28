using Mixi.Api.Modules.Notes;
using Mixi.Api.Modules.Pdf;
using MongoDB.Driver;


namespace Mixi.Api.Modules.Database;

public class MongoMixiDbContext
{ 
   public IMongoDatabase Database { get; }

   public MongoMixiDbContext(IMongoClient client, string databaseName)
   {
       Database = client.GetDatabase(databaseName);
   }
   
   public IMongoCollection<PdfDocument> PdfDocuments => Database.GetCollection<PdfDocument>("PdfDocuments");
   public IMongoCollection<Note>  NotesCollection => Database.GetCollection<Note>("Notes");
}