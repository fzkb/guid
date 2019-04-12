using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GUIDCRUD.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PostGUIDRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string GuidEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string User { get; set; }       

        /// <summary>
        /// 
        /// </summary>
        public int? Expire { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PutGUIDRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int? Expire { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GUIDEntity ToEntity(this PostGUIDRequest request)
            => new GUIDEntity
            {
                GuidValue = request.GuidEntity,
                User = request.User,
                Expire = request.Expire
            };
    }
}
