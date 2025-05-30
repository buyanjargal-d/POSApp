using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POSApp.Core.Interfaces;

namespace POSApp.UI
{
    public partial class PayForm : Form
    {
        private readonly decimal _total;
        private readonly IOrderService _orderService;

        private Label lblTitle;
        private Label lblAmount;
        private Label lblChange;
        private NumericUpDown numCash;
        private Button btnConfirm;

        public PayForm(decimal total, IOrderService orderService)
        {
            _total = total;
            _orderService = orderService;
            InitializeComponent();
            Load += PayForm_Load;
        }

        

        private void PayForm_Load(object sender, EventArgs e)
        {
            numCash.Value = _total;
            UpdateChange();
        }

        private void UpdateChange()
        {
            var cash = numCash.Value;
            var change = cash > _total ? cash - _total : 0;
            lblChange.Text = $"Change: ${change:0.00}";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            var cash = numCash.Value;
            if (cash < _total)
            {
                MessageBox.Show("Insufficient cash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var change = cash - _total;
            PrintReceipt(cash, change);
            //MessageBox.Show($"Payment successful! Change: ${ change: 0.00} ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void PrintReceipt(decimal paid, decimal change)
        {
            using var doc = new System.Drawing.Printing.PrintDocument();
            doc.PrintPage += (s, e) =>
            {
                float y = 20;
                var font = new Font("Segoui UI", 10F);
                var bold = new Font("Segoe UI", 12F, FontStyle.Bold);
                e.Graphics.DrawString("POS RECEIPT", bold, Brushes.Black, 20, y);
                y += 30;
                foreach (var item in _orderService.GetOrderItems().ToList())
                {
                    var line = $"{item.Item.Name} x{item.Quantity} @ ${item.Item.UnitPrice:0.00}";
                    e.Graphics.DrawString(line, font, Brushes.Black, 20, y);
                    y += 20;
                }
                y += 10;
                e.Graphics.DrawString($"Total: ${_total:0.00}", font, Brushes.Black, 20, y); y += 20;
                e.Graphics.DrawString($"Paid: ${paid:0.00}", font, Brushes.Black, 20, y); y += 20;
                e.Graphics.DrawString($"Change: ${change:0.00}", font, Brushes.Black, 20, y); y += 30;
                e.Graphics.DrawString("Thank you!", font, Brushes.Black, 20, y);
            };
            using var preview = new PrintPreviewDialog { Document = doc, Width = 500, Height = 600 };
            preview.ShowDialog();
        }
    }
}
