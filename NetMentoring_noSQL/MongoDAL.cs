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
        private string _connection;
        IMongoDatabase db;
        public MongoDAL(string connection)
        {
            _connection = connection;
            var client = new MongoClient(_connection);

            db = client.GetDatabase("Library");
        }

        public async Task AddBookAsync(Book item)
        {
            var collection = db.GetCollection<Book>("books");
            
            var newBook = CreateNewBook(item);
            
            await collection.InsertOneAsync(newBook);
        }
        public async Task<List<Book>> GetAllBooks()
        {
            var filter = new BsonDocument();
            return await db.GetCollection<Book>("books").Find(filter)
                                                        .Sort(Builders<Book>.Sort.Ascending("Name"))
                                                        .ToListAsync();
        }


        public  async Task<List<Book>> GetBooksByCount(int count)
        {
          return await db.GetCollection<Book>("books").Find(book=> book.Count > count)
                                                      .Sort(Builders<Book>.Sort.Ascending("Name"))
                                                      .Limit(3)
                                                      .ToListAsync();
        }

        public async Task<Book> GetBookWithMaxCount()
        {
            var filter = new BsonDocument();

            return await db.GetCollection<Book>("books")
                        .Find(filter)
                        .Sort(Builders<Book>.Sort.Descending("Count"))
                        .FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookWithMinCount()
        {
            var filter = new BsonDocument();

            return await db.GetCollection<Book>("books")
                        .Find(filter)
                        .Sort(Builders<Book>.Sort.Ascending("Count"))
                        .FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetBookAuthors()
        {
            List<string> authors = new List<string>();
            var filter = new BsonDocument();

            var collection = db.GetCollection<Book>("books");
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
            return authors.Distinct().ToList();
        }

        public async Task<List<Book>> GetBookWithoutAuthors()
        {
            return await db.GetCollection<Book>("books")
                        .Find(book => string.IsNullOrEmpty(book.Author))
                        .ToListAsync();
                        
        }
        public async Task IncreaseBookCount(int count)
        {
            var filter = new BsonDocument();
            var updoneresult = await db.GetCollection<Book>("books").UpdateManyAsync(
                               filter,
                               Builders<Book>.Update.Inc("Count", count));
        }
        public async Task AddNewGenre()
        {
            var updoneresult = await db.GetCollection<Book>("books").UpdateManyAsync(
                               book=>(book.Genre== "fantasy"&& book.Genre != "favority"),
                               Builders<Book>.Update.Set("Genre", "favority"));
        }

        public async Task<long> DeleteBookWithCount(int count)
        {
            var deleteResult = await db.GetCollection<Book>("books").DeleteManyAsync(
                             b=>b.Count<count);
            return deleteResult.DeletedCount;
        }

        public async Task DeleteAllBooks()
        {
            var filter = new BsonDocument();
            var deleteResult = await db.GetCollection<Book>("books").DeleteManyAsync(filter);
        }

        private Book  CreateNewBook(Book item)
        {
            var newBook = new Book
            {
               Name=item.Name,
               Author= item.Author,
               Count = item.Count,
               Genre=item.Genre,
               Year=item.Year
            };
            return newBook;
        }



    }
}


