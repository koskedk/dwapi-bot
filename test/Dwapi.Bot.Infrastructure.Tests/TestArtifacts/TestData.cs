using System;
using System.Collections.Generic;
using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using FizzWare.NBuilder;
using Newtonsoft.Json;

namespace Dwapi.Bot.Infrastructure.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<SubjectIndex> GenerateSubjects(bool simulate=false,int count = 5)
        {
          if (simulate)
            return JsonConvert.DeserializeObject<List<SubjectIndex>>(SubjectsJson());

          return Builder<SubjectIndex>.CreateListOfSize(count)
                .Build()
                .ToList();
        }

        public static SubjectIndex GenerateSubject()
        {
          // 2 F 1996 13165

          var subject = Builder<SubjectIndex>.CreateNew()
            .With(x => x.Gender = "Female")
            .With(x => x.DOB = new DateTime(1996, 1, 1))
            .With(x => x.SiteCode = 13165)
            .With(x => x.FacilityName = "SIAYA COUNTY REFERRAL HOSPITAL")
            .Build();

          return subject;
        }

        public static List<SubjectIndexScore> GenerateSubjectScores(int count = 2)
        {
          var subject = JsonConvert.DeserializeObject<List<SubjectIndex>>(SubjectsJson()).First();

          return Builder<SubjectIndexScore>.CreateListOfSize(count)
            .All()
            .With(x=>x.PatientIndexId=subject.Id)
            .Build()
            .ToList();
        }

        public static List<SubjectIndexStage> GenerateSubjectStages(int count = 2)
        {
          var subject = JsonConvert.DeserializeObject<List<SubjectIndex>>(SubjectsJson()).First();

          return Builder<SubjectIndexStage>.CreateListOfSize(count)
            .All()
            .With(x=>x.PatientIndexId=subject.Id)
            .Build()
            .ToList();
        }

        private static string SubjectsJson()
        {
            var json = @"
[
  {
    ^Id^: ^816139E2-7EED-46B8-A1DF-AB9A00B31452^,
    ^MpiId^: ^D6E8E11C-76A5-41F5-95DB-AB7500D09A5E^,
    ^PatientPk^: 3063,
    ^SiteCode^: 14080,
    ^FacilityName^: ^SIAYA COUNTY REFERRAL HOSPITAL^,
    ^Serial^: ^20087824^,
    ^Gender^: ^Male^,
    ^DOB^: ^1985-12-12 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^RMNT^,
    ^dmMiddleName^: null,
    ^dmLastName^: null,
    ^sxFirstName^: ^R553^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: null,
    ^sxPKValue^: ^MR55300001985^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^MR553000019851212^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^EAFD6D83-13F2-4EC6-BE47-AB7500D1B1E9^,
    ^RowId^: 16907
  },
  {
    ^Id^: ^1090D8A8-7BA8-4910-803C-AB9A00B31453^,
    ^MpiId^: ^7284C654-431A-446E-ACCC-AB7500D09BB3^,
    ^PatientPk^: 38844,
    ^SiteCode^: 14080,
    ^FacilityName^: ^SIAYA COUNTY REFERRAL HOSPITAL^,
    ^Serial^: ^A0051902111020897^,
    ^Gender^: ^Female^,
    ^DOB^: ^1986-02-11 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: null,
    ^dmMiddleName^: null,
    ^dmLastName^: null,
    ^sxFirstName^: ^0000^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^0000^,
    ^sxPKValue^: ^F000000001986^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^F0000000019860211^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^EAFD6D83-13F2-4EC6-BE47-AB7500D1B1E9^,
    ^RowId^: 30636
  },
  {
    ^Id^: ^15B85A5D-B736-4704-8559-AB9A00B31453^,
    ^MpiId^: ^2EAD2682-2D1E-46AD-A9A0-AB7500D09BF5^,
    ^PatientPk^: 46216,
    ^SiteCode^: 14080,
    ^FacilityName^: ^SIAYA COUNTY REFERRAL HOSPITAL^,
    ^Serial^: ^A0081904292124226^,
    ^Gender^: ^Female^,
    ^DOB^: ^1977-06-15 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^JNFR;ANFR^,
    ^dmMiddleName^: ^ASNK;FSNK^,
    ^dmLastName^: null,
    ^sxFirstName^: ^J516^,
    ^sxLastName^: ^W000^,
    ^sxMiddleName^: ^W252^,
    ^sxPKValue^: ^FJ516W0001977^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FJ516W00019770615^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^EAFD6D83-13F2-4EC6-BE47-AB7500D1B1E9^,
    ^RowId^: 33068
  },
  {
    ^Id^: ^7D7C8EBF-1F01-4A98-ACD2-AB9A00B31453^,
    ^MpiId^: ^679A12D8-D474-4C6D-9CD7-AB7500D09C1D^,
    ^PatientPk^: 58383,
    ^SiteCode^: 14080,
    ^FacilityName^: ^SIAYA COUNTY REFERRAL HOSPITAL^,
    ^Serial^: ^A0061906141234546^,
    ^Gender^: ^Male^,
    ^DOB^: ^1992-06-15 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^PRN^,
    ^dmMiddleName^: ^ANTR;ANTRF^,
    ^dmLastName^: null,
    ^sxFirstName^: ^B650^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^A536^,
    ^sxPKValue^: ^MB65000001992^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^MB650000019920615^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^EAFD6D83-13F2-4EC6-BE47-AB7500D1B1E9^,
    ^RowId^: 34388
  },
  {
    ^Id^: ^D5A3FC7D-F5B4-4BD7-BD0A-AB9A00B31453^,
    ^MpiId^: ^7901276C-6C85-40A9-B454-AB7500D09C1D^,
    ^PatientPk^: 58384,
    ^SiteCode^: 14080,
    ^FacilityName^: ^SIAYA COUNTY REFERRAL HOSPITAL^,
    ^Serial^: ^A0061906141236641^,
    ^Gender^: ^Female^,
    ^DOB^: ^1999-06-15 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^LLN^,
    ^dmMiddleName^: ^AKNK^,
    ^dmLastName^: null,
    ^sxFirstName^: ^L450^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^A252^,
    ^sxPKValue^: ^FL45000001999^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FL450000019990615^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^EAFD6D83-13F2-4EC6-BE47-AB7500D1B1E9^,
    ^RowId^: 34471
  },
  {
    ^Id^: ^175A93C1-B655-4991-8333-AB9A00B314BB^,
    ^MpiId^: ^164082EB-3C99-4FAC-8B3B-AB7B00E7845E^,
    ^PatientPk^: 44244,
    ^SiteCode^: 13165,
    ^FacilityName^: ^RIRUTA HC^,
    ^Serial^: ^A0131903071109267^,
    ^Gender^: ^Female^,
    ^DOB^: ^1989-12-22 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^SLFN^,
    ^dmMiddleName^: ^KSNT^,
    ^dmLastName^: null,
    ^sxFirstName^: ^S415^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^K253^,
    ^sxPKValue^: ^FS41500001989^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FS415000019891222^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^BAD20E17-0FC1-4F8F-927B-AB7B00E7B272^,
    ^RowId^: 72191
  },
  {
    ^Id^: ^C8B5A8C0-D18E-4D88-BBD6-AB9A00B314BB^,
    ^MpiId^: ^4E38D989-B375-4ABC-9306-AB7B00E7845E^,
    ^PatientPk^: 44215,
    ^SiteCode^: 13165,
    ^FacilityName^: ^RIRUTA HC^,
    ^Serial^: ^A0131903061249236^,
    ^Gender^: ^Male^,
    ^DOB^: ^1997-04-27 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^PRN^,
    ^dmMiddleName^: ^MRR^,
    ^dmLastName^: null,
    ^sxFirstName^: ^B650^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^M660^,
    ^sxPKValue^: ^MB65000001997^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^MB650000019970427^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^BAD20E17-0FC1-4F8F-927B-AB7B00E7B272^,
    ^RowId^: 72198
  },
  {
    ^Id^: ^920EDDDF-0546-48CF-B070-AB9A00B314BB^,
    ^MpiId^: ^ABB9E21D-C6B7-4B2D-B139-AB7B00E78460^,
    ^PatientPk^: 46285,
    ^SiteCode^: 13165,
    ^FacilityName^: ^RIRUTA HC^,
    ^Serial^: ^A0181903081554695^,
    ^Gender^: ^Female^,
    ^DOB^: ^1996-07-16 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^LLN^,
    ^dmMiddleName^: ^NPR^,
    ^dmLastName^: null,
    ^sxFirstName^: ^L450^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^N160^,
    ^sxPKValue^: ^FL45000001996^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FL450000019960716^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^BAD20E17-0FC1-4F8F-927B-AB7B00E7B272^,
    ^RowId^: 72327
  },
  {
    ^Id^: ^5EBD9AB7-8000-45F1-961A-AB9A00B314BB^,
    ^MpiId^: ^4C5C8B64-A38B-42ED-A066-AB7B00E78480^,
    ^PatientPk^: 46633,
    ^SiteCode^: 13165,
    ^FacilityName^: ^RIRUTA HC^,
    ^Serial^: ^A0131903110950241^,
    ^Gender^: ^Female^,
    ^DOB^: ^1996-09-05 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^ANS^,
    ^dmMiddleName^: ^AN^,
    ^dmLastName^: null,
    ^sxFirstName^: ^E520^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^A500^,
    ^sxPKValue^: ^FE52000002000^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FE520000020000905^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^BAD20E17-0FC1-4F8F-927B-AB7B00E7B272^,
    ^RowId^: 72583
  },
  {
    ^Id^: ^DEE6E758-B78E-441E-BF5E-AB9A00B314BB^,
    ^MpiId^: ^11ACAB6A-6AEB-491D-8F80-AB7B00E78481^,
    ^PatientPk^: 46657,
    ^SiteCode^: 13165,
    ^FacilityName^: ^RIRUTA HC^,
    ^Serial^: ^A0131903111119081^,
    ^Gender^: ^Female^,
    ^DOB^: ^1993-10-04 03:00:00^,
    ^PatientID^: null,
    ^dmFirstName^: ^HLN^,
    ^dmMiddleName^: ^MKNK^,
    ^dmLastName^: null,
    ^sxFirstName^: ^H450^,
    ^sxLastName^: ^0000^,
    ^sxMiddleName^: ^M252^,
    ^sxPKValue^: ^FH45000001993^,
    ^dmPKValue^: null,
    ^sxdmPKValue^: null,
    ^sxPKValueDoB^: ^FH450000019931004^,
    ^dmPKValueDoB^: null,
    ^sxdmPKValueDoB^: null,
    ^FacilityId^: ^BAD20E17-0FC1-4F8F-927B-AB7B00E7B272^,
    ^RowId^: 72675
  }
]

";

            return json.Replace("^", @"'");
        }
    }
}
