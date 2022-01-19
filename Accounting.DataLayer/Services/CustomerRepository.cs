using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Accounting.DataLayer;
using Accounting.ViewModels;


namespace Accounting.DataLayer
{
    public class CustomerRepository : ICustomerRepository
    {
        private Accounting_DB1Entities db;

        public CustomerRepository(Accounting_DB1Entities context)
        {
            db = context;
        }
        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;

            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;

            }
        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }

        public IEnumerable<Customers> GetCustomerByFilter(string parameter)
        {


            return db.Customers.Where(c => c.FullName.Contains(parameter)
                                                     ||
                                           c.Email.Contains(parameter)
                                                     ||
                                           c.Mobile.Contains(parameter)).ToList();
        }


        public Customers GetCustomerById(int customerid)
        {
            return db.Customers.Find(customerid);
        }

        public int GetCustomerIdByName(string name)
        {
            return db.Customers.First(c => c.FullName == name).CustomersID;
        }

        public string GetCustomerNameById(int customerid)
        {
            return db.Customers.Find(customerid).FullName;
        }

        public List<ListCustomerViewModel> GetNameCustomers(string filter = "")
        {
            if (filter == "")
            {
                return db.Customers.Select(c => new ListCustomerViewModel()
                {
                    CustomerID = c.CustomersID,
                    FullName = c.FullName

                }).ToList();
            }
            return db.Customers.Where(c => c.FullName.Contains(filter)).Select(c => new ListCustomerViewModel()
            {
                CustomerID = c.CustomersID,
                FullName = c.FullName
            }).ToList();
        }




        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;

            }
        }



        public bool UpdateCustomer(Customers customer)
        {
            var local = db.Set<Customers>()
                .Local
                .FirstOrDefault(f => f.CustomersID == customer.CustomersID);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(customer).State = EntityState.Modified;
            return true;
        }
    }
}
