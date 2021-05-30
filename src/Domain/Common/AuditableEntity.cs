namespace Domain.Common
{
	using System;

	public abstract class AuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedTime { get; set; }
    }
}
