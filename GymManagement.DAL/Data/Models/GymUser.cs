using GymManagement.DAL.Data.Models.ENum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public abstract class GymUser :BaseEntity
    {
        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; } 
        public Address Address { get; set; }
        public Gender gender { get; set; }
    }

    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int BuildingNumber { get; set; }
    }
}
