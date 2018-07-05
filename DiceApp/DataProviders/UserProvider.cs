﻿using UserService.DataTransferObjects;
using static UserService.DatabaseContexts.UsersContext;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UserService.DataProviders
{
    class UserProvider
    {
        #region CREATE

        public async static Task InsertPlayerAsync(User user) => await Users.InsertOneAsync(user);

        #endregion

        #region READ

        public static User Get(ObjectId id) => Users.Find(u => u.Id == id).Single();

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
