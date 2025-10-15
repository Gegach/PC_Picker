using PC_Picker.Models;
using PC_Picker.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Picker
{
    public partial class FrmAdd : Form
    {
        private bool isUpdate = false;
        private bool isReadOnly = false;
        private Models.Component component = null;
        public FrmAdd(Models.Component selectedComponent, bool isReadOnly_)
        {
            InitializeComponent();
            component = selectedComponent;
            isReadOnly = isReadOnly_;
        }
        private void FrmAdd_Load(object sender, EventArgs e)
        {
            this.Controls.Add(gboMemory);

            cboCategory.Items.AddRange(new string[]
            {
                "Processor",
                "Motherboard",
                "GraphicsCard",
                "Memory",
                "Storage"
            });
            cboSocket.Items.AddRange(new string[] 
            {
                "LGA1200",
                "LGA1700",
                "AM4",
                "AM5",
                "sTRX4"
            });
            cboMemoryTypeGraphics.Items.AddRange(new string[]
            {
                "GDDR5",
                "GDDR6",
                "GDDR6X",
                "HBM2"
            });
            cboInterfaceGraphics.Items.AddRange(new string[]
            {
                "PCIe 3.0",
                "PCIe 4.0",
                "PCIe 5.0"
            });
            cboMemoryTypeMemory.Items.AddRange(new string[]
            {
                "DDR3",
                "DDR4",
                "DDR5"
            });
            cboStorageType.Items.AddRange(new string[]
            {
                "HDD",
                "SSD",
                "NVMe"
            });
            cboInterfaceStorage.Items.AddRange(new string[]
            {
                "SATA III",
                "PCIe 3.0",
                "PCIe 4.0",
                "NVMe"
            });
            cboChipset.Items.AddRange(new string[]
            {
                "B550",
                "Z790",
                "X670"
            });
            cboSocketMotherboard.Items.AddRange(new string[]
            {
                "AM4",
                "AM5",
                "LGA1200",
                "LGA1700"
            });
            cboFormFactor.Items.AddRange(new string[]
            {
                "ATX",
                "Micro-ATX",
                "Mini-ITX"
            });

            gboProcessor.Hide();
            gboGraphicsCard.Hide();
            gboStorage.Hide();
            gboMotherboard.Hide();

            if (component != null)
            {
                txtName.Text = component.Name;
                txtManufacturer.Text = component.Manufacturer;
                txtPrice.Text = component.Price.ToString();
                cboCategory.SelectedItem = component.Category;
                cboCategory.Enabled = false;
                btnConfirm.Text = "Ažuriraj";
                this.Text = "Ažuriranje komponente";
                isUpdate = true;

                if (component is Storage storage)
                {
                    gboStorage.Visible = true;
                    cboStorageType.SelectedItem = storage.StorageType;
                    cboInterfaceStorage.SelectedItem = storage.Interface;
                    numStorageCapacity.Value = storage.Capacity;
                }
                else if (component is GraphicsCard graphicsCard)
                {
                    gboGraphicsCard.Visible = true;
                    cboMemoryTypeGraphics.SelectedItem = graphicsCard.MemoryType;
                    numMemoryGraphics.Value = graphicsCard.Memory;
                    cboInterfaceGraphics.SelectedItem = graphicsCard.Interface;
                    txtGPU.Text = graphicsCard.GPU;
                }
                else if (component is Motherboard motherboard)
                {
                    gboMotherboard.Visible = true;
                    cboSocketMotherboard.SelectedItem = motherboard.Socket;
                    cboChipset.SelectedItem = motherboard.Chipset;
                    cboFormFactor.SelectedItem = motherboard.FormFactor;
                    numRamSlots.Value = motherboard.RamSlots;
                }
                else if (component is Processor processor)
                {
                    gboProcessor.Visible = true;
                    cboSocket.SelectedItem = processor.Socket;
                    numCoreCount.Value = processor.CoreCount;
                    txtFrequency.Text = processor.Frequency.ToString();
                    numCache.Value = processor.Cache;
                }
                else if (component is Memory memory)
                {
                    gboMemory.Visible = true;
                    cboMemoryTypeMemory.SelectedItem = memory.MemoryType;
                    numCapacityMemory.Value = memory.Capacity;
                    numSpeed.Value = memory.Speed;
                }

                if (isReadOnly)
                {
                    txtName.ReadOnly = true;
                    txtManufacturer.ReadOnly = true;
                    txtPrice.ReadOnly = true;
                    btnConfirm.Text = "Odaberi";
                    this.Text = "Pregled komponente";
                    isUpdate = false;
                    SetReadOnly(gboGraphicsCard);
                    SetReadOnly(gboProcessor);
                    SetReadOnly(gboMotherboard);
                    SetReadOnly(gboStorage);
                    SetReadOnly(gboMemory);
                }
            }
            else
            {
                cboCategory.SelectedIndex = 0;
                cboCategory.Enabled = true;
                gboProcessor.Visible = true;
                btnConfirm.Text = "Dodaj komponentu";
            }
        }

        private void SetReadOnly(GroupBox gbo)
        {
            foreach(Control c in gbo.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.ReadOnly = true;
                }
                else if (c is ComboBox cbo)
                {
                    cbo.Enabled = false;
                }
                else if (c is NumericUpDown num)
                {
                    num.Enabled = false;
                }
            }
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            gboMemory.Hide();
            gboProcessor.Hide();
            gboMotherboard.Hide();
            gboStorage.Hide();
            gboGraphicsCard.Hide();


            string selected = cboCategory.SelectedItem.ToString();

            switch (selected)
            {
                case "Processor":
                    gboProcessor.Visible = true;
                    break;
                case "Motherboard":
                    gboMotherboard.Visible = true;
                    break;
                case "GraphicsCard":
                    gboGraphicsCard.Visible = true;
                    break;
                case "Memory":
                    gboMemory.Visible = true;
                    break;
                case "Storage":
                    gboStorage.Visible = true;
                    break;
            }
        }

        private bool Validation(string category)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Unesite naziv komponente.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (txtPrice.Text == "")
            {
                MessageBox.Show("Unesite cijenu komponente.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (cboCategory.SelectedItem == null)
            {
                MessageBox.Show("Odaberite kategoriju komponente.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            switch (category)
            {
                case "Processor":
                    if (numCoreCount.Text == "")
                    {
                        MessageBox.Show("Unesite broj jezgri procesora.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (txtFrequency.Text == "")
                    {
                        MessageBox.Show("Unesite frekvenciju procesora.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboSocket.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite socket procesora.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    break;
                case "Motherboard":
                    if (cboSocketMotherboard.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite socket matične ploče.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboChipset.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite chipset matične ploče.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    break;
                case "GraphicsCard":
                    if (numMemoryGraphics.Text == "")
                    {
                        MessageBox.Show("Unesite količinu memorije grafičke kartice.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboInterfaceGraphics.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite sučelje grafičke kartice.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (txtGPU.Text == "")
                    {
                        MessageBox.Show("Unesite naziv GPU-a grafičke kartice.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    break;
                case "Memory":
                    if (numCapacityMemory.Text == "")
                    {
                        MessageBox.Show("Unesite kapacitet memorije.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboMemoryTypeMemory.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite tip memorije.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    break;
                case "Storage":
                    if (numStorageCapacity.Text == "")
                    {
                        MessageBox.Show("Unesite kapacitet pohrane.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboStorageType.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite tip pohrane.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    if (cboInterfaceStorage.SelectedItem == null)
                    {
                        MessageBox.Show("Odaberite sučelje pohrane.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                    break;
            }
            return false;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            switch (cboCategory.SelectedItem)
            {
                case "Processor":
                    {
                        if (Validation("Processor"))
                        {
                            break;
                        }
                        Processor processor = new Processor
                        {
                            Name = txtName.Text,
                            Manufacturer = txtManufacturer.Text,
                            Price = decimal.Parse(txtPrice.Text),
                            Category = cboCategory.SelectedItem.ToString(),
                            Socket = cboSocket.SelectedItem.ToString(),
                            CoreCount = int.Parse(numCoreCount.Text),
                            Frequency = float.Parse(txtFrequency.Text),
                            Cache = int.Parse(numCache.Text)
                        };
                        if (isUpdate)
                        {
                            processor.Id = component.Id;
                            ComponentRepository.UpdateComponent(processor, FrmLogin.LoggedEmployee);
                        }
                        else if (!isReadOnly)
                        {
                            ComponentRepository.AddComponent(processor, FrmLogin.LoggedEmployee);
                        }
                        else
                        {
                            processor.Id = component.Id;
                            StatisticRepository.AddStatistic(processor, FrmLogin.LoggedEmployee);
                        }
                        break;
                    }
                case "Motherboard":
                    {
                        Validation("Motherboard");
                        Motherboard motherboard = new Motherboard
                        {
                            Name = txtName.Text,
                            Manufacturer = txtManufacturer.Text,
                            Price = decimal.Parse(txtPrice.Text),
                            Category = cboCategory.SelectedItem.ToString(),
                            Socket = cboSocketMotherboard.SelectedItem.ToString(),
                            Chipset = cboChipset.SelectedItem.ToString(),
                            FormFactor = cboFormFactor.SelectedItem.ToString(),
                            RamSlots = int.Parse(numRamSlots.Text)
                        };
                        if (isUpdate)
                        {
                            motherboard.Id = component.Id;
                            ComponentRepository.UpdateComponent(motherboard, FrmLogin.LoggedEmployee);
                        }
                        else if (!isReadOnly)
                        {
                            ComponentRepository.AddComponent(motherboard, FrmLogin.LoggedEmployee);
                        }
                        else
                        {
                            motherboard.Id = component.Id;
                            StatisticRepository.AddStatistic(motherboard, FrmLogin.LoggedEmployee);
                        }
                        break;
                    }
                case "GraphicsCard":
                    {
                        Validation("GraphicsCard");
                        GraphicsCard graphicsCard = new GraphicsCard
                        {
                            Name = txtName.Text,
                            Manufacturer = txtManufacturer.Text,
                            Price = decimal.Parse(txtPrice.Text),
                            Category = cboCategory.SelectedItem.ToString(),
                            MemoryType = cboMemoryTypeGraphics.SelectedItem.ToString(),
                            Memory = int.Parse(numMemoryGraphics.Text),
                            Interface = cboInterfaceGraphics.SelectedItem.ToString(),
                            GPU = txtGPU.Text
                        };
                        if (isUpdate)
                        {
                            graphicsCard.Id = component.Id;
                            ComponentRepository.UpdateComponent(graphicsCard, FrmLogin.LoggedEmployee);
                        }
                        else if (!isReadOnly)
                        {
                            ComponentRepository.AddComponent(graphicsCard, FrmLogin.LoggedEmployee);
                        }
                        else
                        {
                            graphicsCard.Id = component.Id;
                            StatisticRepository.AddStatistic(graphicsCard, FrmLogin.LoggedEmployee);
                        }
                        break;
                    }
                case "Memory":
                    {
                        Validation("Memory");
                        Memory memory = new Memory
                        {
                            Name = txtName.Text,
                            Manufacturer = txtManufacturer.Text,
                            Price = decimal.Parse(txtPrice.Text),
                            Category = cboCategory.SelectedItem.ToString(),
                            MemoryType = cboMemoryTypeMemory.SelectedItem.ToString(),
                            Capacity = int.Parse(numCapacityMemory.Text),
                            Speed = int.Parse(numSpeed.Text)
                        };
                        if (isUpdate)
                        {
                            memory.Id = component.Id;
                            ComponentRepository.UpdateComponent(memory, FrmLogin.LoggedEmployee);
                        }
                        else if (!isReadOnly)
                        {
                            ComponentRepository.AddComponent(memory, FrmLogin.LoggedEmployee);
                        }
                        else
                        {
                            memory.Id = component.Id;
                            StatisticRepository.AddStatistic(memory, FrmLogin.LoggedEmployee);
                        }
                        break;
                    }
                case "Storage":
                    {
                        Validation("Storage");
                        Storage storage = new Storage
                        {
                            Name = txtName.Text,
                            Manufacturer = txtManufacturer.Text,
                            Price = decimal.Parse(txtPrice.Text),
                            Category = cboCategory.SelectedItem.ToString(),
                            StorageType = cboStorageType.SelectedItem.ToString(),
                            Interface = cboInterfaceStorage.SelectedItem.ToString(),
                            Capacity = int.Parse(numStorageCapacity.Text)
                        };
                        if (isUpdate)
                        {
                            storage.Id = component.Id;
                            ComponentRepository.UpdateComponent(storage, FrmLogin.LoggedEmployee);
                        }
                        else if (!isReadOnly)
                        {
                            ComponentRepository.AddComponent(storage, FrmLogin.LoggedEmployee);
                        }
                        else
                        {
                            storage.Id = component.Id;
                            StatisticRepository.AddStatistic(storage, FrmLogin.LoggedEmployee);
                        }
                        break;
                    }
            }
            MessageBox.Show("Komponenta uspješno dodana/azurirana.", "Uspjeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FrmComponents frmComponents = new FrmComponents();
            Hide();
            frmComponents.ShowDialog();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FrmComponents frmComponents = new FrmComponents();
            Hide();
            frmComponents.ShowDialog();
            Close();
        }
    }
}
