using System.Collections.Generic;
using System.Reflection;

namespace MockableStatics.Engine.Models
{
    public class MockableType
    {
        /// <summary>
        /// Class name without namespace
        /// </summary>
        public string Name { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<MemberInfo> StaticMethods { get; set; }
    }
}
