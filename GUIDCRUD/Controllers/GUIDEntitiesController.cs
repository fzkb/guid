using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GUIDCRUD.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace GUIDCRUD.Controllers
{
    /// <summary>
    /// This is a controller class for GUID Entity using 
    /// Entity framework
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GUIDEntitiesController : ControllerBase
    {
        private readonly GUIDEntityDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public GUIDEntitiesController(GUIDEntityDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;

            var cacheKey = "ExpiryTime";

            var existingTime = _distributedCache.GetString(cacheKey);

            existingTime = ((Int32)(DateTime.UtcNow.AddDays(30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            _distributedCache.SetString(cacheKey, existingTime);

        }

        /// <summary>
        /// Get all the Entities from database
        /// </summary>
        /// <returns></returns>
        // GET: api/GUIDEntities
        [HttpGet]
        public IEnumerable<GUIDEntity> GetGuidEntities()
        {
            return _context.GuidEntities;
        }

        /// <summary>
        /// Get Entity by GUID id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //// GET: api/GUIDEntities
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGUIDEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gUIDEntity = await _context.GuidEntities.FindAsync(id);

            if (gUIDEntity == null)
            {
                return NotFound();
            }

            return Ok(gUIDEntity);
        }

        /// <summary>
        /// update the expiry date by guid id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pgUIDEntity"></param>
        /// <returns></returns>

        // PUT: api/GUIDEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGUIDEntity([FromRoute] string id, GUIDEntity pgUIDEntity)
        {
            GUIDEntity gUIDEntity = new GUIDEntity();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pgUIDEntity == null)
            {
                return BadRequest();
            }

            if (id != string.Empty)
            {
                gUIDEntity = await _context.GuidEntities.FindAsync(id);

                if (gUIDEntity != null)
                {
                    gUIDEntity.Expire = pgUIDEntity.Expire;

                    _context.Entry(gUIDEntity).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GUIDEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGUIDEntity", new { id = gUIDEntity.GuidValue }, gUIDEntity);
        }

        /// <summary>
        /// Store the new GUID by user name
        /// </summary>
        /// <param name="gUIDEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostGUIDEntity(GUIDEntity gUIDEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int expiryTime;
            Int32.TryParse(_distributedCache.GetString("ExpiryTime"), out expiryTime);
            gUIDEntity.GuidValue = Guid.NewGuid().ToString("N").ToUpper();
            gUIDEntity.Expire = expiryTime;

            _context.GuidEntities.Add(gUIDEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGUIDEntity", new { id = gUIDEntity.GuidValue }, gUIDEntity);
        }

        // POST: api/GUIDEntities/guid number    
        /// <summary>
        /// save the guid by user name and expiry date 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gUIDEntity"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> PostGUIDEntity([FromRoute] string id, GUIDEntity gUIDEntity)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GUIDEntity entity = new GUIDEntity();

            int expiryTime;
            Int32.TryParse(_distributedCache.GetString("ExpiryTime"), out expiryTime);

            entity.GuidValue = id.ToUpper();
            entity.User = gUIDEntity.User;

            if (expiryTime > gUIDEntity.Expire.Value)
                gUIDEntity.Expire = expiryTime;

            entity.Expire = gUIDEntity.Expire;

            _context.GuidEntities.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGUIDEntity", new { id = entity.GuidValue }, entity);
        }

        /// <summary>
        /// Delete the GUID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/GUIDEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGUIDEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gUIDEntity = await _context.GuidEntities.FindAsync(id);
            if (gUIDEntity == null)
            {
                return Ok("Not found");
            }

            //_context.GuidEntities.Remove(gUIDEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GUIDEntityExists(string id)
        {
            return _context.GuidEntities.Any(e => e.GuidValue == id);
        }

    }
}