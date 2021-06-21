using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OPTEL.Entity.Helpers.Validation
{
    public class DbEntityValidationResult
    {
        public EntityEntry Entry { get; }

        public ICollection<ValidationResult> ValidationResults { get; }

        public DbEntityValidationResult(EntityEntry entry, ICollection<ValidationResult> validationResults)
        {
            Entry = entry;
            ValidationResults = validationResults;
        }
    }
}
