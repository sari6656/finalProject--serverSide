using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Casting
{
    public class UserCast
    {
        public static ProjectEntities db = new ProjectEntities();
        public static UserDTO GetUserDTO(User user)
        {
            
            return new UserDTO()
            {
                codeUser = user.codeUser,
                nameUser = user.nameUser,
                phoneUser = user.phoneUser,
                mailUser = user.mailUser,
                passwordUser=user.passwordUser
            };
        }
        public static User GetUser(UserDTO user)
        {
            return new User()
            {
                codeUser = user.codeUser,
                nameUser = user.nameUser,
                phoneUser = user.phoneUser,
                mailUser = user.mailUser,
                passwordUser=user.passwordUser
            };
        }
        //המרת רשימה שלמה
        public static List<UserDTO> GetUserDTOs(List<User> users)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var item in users)
            {
                userDTOs.Add(new UserDTO()
                {
                    codeUser = item.codeUser,
                    nameUser = item.nameUser,
                    phoneUser = item.phoneUser,
                    mailUser = item.mailUser,
                    passwordUser=item.passwordUser
                });
            }
            return userDTOs;
        }
    }
}
