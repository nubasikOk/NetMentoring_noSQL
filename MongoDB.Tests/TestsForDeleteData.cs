using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMentoring_noSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Tests
{
    [TestClass]
    class TestsForDeleteData
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
        public async Task Test_IsCorrect_DeleteAllBooks()
        {
            await client.DeleteAllBooks();
            var actualListBook = await client.GetAllBooks();
            Assert.IsTrue(actualListBook.Count == 0);
        }


        [TestMethod]
        [DataRow(3)]
        public async Task Test_IsCorrect_DeleteBookByCount(int count)
        {
            Assert.IsTrue(await client.DeleteBookWithCount(count)>0);
        }
    }
}
