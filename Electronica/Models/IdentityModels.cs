using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Electronica.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string LastName { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Cart_Item> cart_Items { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        
        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order_Tracking> GetOrder_Trackings { get; set; }
        public DbSet<Order_Address> Order_Addresses { get; set; }
        public  DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    
        public System.Data.Entity.DbSet<Electronica.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Electronica.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Electronica.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<Electronica.Models.ShopVM> ShopVMs { get; set; }

        public System.Data.Entity.DbSet<Electronica.Models.StockOrder> StockOrders { get; set; }

        public System.Data.Entity.DbSet<Electronica.Models.Customer> Customers { get; set; }

        //public System.Data.Entity.DbSet<Electronica.Models.ExpandedUserDTO> ExpandedUserDTOes { get; set; }

        //public System.Data.Entity.DbSet<Electronica.Models.UserRolesDTO> UserRolesDTOes { get; set; }

        //public System.Data.Entity.DbSet<Electronica.Models.UserAndRolesDTO> UserAndRolesDTOes { get; set; }

        //public System.Data.Entity.DbSet<Electronica.Models.RoleDTO> RoleDTOes { get; set; }
    }
}