using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace artifi.Api.Models
{
    public class AgencyMember
    {
        public Guid AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string RoleInAgency { get; set; } // e.g., "Lead Artist"
    }
}
