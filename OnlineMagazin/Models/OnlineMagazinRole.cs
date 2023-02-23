using Microsoft.AspNetCore.Identity;
using OnlineMagazin.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.Models
{
    public class OnlineMagazinRole
    {
        [Key]
        public int RoleId { get; set; }

        [StringLength(12)]
        [DisplayName("Роль")]
        public string RoleName { get; set; }
        public enum Roles
        {
            User,
            Admin
        }
        public static async Task SeedRolesAsync(UserManager<OnlineMagazinUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        }
        public static async Task CreatedAdminUserRole(UserManager<OnlineMagazinUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new OnlineMagazinUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FirstAndLastName = "AdminName",
                Foto = "adminFoto.webp",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }

            }
        }
    }
}
