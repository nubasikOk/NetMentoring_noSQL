using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMentoring_noSQL;
using NetMentoring_noSQL.Collections;

namespace MongoDB.Tests
{
    [TestClass]
    public class TestsForAddOrUpdateData
    {
        string connectionString;
        MongoDAL client;
        [TestInitialize]
        public void Test_Init()
        {
            connectionString = "mongodb://localhost:27017";
            client = new MongoDAL(connectionString);
        }

        [TestMethod]
        public async Task Test_IsCorrect_BookAdd()
        {
            await client.AddBooks(GetBookCollection());
            Assert.IsNotNull(await client.GetAllBooks());
        }

        [TestMethod]
        public async Task Test_IsNotNull_GetBookWithMaxMinCount()
        {
            
            Assert.IsNotNull(await client.GetBookWithMaxCount());
            Assert.IsNotNull(await client.GetBookWithMinCount());
        }

        [TestMethod]
        public async Task Test_IsCorrect_GetBookAuthors()
        {
             var listAuthors = await client.GetBookAuthors();
             Assert.IsTrue(listAuthors.Count>0);
        }

        [TestMethod]
        public async Task Test_IsCorrect_GetBookWithoutAuthors()
        {
            var listBooks = await client.GetBookWithoutAuthors();
            Assert.IsTrue(listBooks.Count > 0);
        }


       

        private  Book[] GetBookCollection()
        {
            var books = new  Book []
                {
                    new Book()
                    {
                        Name = "Hobbit",
                        Author = "Tolkien",
                        Count = 5,
                        Genre = "fantasy",
                        Year = 2014
                    },

                    new Book()
                    {
                        Name = "Lord of the rings",
                        Author = "Tolkien",
                        Count = 3,
                        Genre = "fantasy",
                        Year = 2015
                    },

                    new Book()
                    {
                        Name = "Kolobok",
                        Count = 10,
                        Genre = "kids",
                        Year = 2000
                    },
                    new Book()
                    {
                        Name = "Repka",
                        Count = 11,
                        Genre = "kids",
                        Year = 2000
                    },
                     new Book()
                    {
                        Name = "Dyadya Stiopa",
                        Author = "Mihalkov",
                        Count = 1,
                        Genre = "kids",
                        Year = 2001
                    },
                };

            return books;
        }
    }
}
