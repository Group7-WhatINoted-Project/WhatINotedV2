using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    // this is contentGroup table contain ContentGroupId as primary key, 
    // ContentId, GroupId, Content and Group
    public class ContentGroup
    {
        public int ContentGroupId { get; set; }

        public int ContentId { get; set; }
        public int GroupId { get; set; }

        public virtual Content Content { get; set; }
        public virtual Group Group { get; set; }
    }
}