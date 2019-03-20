using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NetMentoring_noSQL.Collections;

namespace NetMentoring_noSQL
{
    public class MongoDAL
    {
      
        
        private IMongoCollection<Book> collection { get; set; }

        private BsonDocument filter { get; set; }

        public MongoDAL(string connection)
        {
            var client = new MongoClient(connection);
            var db = client.GetDatabase("Library");
            collection = db.GetCollection<Book>("books");
            filter = new BsonDocument();
        }

        public async Task AddBooks(IEnumerable<Book> items)
        {
            await collection.InsertManyAsync(items);
        }
        public async Task<List<Book>> GetAllBooks()
        {
            return await collection.Find(filter)
                                   .Sort(Builders<Book>.Sort.Ascending("Name"))
                                   .ToListAsync();
        }


        public  async Task<List<Book>> GetBooksByCount(int count)
        {
          return await collection.Find(book=> book.Count > count)
                                                      .Sort(Builders<Book>.Sort.Ascending("Name"))
                                                      .Limit(3)
                                                      .ToListAsync();
        }

        public async Task<Book> GetBookWithMaxCount()
        {
            return await collection
                        .Find(filter)
                        .Sort(Builders<Book>.Sort.Descending("Count"))
                        .FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookWithMinCount()
        {
            return await collection
                        .Find(filter)
                        .Sort(Builders<Book>.Sort.Ascending("Count"))
                        .FirstOrDefaultAsync();
        }

        public async Task<HashSet<string>> GetBookAuthors()
        {
            HashSet<string> authors = new HashSet<string>();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var books = cursor.Current;
                    foreach (var item in books)
                    {
                        if(!string.IsNullOrEmpty(item.Author))

                            authors.Add(item.Author);
                    }
                }
            }
            return authors;
        }

        public async Task<List<Book>> GetBookWithoutAuthors()
        {
            return await collection
                        .Find(book => string.IsNullOrEmpty(book.Author))
                        .ToListAsync();
                        
        }
        public async Task IncreaseBookCount(int count)
        {
            var updoneresult = await collection.UpdateManyAsync(
                               filter,
                               Builders<Book>.Update.Inc("Count", count));
        }
        public async Task AddNewGenre()
        {
            var updoneresult = await collection.UpdateManyAsync(
                               book=>(book.Genre== "fantasy"&& book.Genre != "favority"),
                               Builders<Book>.Update.Set("Genre", "favority"));
        }

        public async Task<long> DeleteBookWithCount(int count)
        {
            var deleteResult = await collection.DeleteManyAsync(
                             b=>b.Count<count);
            return deleteResult.DeletedCount;
        }

        public async Task DeleteAllBooks()
        {
            var deleteResult = await collection.DeleteManyAsync(filter);
        }

      


    }
}


