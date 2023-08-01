using Core.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi;

namespace Application.IntegrationTest
{
    [SetUpFixture]
    public partial class TestHelper
    {
        private static WebApplicationFactory<Startup> _factory = null!;
        private static IServiceScopeFactory _scopeFactory;
        private static Guid _currentUserId;

        [OneTimeSetUp]
        public void RunApplication()
        {
            _factory = new CustomWebApplicationFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : IEntity
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task SaveChangeAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.SaveChangesAsync();
        }

        public static async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : IEntity
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var entity in entities)
            {
                context.Add(entity);
            }

            await context.SaveChangesAsync();
        }

        public static async Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : class, IEntity
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.GetEntities<TEntity>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public static async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.GetEntities<TEntity>().FirstOrDefaultAsync(expression);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public static async Task<Guid> RunAsStudentAsync()
        {
            var student = new Student { FirstName = "FirstName", LastName = "LastName", Occupation = "Student" };
            return await RunAsUserAsync("student@local", "Administrator1234!", new[] { "student" }, student);
        }

        public static async Task<Guid> RunAsTeacherAsync()
        {
            var teacher = new Teacher { FirstName = "FirstName", LastName = "LastName" };
            return await RunAsUserAsync("teacher@local", "Administrator12345!", new[] { "teacher" }, teacher);
        }

        public static async Task<Guid> RunAsUserAsync<T>(string userName, string password, string[] roles, T user) where T : User
        {
            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            user.Email = userName;
            user.UserName = userName;
            var existingUser = userManager.Users.FirstOrDefault(s => s.UserName == userName);
            if (existingUser != null)
            {
                user = (T)existingUser;
            }
            else
            {
                await userManager.CreateAsync(user, password);
            }

            if (roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }

                await userManager.AddToRolesAsync(user, roles);
            }
            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("", ""));
            _currentUserId = user.Id;

            return _currentUserId;
        }

        public static Guid GetAuthenticatedUserId()
        {
            return _currentUserId;
        }

        public static async Task<T> GetAuthenticatedUser<T>() where T : User
        {
            var userId = GetAuthenticatedUserId();
            return await GetAsync<T>(userId);
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static bool IsErrorExists(string propertyName, string errorMessage, ValidationFailureException validationError)
        {
            return validationError.ErrorResponse.Errors.Any(s => s.PropertyName == propertyName && s.ErrorMessage == errorMessage);
        }
    }
}
