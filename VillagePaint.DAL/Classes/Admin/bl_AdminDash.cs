using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillagePaint.DAL.Classes.Admin
{
    public class bl_AdminDash
    {
        public int countClients { get; set; }
        public int countAdmins { get; set; }
        public static bl_AdminDash AdminDashList()
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qU = from row in metadata.db_User                         
                         select new
                         {
                             userID = row.userID,
                         };
                var qUsers = qU.Count();

                var qRs = from row in metadata.db_Customer
                          
                          select new
                          {
                              userID = row.customerID,
                          };
                var qClients = qRs.Count();

                var q = new bl_AdminDash
                {
                    countClients = qUsers,
                    countAdmins = qClients,
                };

                return q;

            }
        }
    }
}
