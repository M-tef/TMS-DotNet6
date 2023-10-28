using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMSBlazorAPI.Data;
using TMSBlazorAPI.Models;
using TMSBlazorAPI.Models.Club;
using TMSBlazorAPI.Models.User;
using TMSBlazorAPI.Static;

namespace TMSBlazorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TMSDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<ClubsController> logger;

        public UsersController(TMSDbContext context, IMapper mapper, ILogger<ClubsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadOnlyDto>>> GetUsers()
        {
            var userDtos = await _context.Users
                    .ProjectTo<UserReadOnlyDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
            //var clubDtos = mapper.Map<IEnumerable<ClubReadOnlyDto>>(User);

            if (_context.Users == null)
            {
                return NotFound();
            }
            return Ok(userDtos);
        }



        // GET: api/Users1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadOnlyDto>> GetUser(int id)
        {
            var userDto = await _context.Users
                    //.Include(q => q.User)
                    .ProjectTo<UserReadOnlyDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            // .ToListAsync();

            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(userDto);
        }


        [Authorize(Roles = "Super Admin,Club Admin,Non Admin")]
        [HttpPut("Update by UserID ")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            mapper.Map(userDto, user);

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }




        // POST: api/Users
        [HttpPost("Register")]
        public async Task<ActionResult<UserCreateDto>> Register (UserCreateDto userDto)
        {
            try
            {
                var user = mapper.Map<User>(userDto);

                if (_context.Users == null)
                {
                    return Problem("Entity set for required entry can not be null.");
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"error on POST in {nameof(Register)}");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserCreateDto>> Login(UserLoginDto userDto)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(item => item.Email == userDto.Username && item.Password == userDto.Password);
            var passwordValid = mapper.Map<User>(userDto);

            if (user == null )
            {
                return NotFound();
            }
            //return Accepted();

            var welcome = "Hello, Welcome to BCS ";
            // return base.Content(html, "text/html");
            return base.Content(welcome , "text/html");
        }


        // DELETE: api/Users/5
        [Authorize(Roles = "Super Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool UserExists(int id)
        //{
        //    return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        //}

        private async Task<bool> UserExists(int id)
        {
            return await (_context.Users?.AnyAsync(e => e.UserId == id));
        }
    }
}
