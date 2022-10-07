using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenServices tokenServices;
        public AccountController(DataContext context, ITokenServices tokenServices)
        {
            this.tokenServices = tokenServices;
            this._context = context;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) {

            if(await UserExists(registerDto.Username!)){
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username!.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)),
                PasswordSalt = hmac.Key
            };
            this._context.Users?.Add(user);
            await this._context.SaveChangesAsync();

            return new UserDto{
                Username = user.UserName,
                Token = this.tokenServices.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto) {
            var user = await this._context.Users!
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null) {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt!);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));
            for(int i = 0; i < computedHash.Length; i++){
                if(computedHash[i]!= user.PasswordHash![i]){
                    return Unauthorized("Invalid Password");
                }
            }
            return new UserDto{
                Username = user.UserName,
                Token = this.tokenServices.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string username){
            return await this._context.Users!.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}