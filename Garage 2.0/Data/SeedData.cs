using Garage_2._0.Data;
using Garage_2._0.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Garage_2._0
{
    public class SeedData
    {
        private static Garage_2_0Context context = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<User> userManager = default!;


        public static async Task InitAsync(Garage_2_0Context _context, IServiceProvider services)
        {
            context = _context;

            await AddVehicleTypesAsync();
            if (context.Roles.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<User>>();

            var roleNames = new[] { "User", "Admin" };
            var adminEmail = "admin@gymbokning.se";

            await AddRolesAsync(roleNames);


            var admin = await AddAccountAsync(adminEmail, "Admin", "Adminsson", "Abc123!");

            await AddUserToRoleAsync(admin, "Admin");

        }

        private static async Task AddVehicleTypesAsync()
        {
            if (context.VehicleTypes.Any()) return;
            
            var elScooterType = new VehicleType() { Name = "ElectricScooter", SpotSize = 0.1m};
            var bicyleType = new VehicleType() { Name = "Bicyle", SpotSize = 0.2m};
            var mcType = new VehicleType() { Name = "MotorCycle", SpotSize = 0.3m};
            var carType = new VehicleType() { Name = "Car", SpotSize = 1.0m};
            
            context.VehicleTypes.Add(elScooterType);
            context.VehicleTypes.Add(bicyleType);
            context.VehicleTypes.Add(mcType);
            context.VehicleTypes.Add(carType);
            context.SaveChanges();
        }

        private static async Task AddUserToRoleAsync(User user, string role)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<User> AddAccountAsync(string accountEmail, string fName, string lName, string password)
        {
            var found = await userManager.FindByEmailAsync(accountEmail);

            if (found != null) return null!;

            var user = new User
            {
                UserName = accountEmail,
                Email = accountEmail,
                EmailConfirmed = true,
                FirstName = fName,
                LastName = lName,
                PersonalNumber = 1234
            }; 

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return user;

        }
    }
}
