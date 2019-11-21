using Entities;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Casting;
using BL.Helpers;
using System.Web;
using System.Net.Http;

namespace BL
{
    public class Users
    {
        //הרשמה
        public static WebResult<UserDTO> Register(UserDTO user)//האובייקט מגיע עם שדות חובה שכבר נבדקו
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                User goodUser = UserCast.GetUser(user);
                if (db.Users.FirstOrDefault(w => w.passwordUser == goodUser.passwordUser) != null
                    || db.Users.FirstOrDefault(w => w.mailUser == user.mailUser) != null)// אם יש כבר כזו סיסמה או כזה מייל
                    return new WebResult<UserDTO>
                    {
                        Message = "אחד מהנתונים שהוקשו קיימים במערכת",
                        Status = false,
                        Value = null
                    };
                db.Users.Add(goodUser);
                db.SaveChanges();
                return new WebResult<UserDTO>
                {
                    Message = "הנתונים נשמרו בהצלחה",
                    Value = user,
                    Status = true
                };
            }
        }
    }
}
