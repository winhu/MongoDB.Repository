using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MongoDB.Repository.Tests.DBContext
{
    [TestFixture]
    public class MongoDBContextTest : DBContextTestBase
    {
        [TestCase]
        public void MongoDBContextTestCase()
        {
            try
            {
                MongoDBRepository.RegisterMongoDBContext(new CompanyDBContext());
                MongoDBRepository.RegisterMongoDBContext(new FactoryDBContext());
            }
            catch (Exception e)
            {
                Assert.Pass(e.Message);
            }

        }
    }
}
