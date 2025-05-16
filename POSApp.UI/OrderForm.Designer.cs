namespace POSApp.UI
{
    partial class OrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponents()
        {
            dgvOrder = new DataGridView
            {
                Name = "dgvOrder",
                Top = 0,
                Left = 0,
                Width = 500,
                Height = 220,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            dgvOrder.Columns.Add("Code", "Code");
            dgvOrder.Columns.Add("Name", "Name");
            dgvOrder.Columns.Add("Total", "Total");

            var minusBtn = new DataGridViewButtonColumn
            {
                Name = "Minus",
                HeaderText = "-",
                Text = "-",
                UseColumnTextForButtonValue = true,
                Width = 30
            };

            var qtyCol = new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Qty",
                Width = 50
            };

            var plusBtn = new DataGridViewButtonColumn
            {
                Name = "Plus",
                HeaderText = "+",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 30
            };

            dgvOrder.Columns.Add(minusBtn);
            dgvOrder.Columns.Add(qtyCol);
            dgvOrder.Columns.Add(plusBtn);

            dgvOrder.CellEndEdit += DgvOrder_CellEndEdit;
            dgvOrder.CellContentClick += DgvOrder_CellContentClick;

            lblTotal = new Label { Name = "lblTotal", Top = 230, Left = 0, Width = 200, Text = "Total: $0.00" };

            Controls.Add(dgvOrder);
            Controls.Add(lblTotal);
        }

        #endregion
    }
}