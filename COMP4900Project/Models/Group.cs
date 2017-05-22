using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    // this is Group table contain GroupId, GroupName, Links to UserGroups and ContentGroups
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<ContentGroup> ContentGroups { get; set; }
    }
}