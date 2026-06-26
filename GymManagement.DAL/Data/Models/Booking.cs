using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Booking : BaseEntity
    {
        public Member member { get; set; }
        public int MemberId { get; set; }

        public Session session { get; set; }
        public int SessionId { get; set; }

        // BookingDate  = CreatedAt Of BaseEnity
        public bool ISAttented { get; set; }
    }
}
