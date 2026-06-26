using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        #region RelationShip
        public HealthRecord healthRecord { get; set; } = default!;

        public ICollection<MemberShip> memberShips { get; set; } = default!;

        public ICollection<Booking> bookings { get; set; } = default!;
        #endregion
        //JoinDate = CreatedAt Of BaseEntity
    }
}
