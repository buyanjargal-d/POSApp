using System;
using System.Windows.Forms;

namespace POSApp.UI
{
    partial class OrderForm
    {
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support – do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponents()
        {
            this.SuspendLayout();
            this.dgvOrder = new DataGridView
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
            this.dgvOrder.Columns.Add("Code", "Code");
            this.dgvOrder.Columns.Add("Name", "Name");
            this.dgvOrder.Columns.Add("Total", "Total");
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

            this.dgvOrder.Columns.Add(minusBtn);
            this.dgvOrder.Columns.Add(qtyCol);
            this.dgvOrder.Columns.Add(plusBtn);
            this.dgvOrder.CellEndEdit += DgvOrder_CellEndEdit;
            this.dgvOrder.CellContentClick += DgvOrder_CellContentClick;
            this.lblTotal = new Label
            {
                Name = "lblTotal",
                Top = 230,
                Left = 0,
                Width = 200,
                Text = "Total: $0.00"
            };
            this.Controls.Add(this.dgvOrder);
            this.Controls.Add(this.lblTotal);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
