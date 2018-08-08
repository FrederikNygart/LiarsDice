using Authentication.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Resources.Strings;

namespace Authentication.DatabaseContexts
{
    public static class UsersContext
    {
        public static IMongoDatabase db
        {
            get
            {
                MongoClient client = new MongoClient(Connection);
                return client.GetDatabase(UsersDataBaseName);
            }
        }

        public static IMongoCollection<User> Users => db.GetCollection<User>(UsersContextUsers);
          
    }
}
