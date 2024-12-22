using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities
{
    public class Profile
    {
        // Key
        public long ProfileId { get; set; }

        // Foreigh Key（not define navigate key）
        public long OrgId { get; set; }  // Organisation Foreign key
        public long RoleId { get; set; } // Role Foreign key
        public long UserId { get; set; } // User Foreign key

        // Foreign key: Supervisor（maybe point to the other  Profile）
        public long? SupervisorId { get; set; }

        // basis attribute
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string ExtraFields { get; set; }
        public bool IsActive { get; set; }

        // navigation attribute
    }
}
