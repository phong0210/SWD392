using System;
using System.Threading.Tasks;
using System.Linq;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using AutoMapper;

namespace DiamondShopSystem.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserGetResponseDto> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
                var userEntity = await userRepo.GetByIdAsync(userId);
                
                if (userEntity == null)
                {
                    return new UserGetResponseDto
                    {
                        Success = false,
                        Error = "User not found"
                    };
                }

                // Get role information
                string roleName = await GetUserRoleNameAsync(userEntity.Id);

                var userAccountInfo = new UserAccountInfoDto
                {
                    Id = userEntity.Id,
                    FullName = userEntity.Name,
                    Email = userEntity.Email,
                    Phone = userEntity.Phone,
                    RoleName = roleName,
                    IsActive = userEntity.Status,
                    CreatedAt = userEntity.CreatedAt,
                    Address = userEntity.Address
                };

                return new UserGetResponseDto
                {
                    Success = true,
                    User = userAccountInfo
                };
            }
            catch (Exception)
            {
                return new UserGetResponseDto
                {
                    Success = false,
                    Error = "An error occurred while retrieving user account information."
                };
            }
        }



        public async Task<UserUpdateResponseDto> UpdateUserAccountAsync(Guid userId, UserUpdateDto updateDto)
        {
            try
            {
                var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
                var userEntity = await userRepo.GetByIdAsync(userId);
                
                if (userEntity == null)
                {
                    return new UserUpdateResponseDto
                    {
                        Success = false,
                        Error = "User not found"
                    };
                }

                // Check if email is already taken by another user
                var existingUsers = await userRepo.FindAsync(u => u.Email == updateDto.Email && u.Id != userId);
                if (existingUsers.Any())
                {
                    return new UserUpdateResponseDto
                    {
                        Success = false,
                        Error = "Email is already taken by another user"
                    };
                }

                // Update user information
                userEntity.Name = updateDto.Name.Trim();
                userEntity.Email = updateDto.Email;
                userEntity.Phone = updateDto.Phone;
                userEntity.Address = updateDto.Address ?? string.Empty;

                userRepo.Update(userEntity);
                await _unitOfWork.SaveChangesAsync();

                // Get updated user info
                var updatedUser = await userRepo.GetByIdAsync(userId);
                string roleName = await GetUserRoleNameAsync(updatedUser.Id);

                var userAccountInfo = new UserAccountInfoDto
                {
                    Id = updatedUser.Id,
                    FullName = updatedUser.Name,
                    Email = updatedUser.Email,
                    Phone = updatedUser.Phone,
                    RoleName = roleName,
                    IsActive = updatedUser.Status,
                    CreatedAt = updatedUser.CreatedAt,
                    Address = updatedUser.Address
                };

                return new UserUpdateResponseDto
                {
                    Success = true,
                    User = userAccountInfo
                };
            }
            catch (Exception)
            {
                return new UserUpdateResponseDto
                {
                    Success = false,
                    Error = "An error occurred while updating user account information."
                };
            }
        }

        private async Task<string> GetUserRoleNameAsync(Guid userId)
        {
            try
            {
                var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
                var staffProfileRepo = _unitOfWork.Repository<StaffProfile>();
                var roleRepo = _unitOfWork.Repository<Role>();

                // Check if user has a staff profile
                var staffProfiles = await staffProfileRepo.FindAsync(sp => sp.UserId == userId);
                var staffProfile = staffProfiles.FirstOrDefault();

                if (staffProfile != null && staffProfile.RoleId != Guid.Empty)
                {
                    var roles = await roleRepo.FindAsync(r => r.Id == staffProfile.RoleId);
                    var role = roles.FirstOrDefault();
                    if (role != null)
                    {
                        return role.Name;
                    }
                }

                return "Customer";
            }
            catch
            {
                return "Customer";
            }
        }
    }
} 