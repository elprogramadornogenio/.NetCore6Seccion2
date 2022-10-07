using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController: BaseApiController
    {
        public DataContext _context;
        public UsersController(DataContext context)
        {
            this._context = context;
            
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await this._context.Users?.ToListAsync()!;
            
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser(int id){
            return await this._context.Users?.Where(x=> x.Id == id).ToListAsync()!;   
        }
    }
}