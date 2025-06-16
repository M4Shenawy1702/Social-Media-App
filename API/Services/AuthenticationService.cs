using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class AuthenticationService
     : IAuthenticationService
    {
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        private long _MaxAllowedSize = 10485760;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jWTSettings;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jWTSettings, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userManager = userManager;
            _jWTSettings = jWTSettings.Value;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedException();

            var jwtToken = await CreateJwtTokenAsync(user);

            return new UserResponse(user.Id, user.Email!, user.UserName!,user.ProfilePictureUrl!, user.DisplayName, jwtToken);
        }
        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            ValidateRegistrationRequest(request);

            await ValidateUserExistence(request);

            // Handle image uploads
            string? profilePicturePath = null;
            string? coverPhotoPath = null;

            if (request.ProfilePicture != null)
            {
                var extension = Path.GetExtension(request.ProfilePicture.FileName)?.ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(extension) || !_AllowedExtensions.Contains(extension))
                {
                    throw new ServiceException(
                        (int)HttpStatusCode.BadRequest,
                        $"Profile Allowed file extensions are: {string.Join(", ", _AllowedExtensions)}");
                }

                if (request.ProfilePicture.Length > _MaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest,
                        $"Max allowed size: {_MaxAllowedSize / (1024 * 1024)}MB");

                var imageFileName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine("wwwroot", "images", "users", "profile");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var imagePath = Path.Combine(folderPath, imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.ProfilePicture.CopyToAsync(stream);
                }

                profilePicturePath = $"/images/users/profile/{imageFileName}";
            }


            if (request.CoverPhoto != null)
            {
                var extension = Path.GetExtension(request.CoverPhoto.FileName).ToLower();
                if (string.IsNullOrEmpty(extension) || !_AllowedExtensions.Contains(extension))
                    throw new ServiceException((int)HttpStatusCode.BadRequest,
                        $"Cover Allowed extensions: {string.Join(", ", _AllowedExtensions)}");

                if (request.CoverPhoto.Length > _MaxAllowedSize)
                    throw new ServiceException((int)HttpStatusCode.BadRequest,
                        $"Max allowed size: {_MaxAllowedSize / (1024 * 1024)}MB");

                var imageFileName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine("wwwroot", "images", "users", "cover");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var imagePath = Path.Combine(folderPath, imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.CoverPhoto.CopyToAsync(stream);
                }

                coverPhotoPath = $"/images/users/cover/{imageFileName}";
            }

            // Create user
            var user = new ApplicationUser
            {
                Email = request.Email.Trim(),
                UserName = request.UserName.Trim(),
                DisplayName = request.DisplayName?.Trim() ?? request.UserName.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                ProfilePictureUrl = profilePicturePath!,
                CoverPhotoUrl = coverPhotoPath!,
                Gender = request.Gender,
                Bio = request.Bio!,
                DateOfBirth = request.DateOfBirth,
                UserAddress = new UserAddress
                {
                    City = request.City,
                    Country = request.Country,
                    Street = request.Street,
                }
            };

            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                throw new ServiceException((int)HttpStatusCode.BadRequest,
                    $"User creation failed: {string.Join(", ", creationResult.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            var jwtToken = await CreateJwtTokenAsync(user);

            return new UserResponse(user.Id, user.Email!, user.UserName!,user.ProfilePictureUrl!, user.DisplayName, jwtToken);
        }
        public async Task SendEmailVerificationCodeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId);

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var verification = new EmailVerification
            {
                UserId = userId,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10)
            };

            var repo = _unitOfWork.GetRepository<EmailVerification, int>();
            var oldCodes = await repo.FindAllWithcraiteriaAsync(v => v.UserId == userId);
            repo.DeleteRange(oldCodes.ToList());

            repo.Add(verification);
            await _unitOfWork.SaveChangesAsync();

            // Send the email (using IEmailService or SmtpClient)
            var message = $"""
                Hello {user.UserName},
                F
                Your verification code is: {code}

                This code will expire in 10 minutes.

                If you didn’t request this, please ignore this email.
                """;
            await _emailService.SendAsync(user.Email!, "Email Verification Code", message);
        }
        public async Task ConfirmEmailWithCodeAsync(string userId, string code)
        {
            var repo = _unitOfWork.GetRepository<EmailVerification, int>();
            var verification = await repo.FindWithcraiteriaAsync(v => v.UserId == userId && v.Code == code);

            if (verification == null || verification.ExpiryDate < DateTime.UtcNow)
                throw new ServiceException(400, "Invalid or expired code");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId);

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            repo.Delete(verification);
            await _unitOfWork.SaveChangesAsync();
        }

        // Helper methods
        private void ValidateRegistrationRequest(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ServiceException((int)HttpStatusCode.BadRequest, "Email is required");

            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ServiceException((int)HttpStatusCode.BadRequest, "Username is required");
        }

        private async Task ValidateUserExistence(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email.Trim()) is not null)
                throw new ServiceException((int)HttpStatusCode.Conflict, "Email is already registered");

            if (await _userManager.FindByNameAsync(request.UserName.Trim()) is not null)
                throw new ServiceException((int)HttpStatusCode.Conflict, "Username is already taken");

            if (await _userManager.Users.FirstOrDefaultAsync(u => u.DisplayName == request.DisplayName.Trim()) is not null)
                throw new ServiceException((int)HttpStatusCode.Conflict, "DisplayName is already taken");
        }

        private async Task<string> CreateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Key));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jWTSettings.DurationInDays),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}