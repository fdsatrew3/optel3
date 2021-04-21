using System;
using System.Collections.Generic;

using OPTEL.Entity.Helpers.Validation;

namespace OPTEL.Entity.Helpers.Exceptions
{
    public class DbEntityValidationException : Exception
    {
        public IEnumerable<DbEntityValidationResult> EntityValidationResults { get; }

        public DbEntityValidationException(IEnumerable<DbEntityValidationResult> entityValidationResults) 
            : base()
        {
            EntityValidationResults = entityValidationResults;
        }

        public DbEntityValidationException(string message, IEnumerable<DbEntityValidationResult> entityValidationResults, Exception innerException) 
            : base(message, innerException)
        {
            EntityValidationResults = entityValidationResults;
        }
    }
}
