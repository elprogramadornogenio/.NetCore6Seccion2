using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        public DataContext _context;
        public UsersController(DataContext context)
        {
            this._context = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await this._context.Users?.ToListAsync()!;
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser(int id){
            return await this._context.Users?.Where(x=> x.Id == id).ToListAsync()!;   
        }
    }
}