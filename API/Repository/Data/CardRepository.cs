using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class CardRepository : GeneralRepository<MyContext, Card, string>

    {
        public CardRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
