using System.Net;
using API.Shared.Dtos;
using Microsoft.AspNetCore.Identity;


namespace API.Services
{
    public class UserService(UserManager<ApplicationUser> _userManager, IMapper _mapper, IUnitOfWork _unitOfWork)
    : IUserService
    {
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        private long _MaxAllowedSize = 10485760;
        public async Task<PaginatedResult<UserDetailsDto>> GetAllUserAsync(UserQueryParameters parameters)
        {
            var specs = new GetUserWithAddressSpesification(parameters);
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();

            var users = await userRepo.GetAllAsync(specs);

            var data = _mapper.Map<IEnumerable<UserDetailsDto>>(users);

            var totalCount = await userRepo.CountAsync(new UserCountSpecifications(parameters));
            var pageCount = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return new(parameters.PageIndex, pageCount, totalCount, data);
        }


        public async Task<UserDetailsDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.Users
            .Include(u => u.UserAddress)
            .Include(u => u.RelationshipsInitiated)
            .ThenInclude(r => r.Receiver)
            .Include(u => u.RelationshipsReceived)
            .ThenInclude(r => r.Initiator)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UserNotFoundException(userId);


            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<UserDetailsDto> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto)
        {
            var user = await _userManager.Users
               .Include(u => u.UserAddress)
               .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new UserNotFoundException(userId);

            string profilePicturePath = null;
            string coverPhotoPath = null;

            if (updateDto.ProfilePicture != null)
            {
                profilePicturePath = await SaveImageAsync(updateDto.ProfilePicture, "profile", user.ProfilePictureUrl);
            }

            if (updateDto.CoverPhoto != null)
            {
                coverPhotoPath = await SaveImageAsync(updateDto.CoverPhoto, "cover", user.CoverPhotoUrl);
            }

            user.Email = updateDto.Email.Trim();
            user.UserName = updateDto.UserName.Trim();
            user.DisplayName = updateDto.DisplayName?.Trim() ?? updateDto.UserName.Trim();
            user.PhoneNumber = updateDto.PhoneNumber?.Trim();
            user.ProfilePictureUrl = profilePicturePath!;
            user.CoverPhotoUrl = coverPhotoPath!;
            user.Gender = updateDto.Gender;
            user.Bio = updateDto.Bio!;
            user.DateOfBirth = updateDto.DateOfBirth;
            if (user.UserAddress == null)
            {
                user.UserAddress = new UserAddress
                {
                    City = updateDto.City,
                    Country = updateDto.Country,
                    Street = updateDto.Street
                };
            }

            user.UserAddress.City = updateDto.City;
            user.UserAddress.Country = updateDto.Country;
            user.UserAddress.Street = updateDto.Street;

            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserDetailsDto>(user);
        }

        private async Task<string> SaveImageAsync(IFormFile image, string subFolder, string? oldImagePath = null)
        {
            var extension = Path.GetExtension(image.FileName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !_AllowedExtensions.Contains(extension))
                throw new ServiceException((int)HttpStatusCode.BadRequest,
                    $"Allowed file extensions: {string.Join(", ", _AllowedExtensions)}");

            if (image.Length > _MaxAllowedSize)
                throw new ServiceException((int)HttpStatusCode.BadRequest,
                    $"Max allowed size: {_MaxAllowedSize / (1024 * 1024)}MB");

            // Delete old image if exists
            if (!string.IsNullOrWhiteSpace(oldImagePath))
            {
                var oldImageFullPath = Path.Combine("wwwroot", oldImagePath.TrimStart('/'));
                if (File.Exists(oldImageFullPath))
                    File.Delete(oldImageFullPath);
            }

            var imageFileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine("wwwroot", "images", "users", subFolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var imagePath = Path.Combine(folderPath, imageFileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/users/{subFolder}/{imageFileName}";
        }

    }
}