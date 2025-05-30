using POSApp.Core.Interfaces;
using POSApp.Core.Models;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace POSApp.UI
{
    public partial class OrderForm : Panel
    {
        private readonly IOrderService _orderService;
        private Panel orderItemsPanel;
        private Label totalLabel;
        private Button payButton;

        public OrderForm(IOrderService orderService)
        {
            _orderService = orderService;
            SetupUI();
            RefreshOrder();
        }

        private void SetupUI()
        {
            // Panel styling
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Controls.Clear();

            // Header
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.White
            };
            var headerLabel = new Label
            {
                Text = "Current Order",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.MediumPurple,
                AutoSize = true,
                Dock = DockStyle.Left
            };
            headerPanel.Controls.Add(headerLabel);
            this.Controls.Add(headerPanel);

            // Order items container
            orderItemsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(8,28,8,8)
            };
            this.Controls.Add(orderItemsPanel);

            // Bottom panel
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.Transparent
            };

            // Total section
            var totalPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(0)
            };
            totalLabel = new Label
            {
                Text = "TOTAL: $0.00",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            totalPanel.Controls.Add(totalLabel);
            bottomPanel.Controls.Add(totalPanel);

            // Pay button
            payButton = new Button
            {
                Text = "Pay Now",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Bottom,
                Height = 40
            };
            payButton.FlatAppearance.BorderSize = 0;
            payButton.Click += PayButton_Click;
            payButton.Resize += (s, e) =>
            {
                payButton.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, payButton.Width, payButton.Height, 8, 8));
            };
            bottomPanel.Controls.Add(payButton);

            this.Controls.Add(bottomPanel);
        }

        public void RefreshOrder()
        {
            orderItemsPanel.Controls.Clear();
            var items = _orderService.GetOrderItems().ToList();
            int y = 0;
            foreach (var orderItem in items)
            {
                var panel = CreateOrderItemPanel(orderItem, y);
                orderItemsPanel.Controls.Add(panel);
                y += panel.Height + 6;
            }
            totalLabel.Text = $"TOTAL: ${_orderService.CalculateTotal():0.00}";
        }

        private Panel CreateOrderItemPanel(OrderItem orderItem, int yPosition)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Margin = new Padding(0, 4, 0, 4)
            };

            // Name
            var nameLabel = new Label
            {
                Text = orderItem.Item.Name,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                AutoSize = true,
                Location = new Point(12, 8)
            };
            // Total price
            var priceLabel = new Label
            {
                Text = $"${orderItem.Total:0.00}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                AutoSize = true,
                Location = new Point(panel.Width - 80, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Quantity controls
            var minusBtn = new Button
            {
                Text = "-",
                Size = new Size(28, 28),
                Location = new Point(12, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            minusBtn.FlatAppearance.BorderSize = 0;
            minusBtn.Click += (s, e) => { _orderService.DecrementQuantity(orderItem.Item.Code); RefreshOrder(); };

            var qtyLabel = new Label
            {
                Text = orderItem.Quantity.ToString(),
                Size = new Size(36, 28),
                Location = new Point(44, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(51, 65, 85)
            };

            var plusBtn = new Button
            {
                Text = "+",
                Size = new Size(28, 28),
                Location = new Point(84, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            plusBtn.FlatAppearance.BorderSize = 0;
            plusBtn.Click += (s, e) => { _orderService.IncrementQuantity(orderItem.Item.Code); RefreshOrder(); };

            panel.Controls.AddRange(new Control[] { nameLabel, priceLabel, minusBtn, qtyLabel, plusBtn });
            return panel;
        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            var total = _orderService.CalculateTotal();
            if (total == 0)
            {
                MessageBox.Show("Cart is empty.", "Order Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var payForm = new PayForm(total, _orderService);
            if (payForm.ShowDialog() == DialogResult.OK)
            {
                _orderService.ClearOrder();
                RefreshOrder();
            }
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);
    }
}
