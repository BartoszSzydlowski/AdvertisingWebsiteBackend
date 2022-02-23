using API.Helpers.EmailHelpers;
using API.Models;
using API.Models.User;
using API.Wrappers;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers.V1
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IConfiguration _configuration;

        public IdentityController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
                {
                    Succeeded = false,
                    Message = "User already exists!",
                    Errors = new List<string>() { "User already exists!" }
                });
            }
            if (register.Password == register.ConfirmPassword)
            {
                var user = new ApplicationUser()
                {
                    Email = register.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = register.Username,
                    PhoneNumber = register.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, register.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
                    {
                        Succeeded = false,
                        Message = "User creation failed! Please check user details and try agian.",
                        Errors = result.Errors.Select(x => x.Description)
                    });
                }

                if (!await _roleManger.RoleExistsAsync(UserRoles.User))
                {
                    await _roleManger.CreateAsync(new IdentityRole(UserRoles.User));
                }

                await _userManager.AddToRoleAsync(user, UserRoles.User);

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedConfirmationToken = Encoding.UTF8.GetBytes(confirmationToken);
                var validConfirmationToken = WebEncoders.Base64UrlEncode(encodedConfirmationToken);
                var confirmationLink = $"http://localhost:3000/confirmEmail?userEmail={user.Email}&token={validConfirmationToken}";

                var sender = new EmailSender();
                await sender.SendEmailAsync(user.Email, "Advertising website - welcome",
                    $"Please confirm your account by clicking this link:<br>{confirmationLink}" +
                    $"<br>Token: {validConfirmationToken}");

                return Ok(new Response { Succeeded = true, Message = "You created account. Check your email given in registration to confirm account." });
            }
            return BadRequest(new Response<bool> { Succeeded = false, Message = "Passwords don't match", Errors = new List<string>() { "Passwords don't match" } });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Succeeded = false,
                    Message = "User already exists!"
                });
            }

            var user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.Username
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
                {
                    Succeeded = false,
                    Message = "Admin creation failed! Please check details and try agian.",
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            if (!await _roleManger.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManger.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new Response { Succeeded = true, Message = "User created successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMod(RegisterModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Succeeded = false,
                    Message = "User already exists!"
                });
            }

            var user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.Username
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
                {
                    Succeeded = false,
                    Message = "Moderator creation failed! Please check details and try agian.",
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            if (!await _roleManger.RoleExistsAsync(UserRoles.Moderator))
            {
                await _roleManger.CreateAsync(new IdentityRole(UserRoles.Moderator));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Moderator);

            return Ok(new Response { Succeeded = true, Message = "User created successfully!. Please confirm your account on email" });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new TokenResponse()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email, string token, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var encodedToken = WebEncoders.Base64UrlDecode(token);
                var validToken = Encoding.UTF8.GetString(encodedToken);

                var result = await _userManager.ConfirmEmailAsync(user, validToken);

                if (result.Succeeded)
                {
                    return Ok(new Response { Succeeded = true, Message = "Successfully confirmed account" });
                }
            }

            return BadRequest(new Response { Succeeded = false, Message = "Failed to confirm account" });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var resetPasswordLink = $"http://localhost:3000/recoverPassword?userEmail={user.Email}&token={validToken}";
                var sender = new EmailSender();
                await sender.SendEmailAsync(user.Email, "Advertising website - reset your password",
                    $"Reset your password here:<br>{resetPasswordLink}<br>");
            }

            return Ok(new Response { Succeeded = true, Message = "Check your email - we sent link there to reset password" });
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordModel model, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var encodedToken = WebEncoders.Base64UrlDecode(model.Token);
                var validToken = Encoding.UTF8.GetString(encodedToken);
                if (model.Password == "" || model.Password == null || model.ConfirmPassword == "" || model.ConfirmPassword == null)
                {
                    return BadRequest(new Response { Succeeded = false, Message = "Password can't be empty" });
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest(new Response { Succeeded = false, Message = "Passwords don't match" });
                }

                var result = await _userManager.ResetPasswordAsync(user, validToken, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new Response { Succeeded = true, Message = "Password successfully updated" });
                }
            }
            return BadRequest(new Response { Succeeded = false, Message = "Failed to update password" });
        }

        //[HttpGet("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetUserNameAndRole()
        {
            return Ok(new
            {
                Username = User.FindFirstValue(ClaimTypes.Name),
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserData(string userId, string userName)
        {
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(userName))
            {
                return Ok(new
                {
                    Username = User.FindFirstValue(ClaimTypes.Name),
                    Email = User.FindFirstValue(ClaimTypes.Email),
                    PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone) ?? ""
                });
            }
            if (string.IsNullOrEmpty(userName))
            {
                var user = await _userManager.FindByIdAsync(userId);
                return Ok(new
                {
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? ""
                });
            }
            if (string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByNameAsync(userName);
                return Ok(new
                {
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? ""
                });
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword(UpdatePassword model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, model.Password);
                await _userManager.UpdateAsync(user);
                return NoContent();
            }
            return BadRequest(new Response<bool>
            {
                Message = "Passwords don't match"
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmail(UpdateEmail model)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePhoneNumber(UpdatePhoneNumber model)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            user.PhoneNumber = model.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
