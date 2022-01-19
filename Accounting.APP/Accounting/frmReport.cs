using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer;
using Accounting.Utlity;
using Accounting.ViewModels;

namespace Accounting.APP
{
    public partial class frmReport : Form
    {

        public int TypID = 0;

        public frmReport()
        {
            InitializeComponent();
        }


        private void frmReport_Load(object sender, EventArgs e)
        {


            using (UnitOfWork db= new UnitOfWork())
            {

                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }


            if (TypID == 1)
            {
                this.Text = "گزارش دریافت";
            }
            else
            {
                this.Text = "گزارش پرداخت";
            }
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }


        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();

                DateTime? startDate;
                DateTime? endDate;

                if ((int)cbCustomer.SelectedValue!=0)
                {
                    int customerid = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(c => c.TypeID == TypID  &&  c.CustomerID==customerid));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(c => c.TypeID == TypID ));
                }

                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTitle >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTitle <= endDate.Value).ToList();
                }

                

                dgReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string Customername = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.CustomerID, Customername, accounting.Amount, accounting.DateTitle.ToShamsi(), accounting.Description);
                }
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (RtlMessageBox.Show("آیا از حذف مطمئتن هستید ؟", "هشدار", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.save();
                        Filter();
                    }
                }
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow!=null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frmnewaccounting = new frmNewAccounting();
                frmnewaccounting.AccountID = id;
                if (frmnewaccounting.ShowDialog()==DialogResult.OK)
                {
                    Filter();
                } 
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add(" ");
            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtPrint.Rows.Add(
                    item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString()
                    );
            }
            stiprint.Load(Application.StartupPath + "/Report");
            stiprint.RegData("DT", dtPrint);
            stiprint.Show();
        }
    }
}
