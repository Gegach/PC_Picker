using PC_Picker.Models;
using PC_Picker.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Picker
{
    public partial class FrmLogin : Form
    {
        public static Employee LoggedEmployee {  get; set; }
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("Korisničko ime nije uneseno!", "Problem",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Lozinka nije unesena!", "Problem",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LoggedEmployee = EmployeeRepository.GetEmployee(txtUsername.Text);
                if (LoggedEmployee != null && LoggedEmployee.CheckPassword(txtPassword.Text))
                {
                    MessageBox.Show("Dobrodošli!", "Prijavljeni ste",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmComponents frmComponents = new FrmComponents();
                    Hide();
                    frmComponents.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Krivi podaci!", "Problem",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
