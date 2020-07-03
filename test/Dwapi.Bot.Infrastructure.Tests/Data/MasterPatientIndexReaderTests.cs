using System.Linq;
using Dwapi.Bot.Core.Domain.Readers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class MasterPatientIndexReaderTests
    {
        private IMasterPatientIndexReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = TestInitializer.ServiceProvider.GetService<IMasterPatientIndexReader>();
        }

        [Test]
        public void should_GetRecordCount()
        {
            var count = _reader.GetRecordCount().Result;
            Assert.True((count > 0));
            Log.Debug($"{count} Records");
        }

        [Test]
        public void should_Get_Site_RecordCount()
        {
            var count = _reader.GetRecordCount(14080).Result;
            Assert.True((count > 0));
            Log.Debug($"{count} Records");
        }

        [Test]
        public void should_Get_Mpi_Sites()
        {
            var sites = _reader.GetMpiSites().Result.ToList();
            Assert.True((sites.Count > 0));
            foreach (var site in sites)
                Log.Debug($"{site}");
        }


        [Test]
        public void should_Read_Paged()
        {
            var top5 = _reader.Read(1, 5).Result.ToList();
            Assert.True((top5.Count == 5));

            var bottom5 = _reader.Read(2, 5).Result.ToList();
            Assert.True((bottom5.Count == 5));
        }

        [Test]
        public void should_Read_Site_Paged()
        {
            var top5 = _reader.Read(1, 5,14080).Result.ToList();
            Assert.True((top5.Count == 5));
            var bottom5 = _reader.Read(2, 5,14080).Result.ToList();
            Assert.True((bottom5.Count == 0));
        }
    }
}
