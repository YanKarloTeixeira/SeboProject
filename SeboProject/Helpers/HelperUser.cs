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

        public static bool isAdministrator(string user)
        {
            if(user=="yankarlo@hotmail.com"||
                user=="admin@admin.com"||
                user=="anurlai @my.centennialcollege.ca"||
                user=="kkaur279@my.centennialcollege.ca"||
                user=="npate476 @my.centennialcollege.ca"||
                user=="rdhali36 @my.centennialcollege.ca"||
                user=="rpunia2 @my.centennialcollege.ca"||
                user=="sdas47 @my.centennialcollege.ca")

                    return true;
            else
                    return false;
            
        }
    }
}
