using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.Entities;
using Registration.Resources;

namespace Registration.Services
{
    public sealed class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly string _pepper;
        private readonly int _iteration = 3;

        public UserService(DataContext context)
        {
            _context = context;
            _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
        }

        public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken)
        {
            // Check if the email already exists in the database
            if (await IsEmailAlreadyExists(resource.Email, cancellationToken))
            {
                throw new Exception("Email address already exists.");
            }

            var user = new User
            {
                FirstName = resource.FirstName,
                LastName = resource.LastName,
                Email = resource.Email,
                Address = resource.Address,
                role=resource.role,
                PasswordSalt = PasswordHasher.GenerateSalt()
            };
            user.PasswordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserResource(user.Id, user.FirstName, user.Email, user.LastName, user.Address,user.role);
        }

        public async Task<UserResource> Login(LoginResource resource, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == resource.Email, cancellationToken);

            if (user == null)
                throw new Exception("Username or password did not match.");

            var passwordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Username or password did not match.");

            return new UserResource(user.Id, user.FirstName, user.LastName, user.Email, user.Address,user.role);
        }

        private async Task<bool> IsEmailAlreadyExists(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }
    }
}
