using MongoDB.Bson;
using MongoDB.Driver;
using NetMentoring_noSQL;
using NetMentoring_noSQL.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   
    class Program
    {


        public static async Task test()
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoDAL(connectionString);
           
            client.DeleteAllBooks().Wait();
           
        }
        static void Main(string[] args)
        {

            

            test().Wait();
            Console.ReadLine();
          

           
        }

       


       
    }
}

