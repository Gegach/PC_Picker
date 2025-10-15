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
    public partial class FrmStatistics : Form
    {
        private Models.Component component = null;
        
        public FrmStatistics(Models.Component selectedComponent)
        {
            InitializeComponent();
            component = selectedComponent;
        }
        private void ShowStatistics()
        {
            List<Statistic> statistics = StatisticRepository.GetStatistics(component.Id);
            dgvStatistics.DataSource = statistics;
        }

        private void FrmStatistics_Load(object sender, EventArgs e)
        {
            ShowStatistics();
        }

        private void btnCloseView_Click(object sender, EventArgs e)
        {
            FrmComponents frmComponents = new FrmComponents();
            Hide();
            frmComponents.ShowDialog();
            Close();
        }
    }
}
