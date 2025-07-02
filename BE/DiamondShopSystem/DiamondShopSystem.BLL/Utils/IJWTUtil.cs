using DiamondShopSystem.BLL.Domain.Entities;

namespace DiamondShopSystem.BLL.Utils
{
        public interface IJWTUtil
        {
            string GenerateJwtToken(User user, Tuple<string, Guid>? guidClaimer, bool flag);
        }
}
