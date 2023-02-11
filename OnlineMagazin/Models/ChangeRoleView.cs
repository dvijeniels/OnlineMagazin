using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OnlineMagazin.Models
{
    public class ChangeRoleView
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleView()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
