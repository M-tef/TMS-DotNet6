using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMSBlazorAPI.Data;
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


        // PUT: api/Users/5
        //[HttpPut("Update by UserID {id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}




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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> PostUser(UserCreateDto userDto)
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
                logger.LogError(ex, $"error on POST in {nameof(PostUser)}");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }

        // DELETE: api/Users/5
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
