using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace COMP4900Project.Models
{
    public class Friend //this is the Friend table contains FriendId as primary key, User1Id, User2Id as the foreignKey, link to User1 and User2
    {
        public int FriendId { get; set; }

        public string User1Id { get; set; }
        public string User2Id { get; set; }

        [ForeignKey("User1Id")]
        public virtual ApplicationUser User1 { get; set; }
        [ForeignKey("User2Id")]
        public virtual ApplicationUser User2 { get; set; }
    }
}
