using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserService.DataTransferObjects;
using static UserService.DatabaseContexts.UsersContext;

namespace UserService.DataProviders
{
    class UserProvider
    {
        #region CREATE

        public async static Task InsertPlayerAsync(User user) => await Users.InsertOneAsync(user);

        #endregion

        #region READ

        public static User Get(ObjectId userId) => Users.Find(u => u.Id == userId).Single();
        public static long GetEloRating(ObjectId userId) => Get(userId).EloRating;

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
