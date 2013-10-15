using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface IEntity
    {
        /// <summary>
        /// mongo id
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// save document
        /// </summary>
        void Save();
        /// <summary>
        /// remove document
        /// </summary>
        void Remove();
    }
}
