using System;
using System.Windows.Forms;
using POSApp.Core.Interfaces;
using POSApp.Core.Models;

namespace POSApp.UI
{
    public partial class OrderForm : Panel
    {
        private readonly IOrderService _orderService;
        private DataGridView dgvOrder;
        private Label lblTotal;

        public OrderForm(IOrderService orderService)
        {
            _orderService = orderService;
            Width = 500;
            Height = 280;
            InitializeComponents();
            RefreshOrderTable();
        }

        private void DgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var code = dgvOrder.Rows[e.RowIndex].Cells["Code"].Value.ToString();

                if (dgvOrder.Columns[e.ColumnIndex].Name == "Plus")
                {
                    _orderService.IncrementQuantity(code);
                }
                else if (dgvOrder.Columns[e.ColumnIndex].Name == "Minus")
                {
                    _orderService.DecrementQuantity(code);
                }

                // Remove item if quantity is zero
                var updatedItems = _orderService.GetOrderItems().ToList();
                var itemToRemove = updatedItems.FirstOrDefault(x => x.Item.Code == code && x.Quantity == 0);
                if (itemToRemove != null)
                {
                    updatedItems.Remove(itemToRemove);
                    typeof(List<OrderItem>)
                        .GetField("_orderItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(_orderService, updatedItems);
                }

                RefreshOrderTable();
            }
        }

        private void DgvOrder_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOrder.Columns[e.ColumnIndex].Name == "Quantity")
            {
                var code = dgvOrder.Rows[e.RowIndex].Cells["Code"].Value?.ToString();
                var qtyValue = dgvOrder.Rows[e.RowIndex].Cells["Quantity"].Value?.ToString();

                if (int.TryParse(qtyValue, out int newQty))
                {
                    if (newQty <= 0)
                    {
                        var orderItem = _orderService.GetOrderItems().FirstOrDefault(x => x.Item.Code == code);
                        if (orderItem != null)
                        {
                            _orderService.GetOrderItems().ToList().Remove(orderItem);
                        }
                    }
                    else
                    {
                        var orderItem = _orderService.GetOrderItems().FirstOrDefault(x => x.Item.Code == code);
                        if (orderItem != null)
                        {
                            orderItem.Quantity = newQty;
                        }
                    }
                    RefreshOrderTable();
                }
                else
                {
                    MessageBox.Show("Invalid quantity entered.");
                    RefreshOrderTable();
                }
            }
        }

        public void RefreshOrderTable()
        {
            dgvOrder.Rows.Clear();
            foreach (var item in _orderService.GetOrderItems())
            {
                dgvOrder.Rows.Add(
                    item.Item.Code,       // Code column
                    item.Item.Name,       // Name column
                    item.Total,           // Total column
                    "-",                  // Minus button column
                    item.Quantity,        // Quantity (TextBoxColumn)
                    "+"                   // Plus button column
                );
            }

            lblTotal.Text = $"Total: ${_orderService.CalculateTotal():0.00}";
        }

    }
}
