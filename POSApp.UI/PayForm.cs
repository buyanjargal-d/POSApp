using POSApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSApp.UI
{
    public partial class PayForm : Form
    {
        private readonly decimal _total;

        private Label lblTotalAmount;
        private TextBox txtCash;
        private Button btnConfirm;
        private readonly IOrderService _orderService;

        public PayForm(decimal total, IOrderService orderService)
        {
            _total = total;
            _orderService = orderService;
            InitializeComponent();
        }

        private void PayForm_Load(object sender, EventArgs e)
        {
            lblTotalAmount.Text = $"Total: ${_total:0.00}";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCash.Text, out decimal cashGiven))
            {
                if (cashGiven >= _total)
                {
                    var change = cashGiven - _total;
                    PrintReceipt(cashGiven, change); 
                    MessageBox.Show($"Change: ${change:0.00}", "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Insufficient cash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PrintReceipt(decimal paid, decimal change)
        {
            var items = _orderService.GetOrderItems().ToList();
            var total = _total;

            PrintDocument doc = new PrintDocument();
            doc.PrintPage += (s, e) =>
            {
                float y = 20;
                var font = new Font("Consolas", 10);
                var bold = new Font("Consolas", 12, FontStyle.Bold);
                var brush = Brushes.Black;

                e.Graphics.DrawString("POS RECEIPT", bold, brush, 20, y);
                y += 30;

                foreach (var item in items)
                {
                    string line = $"{item.Item.Name} x{item.Quantity} @ ${item.Item.UnitPrice:0.00}";
                    e.Graphics.DrawString(line, font, brush, 20, y);
                    y += 20;
                }

                y += 10;
                e.Graphics.DrawString($"Total: ${total:0.00}", font, brush, 20, y); y += 20;
                e.Graphics.DrawString($"Paid:  ${paid:0.00}", font, brush, 20, y); y += 20;
                e.Graphics.DrawString($"Change:${change:0.00}", font, brush, 20, y); y += 30;
                e.Graphics.DrawString("Thank you!", font, brush, 20, y);
            };

            new PrintPreviewDialog
            {
                Document = doc,
                Width = 600,
                Height = 800
            }.ShowDialog();
        }

    }
}
