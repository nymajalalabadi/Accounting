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
using ValidationComponents;

namespace Accounting.APP
{
    public partial class frmNewAccounting : Form
    {
        public int AccountID = 0;

        private UnitOfWork db;

        public frmNewAccounting()
        {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e)
        {

             db  = new UnitOfWork();

            dgvCustomers.AutoGenerateColumns = false;

            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();

            if (AccountID != 0)
            {
                var account = db.AccountingRepository.GetById(AccountID);
                txtname.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                txtAmount.Value = account.Amount;
                txtDescription.Text = account.Description;

                if (account.TypeID == 1)
                {
                    rbRecive.Checked = true;
                }
                else
                {
                    rbPey.Checked = true;
                }
                this.Text = "ویرایش";
                btnSave.Text = "ویرایش";
                db.Dispose();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.AutoGenerateColumns = false;

            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtname.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                
                if (rbPey.Checked || rbRecive.Checked)
                {
                    db = new UnitOfWork();

                    DataLayer.Accounting accounting = new DataLayer.Accounting()

                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        Description = txtDescription.Text,
                        DateTitle = DateTime.Now,
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtname.Text),
                        TypeID = (rbRecive.Checked) ? 1 : 2,
                    };

                    if (AccountID == 0)
                    {
                        db.AccountingRepository.Insert(accounting);
                    }
                    else
                    {

                        accounting.ID = AccountID;
                        db.AccountingRepository.Update(accounting);


                    }

                    db.save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("لطفا نوع تراکنش را لنتخاب کنید");
                }
            }
        }
    }
}
