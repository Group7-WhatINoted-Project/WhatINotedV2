using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    public class ContentGroup //this is contentGroup talbe contain ContentGroupId as primary key, ContentId, GroupId, Content and Group
    {
        public int ContentGroupId { get; set; }

        public int ContentId { get; set; }
        public int GroupId { get; set; }

        public virtual Content Content { get; set; }
        public virtual Group Group { get; set; }
    }
}
