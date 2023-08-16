// using Application.Common.Interfaces;
// using Infrastructure.Persistence.SeedData;
// using Microsoft.AspNetCore.Identity;
// using System.Reflection;
//
// namespace Infrastructure.Persistence
// {
//     public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
//     {
//         private readonly ApplicationDbContext _applicationDbContext;
//         // private readonly UserManager<User> _userManager;
//         private readonly RoleManager<IdentityRole<Guid>> _roleManager;
//         private readonly ILoadSeedData _seedData;
//         public ApplicationDbContextInitializer(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole<Guid>> roleManager, ILoadSeedData seedData)
//         {
//             _applicationDbContext = applicationDbContext;
//             _roleManager = roleManager;
//             _seedData = seedData;
//         }
//
//         public async Task InitializeAsync()
//         {
//             // if (!await _roleManager.RoleExistsAsync("admin"))
//             // {
//             //     var model = _seedData.Load();
//             //     var properties = model.GetType().GetProperties().OrderBy(x => x.GetCustomAttribute<SeedDataPropertyAttribute>().Order);
//             //     foreach (var property in properties)
//             //     {
//             //         var type = property.GetCustomAttribute<SeedDataPropertyAttribute>().Type;
//             //         IEnumerable<object> values = property.GetValue(model) as IEnumerable<object>;
//             //         if (values != null)
//             //         {
//             //             foreach (var value in values)
//             //             {
//             //                 if (typeof(User).IsAssignableFrom(type))
//             //                 {
//             //                     var user = (User)value;
//             //                     user.Password ??= type.Name + "1234$"; // Password can be: Admin1234$, Teacher1234$ ... etc.
//             //                     // user.UserName = user.Email;
//             //                     await _userManager.CreateAsync(user, user.Password);
//             //                 }
//             //                 else
//             //                 {
//             //                     await _applicationDbContext.AddAsync(value);
//             //                     _applicationDbContext.Entry(value).State = EntityState.Added;
//             //                 }
//             //             }
//             //             await _applicationDbContext.SaveChangesAsync();
//             //         }
//             //     }
//             // }
//         }
//     }
// }
