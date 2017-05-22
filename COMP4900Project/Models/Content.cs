using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    //this is content table contain ContentId as primary key, Text, Note, Reference, 
    //TimeUpdated, link to UserContents and ContentGroups
    public class Content
    {
        public int ContentId { get; set; }

        public string Text { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public string Reference { get; set; }
        public DateTime TimeUpdated { get; set; }

        public virtual ICollection<UserContent> UserContents { get; set; }
        public virtual ICollection<ContentGroup> ContentGroups { get; set; }
    }
}