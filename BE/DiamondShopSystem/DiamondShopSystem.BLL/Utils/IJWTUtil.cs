using DiamondShopSystem.DAL.Data.Entities;

namespace DiamondShopSystem.BLL.Utils
{
        public interface IJWTUtil
        {
            string GenerateJwtToken(User user, Tuple<string, Guid> tuple, bool flag);
        }
}
