using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.ViewModels;

namespace Accounting.DataLayer
{
    public interface ICustomerRepository
    {
        IEnumerable<Customers> GetCustomerByFilter(string parameter);
        List<Customers> GetAllCustomers();

        List<ListCustomerViewModel> GetNameCustomers(string filter = "");

        int GetCustomerIdByName(string name);

        string GetCustomerNameById(int customerid);

        Customers GetCustomerById(int customerid);

        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerId);
       
    }
}
