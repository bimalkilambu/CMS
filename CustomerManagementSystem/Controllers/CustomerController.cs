using CustomerManagementSystem.Domain;
using CustomerManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace CustomerManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        CMSEntities entities;
        CustomerMapper mapper;

        public CustomerController()
        {
            entities = new CMSEntities();
            mapper = new CustomerMapper();
        }

        // GET: Customer
        public ActionResult Index()
        {
            List<DTO.Customer> customers = CustomerList("", 1, 7, false);
            double totalRows = entities.Customers.Where(c => !c.DateDeleted.HasValue && !c.IsArchive).Count();
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPage = (int)Math.Ceiling(totalRows / 7);
            return View(customers);
        }

        public ActionResult Archive()
        {
            List<DTO.Customer> customers = CustomerList("", 1, 7, true);
            return View(customers);
        }

        public ActionResult Detail(string id = "")
        {
            Domain.Customer customer = new Domain.Customer();

            if (!string.IsNullOrEmpty(id))
            {
                Guid guid = Guid.Empty;

                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out guid))
                {
                    customer = entities.Customers.Where(c => c.Guid.Equals(guid) && !c.DateDeleted.HasValue).FirstOrDefault();
                }
            }

            return View(mapper.Map(customer));
        }

        [HttpPost]
        public JsonResult Save(DTO.Customer customer)
        {
            Domain.Customer model = new Domain.Customer();
            Guid guid = Guid.Empty;
            bool isNewCustomer = true;

            if (!string.IsNullOrEmpty(customer.CustomerId) && Guid.TryParse(customer.CustomerId, out guid) && guid != Guid.Empty)
            {
                isNewCustomer = false;
                model = entities.Customers.Where(c => c.Guid.Equals(guid) && !c.DateDeleted.HasValue).FirstOrDefault();
            }

            model = mapper.Map(customer, model);

            if (isNewCustomer)
            {
                entities.Customers.Add(model);
            };

            entities.SaveChanges();

            return Json(new { CustomerId = model.Guid });
        }

        [HttpPost]
        public JsonResult Delete(string ids)
        {
            string result = string.Empty;
            try
            {
                var idArray = ids.Split(',').ToArray();
                foreach (var id in idArray)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(id, out guid);
                    var customer = entities.Customers.Where(c => c.Guid.Equals(guid)).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.DateDeleted = DateTime.Now;
                    }
                }
                entities.SaveChanges();
                result = "Success";
            }
            catch (Exception ex)
            {
                result = "Failed";
            }

            return Json(new { message = result });
        }

        [HttpPost]
        public JsonResult Archive(string ids)
        {
            string result = string.Empty;
            try
            {
                var idArray = ids.Split(',').ToArray();
                foreach (var id in idArray)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(id, out guid);
                    var customer = entities.Customers.Where(c => c.Guid.Equals(guid)).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.IsArchive = true;
                    }
                }
                entities.SaveChanges();
                result = "Success";
            }
            catch (Exception ex)
            {
                result = "Failed";
            }

            return Json(new { message = result });
        }

        [HttpPost]
        public ActionResult CustomerList(string search, int page, int pageSize)
        {
            try
            {
                List<DTO.Customer> customers = CustomerList(search, page, pageSize, false);
                double totalRows = entities.Customers.Where(c => !c.DateDeleted.HasValue && !c.IsArchive &&
                                    (c.FirstName.Contains(search) || c.LastName.Contains(search) ||
                                    c.Email.Contains(search) || c.Address.Contains(search) ||
                                    c.ContactNumber.Contains(search))).Count();
                ViewBag.CurrentPage = page;
                ViewBag.TotalPage = (int)Math.Ceiling(totalRows / pageSize);
                return PartialView("_partialCustomerList", customers);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex });
            }
        }

        public List<DTO.Customer> CustomerList(string search, int page, int pageSize, bool isArchive)
        {
            var parameter = new List<object>();
            parameter.Add(new SqlParameter("@SearchText", search));
            parameter.Add(new SqlParameter("@pageSize", pageSize));
            parameter.Add(new SqlParameter("@page", page));
            parameter.Add(new SqlParameter("@IsArchive", isArchive));
            return entities.Database.SqlQuery<DTO.Customer>("spGetCustomerList @SearchText, @pageSize, @page, @IsArchive", parameter.ToArray()).ToList();
        }

        public ActionResult ArchiveCustomerList(string search, int page, int pageSize)
        {
            try
            {
                List<DTO.Customer> customers = CustomerList(search, page, pageSize, true);
                double totalRows = entities.Customers.Where(c => !c.DateDeleted.HasValue && !c.IsArchive).Count();
                ViewBag.CurrentPage = page;
                ViewBag.TotalPage = (int)Math.Ceiling(totalRows / pageSize);
                return PartialView("_partialCustomerList", customers);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex });
            }
        }
    }
}