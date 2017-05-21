using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    public class Group //this is Group table contain GroupId, GroupName, Links to UserGroups and ContentGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<ContentGroup> ContentGroups { get; set; }
    }
}
