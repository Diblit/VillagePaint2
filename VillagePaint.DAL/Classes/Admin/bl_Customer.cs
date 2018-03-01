
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePaint.DAL.Utils;

namespace VillagePaint.DAL.Classes.Admin
{
    public class bl_Customer_Result
    {
        public long customerID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_Customer
    {
        public long customerID { get; set; }
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Cell { get; set; }
        public string AddressStreet{ get; set; }
        public string AddressSuburb { get; set; }
        public string AddressCity { get; set; }
        public string ZipCode { get; set; }
        public static List<bl_Customer> CustomerList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_Customer                        
                        select new bl_Customer
                        {
                            customerID = row.customerID,
                            CardNumber = row.CardNumber,
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            Email = row.Email,
                            CompanyName = row.CompanyName,
                            Cell = row.Cell,
                            AddressStreet = row.AddressStreet,
                            AddressSuburb = row.AddressSuburb,
                            AddressCity = row.AddressCity,
                            ZipCode = row.ZipCode,
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.FirstName.Contains(search) || r.LastName.Contains(search) || r.CompanyName.Contains(search) || r.Email.Contains(search) || r.CardNumber.Contains(search) || r.Cell.Contains(search));
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
                        case ("CompanyName"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.CompanyName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.CompanyName);

                                }
                            }
                            break;
                        case ("CardNumber"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.CardNumber);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.CardNumber);

                                }
                            }
                            break;
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.CardNumber);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_Customer_Result Add(bl_Customer info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //var qDuplicate = (from row in metadata.bl_Customer
                //                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                //                  select row).FirstOrDefault();

                //if (qDuplicate == null)
                //{
                    var newCustomer = new db_Customer
                    {
                        FirstName = info.FirstName,
                        LastName = info.LastName,
                        Email = info.Email,
                        Cell = info.Cell,
                        CompanyName = info.CompanyName,
                        CardNumber = info.CardNumber,
                        AddressStreet = info.AddressStreet,
                        AddressSuburb = info.AddressSuburb,
                        AddressCity = info.AddressCity,
                        ZipCode = info.ZipCode,                        
                    };

                    metadata.db_Customer.Add(newCustomer);
                    metadata.SaveChanges();

                    var result = new bl_Customer_Result
                    {
                        hasError = false,
                    };
                    return result;
                //}
                //else
                //{
                //    var result = new bl_GuestList_Result
                //    {
                //        hasError = true,
                //        ErrorText = "Email already exist for another Guest"
                //    };
                //    return result;
                //}

            }
        }
        public static bl_Customer_Result Edit(bl_Customer info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qCustomer = (from row in metadata.db_Customer
                                 where row.customerID == info.customerID
                                 select row).FirstOrDefault();

                //Check if their is a duplicate 
                //var qDuplicate = (from row in metadata.db_User
                //                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                //                  && row.userID != info.userID
                //                  select row).FirstOrDefault();


                var item = qCustomer;
                if (item == null) throw new NullReferenceException("No Customer found");

                //var duplicate = qDuplicate;
                //if (duplicate == null)
                //{
                item.CardNumber = info.CardNumber;
                item.FirstName = info.FirstName;
                item.LastName = info.LastName;
                item.Email = info.Email;
                item.CompanyName = info.CompanyName;
                item.Cell = info.Cell;
                item.AddressStreet = info.AddressStreet;
                item.AddressSuburb = info.AddressSuburb;
                item.AddressCity = info.AddressCity;
                item.ZipCode = info.ZipCode;

                metadata.SaveChanges();

                var result = new bl_Customer_Result
                {
                    hasError = false
                };
                return result;
                //}
                //else
                //{
                //    var result = new bl_GuestList_Result
                //    {
                //        hasError = true,
                //        ErrorText = "Email already exist for another Guest"
                //    };
                //    return result;
                //}
            }
        }
        public static void Delete(long customerID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_Customer.Find(customerID);
                if (item == null) throw new NullReferenceException("No Customer found to Delete");

                metadata.db_Customer.Remove(item);
                metadata.SaveChanges();

            }
        }
    }
}
