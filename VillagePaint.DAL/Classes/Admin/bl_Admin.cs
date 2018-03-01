using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePaint.DAL.Utils;

namespace VillagePaint.DAL.Classes.Admin
{
    public class bl_Admin_Result
    {
        public long userID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_Admin
    {
        public long userID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Cell { get; set; }
        public bool canDelete { get; set; }
        public static List<bl_Admin> AdminList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_User
                        select new bl_Admin
                        {
                            userID = row.userID,
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            Email = row.Email,
                            Cell = row.Cell,
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.FirstName.Contains(search) || r.LastName.Contains(search) || r.Email.Contains(search) || r.Cell.Contains(search));
                }
                string OrderByCol = paging.sort_col;
                bool OrderDirectionAscending = paging.sort_isAsc;

                //Sorting

                if (!String.IsNullOrWhiteSpace(OrderByCol)) //if there is a column to sort by
                {
                    //do sorting
                    switch (OrderByCol)
                    {
                        case ("FirstName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.FirstName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.FirstName);

                                }
                            }
                            break;
                        case ("LastName"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.LastName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.LastName);

                                }
                            }
                            break;
                        case ("Email"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.Email);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.Email);

                                }
                            }
                            break;
                        case ("Cell"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.Cell);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.Cell);

                                }
                            }
                            break;
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.userID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_Admin_Result Add(bl_Admin info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qDuplicate = (from row in metadata.db_User
                                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                                  select row).FirstOrDefault();

                if (qDuplicate == null)
                {
                    var newGuest = new db_User
                    {
                        FirstName = info.FirstName,
                        LastName = info.LastName,
                        Email = info.Email,
                        Cell = info.Cell,
                        PasswordHash = info.PasswordHash,
                    };

                    metadata.db_User.Add(newGuest);
                    metadata.SaveChanges();

                    var result = new bl_Admin_Result
                    {
                        hasError = false,
                    };
                    return result;
                }
                else
                {
                    var result = new bl_Admin_Result
                    {
                        hasError = true,
                        ErrorText = "Email already exist for another Admin"
                    };
                    return result;
                }

            }
        }
        public static bl_Admin_Result Edit(bl_Admin info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_User
                              where row.userID == info.userID
                              select row).FirstOrDefault();

                //Check if their is a duplicate 
                var qDuplicate = (from row in metadata.db_User
                                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                                  && row.userID != info.userID
                                  select row).FirstOrDefault();


                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Admin found");

                var duplicate = qDuplicate;
                if (duplicate == null)
                {
                    item.FirstName = info.FirstName;
                    item.LastName = info.LastName;
                    item.Email = info.Email;
                    item.Cell = info.Cell;

                    metadata.SaveChanges();

                    var result = new bl_Admin_Result
                    {
                        hasError = false
                    };
                    return result;
                }
                else
                {
                    var result = new bl_Admin_Result
                    {
                        hasError = true,
                        ErrorText = "Email already exist for another Admin"
                    };
                    return result;
                }
            }
        }
        public static bl_Admin_Result ChangePassword(bl_Admin info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_User
                              where row.userID == info.userID
                              select row).FirstOrDefault();


                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Admin found");

                    item.PasswordHash = info.PasswordHash;
                    metadata.SaveChanges();

                    var result = new bl_Admin_Result
                    {
                        hasError = false
                    };
                    return result;


            }
        }
        public static void Delete(long userID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_User.Find(userID);
                if (item == null) throw new NullReferenceException("No Admin found to Delete");

                metadata.db_User.Remove(item);
                metadata.SaveChanges();

            }
        }
    }
}
