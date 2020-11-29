using System;
using System.Collections.Generic;
using System.Linq;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class DockerReaderTests
    {
        private IDocketReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = TestInitializer.ServiceProvider.GetService<IDocketReader>();
        }

        [Test]
        public void should_Get_Sites()
        {
            var sites = _reader.GetSites().Result.ToList();
            Assert.True((sites.Count > 0));
            foreach (var site in sites)
                Log.Debug($"{site}");
        }
        [Test]
        public void should_Get_Sites_By_Code()
        {
            var siteCodes = new int[] {14063, 14000};
            var sites = _reader.GetSites(siteCodes).Result.ToList();
            Assert.True((sites.Count > 0));
            foreach (var site in sites)
                Log.Debug($"{site}");
        }

        [TestCase]
        public void should_Get_Subjects()
        {
            var facId =new Guid("892F8EEC-6536-43A0-9DA6-AC7F00DC5F2D");
            var subjects = _reader.GetSubjects(facId).Result.ToList();
            Assert.True(subjects.Any());
        }

        [TestCase]
        public void should_Get_SubjectExtracts()
        {
            var facId =new Guid("892F8EEC-6536-43A0-9DA6-AC7F00DC5F2D");
            var subjectIds = _reader.GetSubjects(facId).Result.ToList().Select(x=>x.PatientId).ToList();

            var subjectExtracts = _reader.GetSubjectExtracts(subjectIds).Result.ToList();
            Assert.True(subjectExtracts.Any());
            foreach (var extracts in subjectExtracts.GroupBy(x=>x.Extract))
                Log.Debug($"{extracts.Key} | {extracts.Count()}");
        }

        [TestCase]
        public void should_Clean_SubjectExtracts()
        {
            var patientIds=new List<Guid>()
            {
                new Guid("D4F28E44-2900-4E20-9B2F-AC7F00DC8031"),
                new Guid("4548D789-FDC7-41B5-91D4-AC7F00DC8016")
            };
            var extractIds=new List<Guid>()
            {
                new Guid("4548D789-FDC7-41B5-91D4-AC7F00DC8016")
            };

            _reader.CleanExtract("PatientAdverseEventExtract",  new Guid("4548D789-FDC7-41B5-91D4-AC7F00DC8016"),extractIds, patientIds).Wait();

            var extracts =  _reader.GetSubjectExtracts(patientIds).Result.ToList().Where(x=>x.Extract=="PatientAdverseEventExtract");

            Assert.True(extracts.Any());
        }

        [TestCase]
        public void should_Clean_Subject()
        {
            var sites = new List<Guid>
            {
                new Guid("892F8EEC-6536-43A0-9DA6-AC7F00DC5F2D"),
                    new Guid("893F8EEC-6536-43A0-9DA6-AC7F00DC5F2D")
            };

            var patientIds=new List<Guid>()
            {
                new Guid("4548D789-FDC7-41B5-91D4-AC7F00DC8016")
            };

            _reader.CleanSubject( patientIds).Wait();


            sites.ForEach(x =>
            {
                var extracts = _reader.GetSubjects(sites[0]).Result.ToList().Where(x=>x.PatientId==patientIds[0]);
                Assert.False(extracts.Any(x=>x.PatientId==patientIds[0]));
            });

        }
    }
}
