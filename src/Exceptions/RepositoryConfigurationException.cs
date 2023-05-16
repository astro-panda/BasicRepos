using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BasicRepos.Exceptions
{
    public class RepositoryConfigurationException : ConstraintException
    {
        public RepositoryConfigurationException()
        {
        }

        public RepositoryConfigurationException(string s) : base(s)
        {
        }

        public RepositoryConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RepositoryConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
