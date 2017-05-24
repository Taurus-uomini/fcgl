using fcgl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fcgl.function
{
    public class UserFun
    {
        public bool isUsersHP(UserModels um,int? hpid)
        {
            if(hpid==null)
            {
                return false;
            }
            else
            {
                int id = (int)hpid;
                int idnum = um.houseProperty.Sum(index => index.id);
                int[] ids = new int[idnum];
                ids = um.houseProperty.Select(index => index.id).ToArray();
                return ids.Contains(id);
            }
        }
    }
}