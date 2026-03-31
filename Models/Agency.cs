using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Artify.Api.Models
{
public class Agency
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
    // The user who created the agency
    public Guid OwnerId { get; set; }
    public virtual ApplicationUser Owner { get; set; }

    public virtual ICollection<AgencyMember> Members { get; set; }
}
}