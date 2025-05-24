using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using POSApp.Core.Interfaces;
using POSApp.Core.Models;

namespace POSApp.UI
{
    public partial class MainForm : Form
    {
        private readonly IAuthService _authService;
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly User _currentUser;
        private MenuStrip menuStrip;
        private ComboBox cmbCategories;
        private DataGridView dgvItems;
        private TextBox txtItemCode;
        private Button btnPay;
        private OrderForm orderForm;

        private string _selectedItemCode;

        public MainForm(
            IAuthService authService,
            IItemService itemService,
            IOrderService orderService,
            User currentUser)
        {
            _authService = authService;
            _itemService = itemService;
            _orderService = orderService;
            _currentUser = currentUser;

            InitializeComponent();
            SetupMenu();
            SetupUI();
        }

        #region Menu Setup

        private void SetupMenu()
        {
            this.menuStrip = new MenuStrip();

            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.Click += (s, e) =>
            {
                MessageBox.Show("Need help? Call: 9999-8888", "Support");
            };

            this.menuStrip.Items.Add(helpMenu);
            this.MainMenuStrip = this.menuStrip;
            this.Controls.Add(this.menuStrip);
        }

        #endregion

        #region UI Initialization

        private void SetupUI()
        {
            this.SuspendLayout();
            this.cmbCategories = new ComboBox
            {
                Name = "cmbCategories",
                Location = new System.Drawing.Point(10, this.menuStrip.Bottom + 10),
                Size = new System.Drawing.Size(200, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                TabIndex = 0
            };
            this.cmbCategories.SelectedIndexChanged += (s, e) => LoadItems();
            this.Controls.Add(this.cmbCategories);
            this.dgvItems = new DataGridView
            {
                Name = "dgvItems",
                Location = new System.Drawing.Point(10, this.cmbCategories.Bottom + 10),
                Size = new System.Drawing.Size(500, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                TabIndex = 1
            };
            this.dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Code", DataPropertyName = "Code" });
            this.dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name" });
            this.dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Category", DataPropertyName = "Category" });

            this.dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;
            this.dgvItems.MouseDown += DgvItems_MouseDown;
            if (_currentUser.Role == Role.Manager)
            {
                var contextMenu = new ContextMenuStrip();
                var editMenu = new ToolStripMenuItem("Edit");
                var deleteMenu = new ToolStripMenuItem("Delete");

                editMenu.Click += EditMenu_Click;
                deleteMenu.Click += DeleteMenu_Click;

                contextMenu.Items.AddRange(new[] { editMenu, deleteMenu });
                this.dgvItems.ContextMenuStrip = contextMenu;
            }

            this.Controls.Add(this.dgvItems);
            this.txtItemCode = new TextBox
            {
                Name = "txtItemCode",
                Location = new System.Drawing.Point(this.dgvItems.Right + 20, this.dgvItems.Top),
                Size = new System.Drawing.Size(200, 24),
                TabIndex = 2
            };
            this.txtItemCode.KeyDown += txtItemCode_KeyDown;
            this.Controls.Add(this.txtItemCode);
            this.btnPay = new Button
            {
                Name = "btnPay",
                Text = "Pay",
                Location = new System.Drawing.Point(this.txtItemCode.Left, this.txtItemCode.Bottom + 20),
                Size = new System.Drawing.Size(75, 25),
                TabIndex = 3
            };
            this.btnPay.Click += btnPay_Click;
            this.Controls.Add(this.btnPay);
            this.orderForm = new OrderForm(_orderService)
            {
                Location = new System.Drawing.Point(this.txtItemCode.Right + 30, this.dgvItems.Top),
                TabIndex = 4
            };
            this.Controls.Add(this.orderForm);

            LoadCategories();
            LoadItems();

            this.ResumeLayout(false);
            this.PerformLayout();
        }


        #endregion

        #region Data Loading

        private void LoadCategories()
        {
            var all = _itemService.GetAllItems();
            var cats = all.Select(i => i.Category)
                              .Distinct()
                              .OrderBy(c => c)
                              .ToList();

            cats.Insert(0, "All");
            this.cmbCategories.DataSource = cats;
        }

        private void LoadItems()
        {
            var selectedCat = this.cmbCategories.SelectedItem?.ToString();
            var allItems = _itemService.GetAllItems();

            var filtered = string.IsNullOrWhiteSpace(selectedCat) || selectedCat == "All"
                ? allItems
                : allItems.Where(i => i.Category == selectedCat);

            this.dgvItems.DataSource = filtered.ToList();
        }

        #endregion

        #region Event Handlers

        private void DgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var code = this.dgvItems
                .Rows[e.RowIndex]
                .Cells["Code"]
                .Value
                .ToString();

            _orderService.AddToOrder(code);
            orderForm.RefreshOrderTable();
        }

        private void DgvItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var hit = this.dgvItems.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;

            this.dgvItems.ClearSelection();
            this.dgvItems.Rows[hit.RowIndex].Selected = true;
            _selectedItemCode = this.dgvItems.Rows[hit.RowIndex]
                                    .Cells["Code"]
                                    .Value
                                    .ToString();
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;

            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;

            using (var form = new EditItemForm(item))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Name = form.ItemName;
                    item.Category = form.ItemCategory;
                    item.UnitPrice = form.ItemPrice;

                    _itemService.UpdateItem(item);
                    LoadCategories();
                    LoadItems();
                }
            }
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;

            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _itemService.DeleteItem(item.Id);
                LoadCategories();
                LoadItems();
            }
        }

        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (sender is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
            {
                _orderService.AddToOrder(txt.Text.Trim());
                orderForm.RefreshOrderTable();
                txt.Clear();
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            var total = _orderService.CalculateTotal();
            if (total == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            using (var payForm = new PayForm(total, _orderService))
            {
                if (payForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(
                        "Receipt printed.\nThank you for your purchase!",
                        "Receipt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _orderService.ClearOrder();
                    orderForm.RefreshOrderTable();
                }
            }
        }

        #endregion
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            using (var prompt = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            })
            {
                var textLabel = new Label
                {
                    Left = 20,
                    Top = 20,
                    Text = text,
                    Width = 240
                };

                var textBox = new TextBox
                {
                    Left = 20,
                    Top = 50,
                    Width = 240
                };

                var confirmation = new Button
                {
                    Text = "OK",
                    Left = 170,
                    Width = 90,
                    Top = 80,
                    DialogResult = DialogResult.OK
                };
                confirmation.Click += (sender, e) => prompt.Close();

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK
                    ? textBox.Text
                    : string.Empty;
            }
        }
    }
}
