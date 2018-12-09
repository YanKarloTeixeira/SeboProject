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
            if (user.Count() > 0)
                return user[0];
            else
                return -1;

        }
        public static int GetUserBranchId(string UserName, SeboDbContext _context)
        {

            var user = (from s in _context.User where s.UserName == UserName select s.InstitutionBranchId).ToList();
            if (user.Count() > 0)
                return user[0];
            else
                return -1;

        }
    }
}
