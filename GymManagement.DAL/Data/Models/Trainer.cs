using GymManagement.DAL.Data.Models.ENum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        //HireDate = UpdatedAt of BaseEntity
        public Speciality speciality { get; set; }

        #region  RelationShip

        public ICollection<Session> Sessions { get; set; } = default!;


        #endregion
    }
}
