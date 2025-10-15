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
    public partial class FrmComponents : Form
    {
        public FrmComponents()
        {
            InitializeComponent();
            this.Click += Form_Click;
        }

        private void Form_Click(object sender, EventArgs e)
        {
            dgvComponents.ClearSelection();
            dgvComponents.CurrentCell = null;
        }

        private void FrmComponents_Load(object sender, EventArgs e)
        {
            ShowComponents();

            cboFilter.Items.AddRange(new string[]
            {
                "Sve",
                "Procesori",
                "Matične ploče",
                "Grafičke kartice",
                "Memorija",
                "Pohrana",
                "Najbiranije"
            });
            cboFilter.SelectedItem = "Sve";
        }

        private void ShowComponents()
        {
            List<Models.Component> components = ComponentRepository.GetComponents();
            dgvComponents.DataSource = components;
        }
        private void SearchComponents(string filter, string search)
        {
            List<Models.Component> components = ComponentRepository.GetComponents();

            List<Models.Component> filtered = new List<Models.Component>();

            switch (filter)
            {
                case "Procesori":
                    filter = "Processor";
                    break;
                case "Matične ploče":
                    filter = "Motherboard";
                    break;
                case "Grafičke kartice":
                    filter = "GraphicsCard";
                    break;
                case "Memorija":
                    filter = "Memory";
                    break;
                case "Pohrana":
                    filter = "Storage";
                    break;
                case "Najbiranije":
                    filtered = StatisticRepository.GetMostSelectedComponents();
                    break;
            }

            if (!(filter == "Najbiranije"))
            {
                foreach (var c in components)
                {
                    bool matchFilter = false;
                    if (filter == "Sve" || c.Category == filter)
                    {
                        matchFilter = true;
                    }

                    bool matchSearch = false;
                    if (string.IsNullOrEmpty(search) || c.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchSearch = true;
                    }

                    if (matchFilter && matchSearch)
                    {
                        filtered.Add(c);
                    }
                }
            }

            dgvComponents.DataSource = filtered;
        }

        private void btnCreateAdd_Click(object sender, EventArgs e)
        {
            Models.Component selectedComponent = null;
            if (dgvComponents.CurrentRow != null)
            {
                selectedComponent = dgvComponents.CurrentRow.DataBoundItem as Models.Component;
            }
            FrmAdd frmAdd = new FrmAdd(selectedComponent, false);
            Hide();
            frmAdd.ShowDialog();
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Models.Component selectedComponent = null;
            if (dgvComponents.CurrentRow != null)
            {
                selectedComponent = dgvComponents.CurrentRow.DataBoundItem as Models.Component;
            }
            if (selectedComponent == null)
            {
                MessageBox.Show("Molim da odaberete komponentu koju želite obrisati.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ComponentRepository.DeleteComponent(selectedComponent);
                MessageBox.Show("Komponenta uspješno obrisana.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowComponents();
            }
        }
        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cboFilter.SelectedItem.ToString();
            string search = txtSearch.Text;

            SearchComponents(selected, search);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string selected = cboFilter.SelectedItem.ToString();
            string search = txtSearch.Text;

            SearchComponents(selected, search);
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Models.Component selectedComponent = null;
            if (dgvComponents.CurrentRow != null)
            {
                selectedComponent = dgvComponents.CurrentRow.DataBoundItem as Models.Component;
            }
            if (selectedComponent == null)
            {
                MessageBox.Show("Molim da odaberete komponentu za pregled detalja.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FrmAdd frmAdd = new FrmAdd(selectedComponent, true);
                Hide();
                frmAdd.ShowDialog();
                Close();
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            Models.Component selectedComponent = null;
            if (dgvComponents.CurrentRow != null)
            {
                selectedComponent = dgvComponents.CurrentRow.DataBoundItem as Models.Component;
            }
            if (selectedComponent == null)
            {
                MessageBox.Show("Molim da odaberete komponentu za pregled statistike.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FrmStatistics frmStatistics = new FrmStatistics(selectedComponent);
                Hide();
                frmStatistics.ShowDialog();
                Close();
            }
        }
    }
}
