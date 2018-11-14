using SeboProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Helpers
{
    public class HelperUser
    {
        public static int GetUserId(string UserName, SeboDbContext _context)
        {
            
            var user = (from s in _context.User where s.UserName == UserName select s.UserId).ToList();
            int UserId = user[0];
            if (UserId > 0)
                return UserId;
            else
                return -1;

        }
    }
}
