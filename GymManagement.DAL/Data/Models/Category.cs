using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        #region RelationShip
        public ICollection<Session> sessions { get; set; } = default!;
        #endregion
    }
}
