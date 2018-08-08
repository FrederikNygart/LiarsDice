using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AuthenticationService.DataTransferObjects;
using static AuthenticationService.DatabaseContexts.UsersContext;

namespace AuthenticationService.DataProviders
{
    public class UserProvider
    {
        #region CREATE

        public async static Task InsertPlayerAsync(User user) => await Users.InsertOneAsync(user);
        public static void Insert(User user) => Users.InsertOne(user);

        #endregion

        #region READ

        public static User Get(ObjectId userId) => Users.Find(u => u.Id == userId).Single();
        public static double GetEloRating(ObjectId userId) => Get(userId).EloRating;

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
