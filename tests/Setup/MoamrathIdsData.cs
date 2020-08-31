using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroPanda.Data.Test.Setup
{
    public class MoamrathIdsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                Guid.Parse("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267"),
                Guid.Parse("323A3F72-3DAB-422B-A306-8E155CE1F61A"),
                Guid.Parse("8299D594-0AA4-4D33-8EB2-4557A2221AF8"),
                Guid.Parse("155C1269-5D13-4E83-8F0B-34B8B2B5FAE1"),
                Guid.Parse("ABABF0EA-4128-4830-8471-B634585145D9")
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();        
    }

    public class FakeMoamrathIdsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PartialFakeMoamrathIdsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                Guid.Parse("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267"),
                Guid.Parse("323A3F72-3DAB-422B-A306-8E155CE1F61A"),
                Guid.Parse("8299D594-0AA4-4D33-8EB2-4557A2221AF8"),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
