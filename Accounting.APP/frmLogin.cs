using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;
using Accounting.DataLayer;

namespace Accounting.APP
{
    public partial class frmLogin : Form
    {
        public bool IsEdit = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (IsEdit==true)
                    {
                        var login = db.LoginRepository.Get().First();
                        login.UserName = txtUserName.Text  ;
                         login.Password = txtPassword.Text;
                        db.LoginRepository.Update(login);
                        db.save();
                        Application.Restart();
                    }
                    else
                    {
                        if (db.LoginRepository.Get(c => c.UserName == txtUserName.Text && c.Password == txtPassword.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            RtlMessageBox.Show("کاربری یافت نشود");
                        }
                    }
                   
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (IsEdit==true)
            {
                this.Text = "تنضیمات ورود به برنامه ";
                btnLogin.Text = "ذخیره تغیرات";
                using (UnitOfWork db=new UnitOfWork())
                {
                    var login = db.LoginRepository.Get().First();
                    txtUserName.Text = login.UserName;
                    txtPassword.Text = login.Password;
                }
            }
        }
    }
}
