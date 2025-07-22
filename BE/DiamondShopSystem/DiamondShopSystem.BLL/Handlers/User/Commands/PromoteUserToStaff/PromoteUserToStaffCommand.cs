using MediatR;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.PromoteUserToStaff
{
    public record PromoteUserToStaffCommand(string Email, string RoleName, double Salary, DateTime HireDate) : IRequest<bool>;
}
