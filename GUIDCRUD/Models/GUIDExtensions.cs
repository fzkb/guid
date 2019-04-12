using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GUIDCRUD.Models
{

    /// <summary>
    /// 
    /// </summary>
    public static class GUIDExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IQueryable<GUIDEntity> GetGUIDEntities(this GUIDEntityDbContext dbContext)
        {
            var query = dbContext.GuidEntities.AsQueryable();

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<GUIDEntity> GetGUIDEntityByUserAsync(this GUIDEntityDbContext dbContext, GUIDEntity entity)
            => await dbContext.GuidEntities.FirstOrDefaultAsync(guidEntity => guidEntity.User == entity.User);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<GUIDEntity> GetGUIDEntityByGUIDAsync(this GUIDEntityDbContext dbContext, GUIDEntity entity)
            => await dbContext.GuidEntities.FirstOrDefaultAsync(guidEntity => guidEntity.GuidValue == entity.GuidValue);

    }
}
