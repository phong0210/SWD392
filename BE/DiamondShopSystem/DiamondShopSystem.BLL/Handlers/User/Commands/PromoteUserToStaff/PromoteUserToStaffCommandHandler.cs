using MediatR;
using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.PromoteUserToStaff
{
    public class PromoteUserToStaffCommandHandler : IRequestHandler<PromoteUserToStaffCommand, bool>
    {
        private readonly AppDbContext _dbContext;

        public PromoteUserToStaffCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(PromoteUserToStaffCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                                       .Include(u => u.StaffProfile)
                                       .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
            {
                return false; // User not found.
            }

            if (user.StaffProfile != null)
            {
                return false; // User is already a staff member.
            }

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName, cancellationToken);

            if (role == null)
            {
                return false; // Role not found.
            }

            var staffProfile = new StaffProfile
            {
                UserId = user.Id,
                RoleId = role.Id,
                Salary = request.Salary,
                HireDate = request.HireDate
            };

            _dbContext.StaffProfiles.Add(staffProfile);
            user.StaffProfile = staffProfile; // Link staff profile to user

            // Optionally, update user's role directly if your authentication system relies on it
            // This part depends on how your authentication system assigns roles to users.
            // For example, if using ASP.NET Core Identity, you might use UserManager.AddToRoleAsync.
            // For this example, we assume the StaffProfile linkage is sufficient for role recognition.

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
