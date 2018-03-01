using VillagePaint.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TCG.WebUtility;
using VillagePaint.DAL.Classes.Admin;
using VillagePaint.Models;
using TCG.Crypto;
using VillagePaint.Utility;

namespace VillagePaint.Api
{
    public class AdminController : ApiController
    {
        #region Customer
        [HttpGet]
        [Route("api/admin/customer/list")]
        public object AdminCustomerList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_Customer.CustomerList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/customer/add")]
        public object CustomerAdd(CustomerInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();               

                var result = bl_Customer.Add(new bl_Customer
                {
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                    CardNumber = Info.CardNumber,
                    CompanyName = Info.CompanyName,
                    AddressStreet = Info.AddressStreet,
                    AddressSuburb = Info.AddressSuburb,
                    AddressCity = Info.AddressCity,
                    ZipCode = Info.ZipCode,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/customer/edit")]
        public object CustomerEdit(CustomerInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_Customer.Edit(new bl_Customer
                {
                    customerID = Info.customerID,
                    CardNumber = Info.CardNumber,
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                    CompanyName = Info.CompanyName,
                    AddressStreet = Info.AddressStreet,
                    AddressSuburb = Info.AddressSuburb,
                    AddressCity = Info.AddressCity,
                    ZipCode = Info.ZipCode
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/customer/delete")]
        public object DeleteCustomer(long customerID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_Customer.Delete(customerID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }
        #endregion

        #region Admin
        [HttpGet]
        [Route("api/admin/admin/list")]
        public object AdminList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_Admin.AdminList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/admin/add")]
        public object AdminAdd(AdminInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                Info.PasswordHash = PasswordManager.encrypt(Info.PasswordHash);

                var result = bl_Admin.Add(new bl_Admin
                {
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                    PasswordHash = Info.PasswordHash,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/admin/edit")]
        public object AdmminEdit(AdminInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_Admin.Edit(new bl_Admin
                {
                    userID = Info.userID,
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/admin/passwordchange")]
        public object AdminPasswordEdit(AdminInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                Info.PasswordHash = PasswordManager.encrypt(Info.PasswordHash);

                var result = bl_Admin.ChangePassword(new bl_Admin
                {
                    userID = Info.userID,
                    PasswordHash = Info.PasswordHash,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/admin/delete")]
        public object DeleteAdmin(long userID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_Admin.Delete(userID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }
        #endregion
    }
}