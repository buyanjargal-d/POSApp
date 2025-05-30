using System;
using System.Collections.Generic;
using System.Drawing;
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
        private string _selectedItemCode;

        // UI Controls
        private Panel categoryPanel;
        private FlowLayoutPanel itemsFlowPanel;
        private Panel orderPanel;
        private OrderForm modernOrderForm;
        private Button[] categoryButtons;
        private string selectedCategory = "All Menu";

        // Dynamic category color mapping
        private Dictionary<string, Color> categoryColors;

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
            SetupModernUI();
            LoadItems();
        }

        private void SetupModernUI()
        {
            BackColor = Color.MediumPurple;
            Font = new Font("Segoe UI", 9F);
            WindowState = FormWindowState.Maximized;

            SetupCategorySection();
            SetupItemsSection();
            SetupOrderSection();
        }

        private void SetupCategorySection()
        {
            if (categoryPanel != null)
                Controls.Remove(categoryPanel);

            var items = _itemService.GetAllItems().ToList();
            var categories = items
                .GroupBy(i => i.Category)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderBy(c => c.Name)
                .ToList();

            categories.Insert(0, new { Name = "All Menu", Count = items.Count });

            var palette = new[]
            {
                Color.FromArgb(245,158,11),
                Color.FromArgb(236,72,153),
                Color.FromArgb(168,85,247),
                Color.FromArgb(34,197,94),
                Color.FromArgb(239,68,68),
                Color.FromArgb(59,130,246)
            };
            categoryColors = new Dictionary<string, Color>();
            for (int i = 0; i < categories.Count; i++)
                categoryColors[categories[i].Name] = palette[i % palette.Length];

            categoryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            categoryButtons = new Button[categories.Count];
            const int btnWidth = 100, btnHeight = 30, spacing = 10;
            for (int i = 0; i < categories.Count; i++)
            {
                var cat = categories[i];
                var btn = new Button
                {
                    Text = $"{cat.Name} ({cat.Count})",
                    Size = new Size(btnWidth, btnHeight),
                    Location = new Point(10 + i * (btnWidth + spacing), 5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = cat.Name == selectedCategory ? Color.MediumPurple : Color.White,
                    ForeColor = cat.Name == selectedCategory ? Color.White : Color.Gray,
                    Tag = cat.Name,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += CategoryButton_Click;
                categoryButtons[i] = btn;
                categoryPanel.Controls.Add(btn);
            }

            Controls.Add(categoryPanel);
        }

        private void SetupItemsSection()
        {
            var container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            itemsFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent,
                Padding = new Padding(10,40,10,10)
            };
            container.Controls.Add(itemsFlowPanel);
            Controls.Add(container);
        }

        private void SetupOrderSection()
        {
            orderPanel = new Panel { Dock = DockStyle.Right, Width = 300, BackColor = Color.White, Padding = new Padding(8) };
            modernOrderForm = new OrderForm(_orderService) { Dock = DockStyle.Fill };
            orderPanel.Controls.Add(modernOrderForm);
            Controls.Add(orderPanel);
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                selectedCategory = btn.Tag.ToString();
                foreach (var b in categoryButtons)
                {
                    bool sel = b.Tag.ToString() == selectedCategory;
                    b.BackColor = sel ? Color.MediumPurple : Color.White;
                    b.ForeColor = sel ? Color.White : Color.Gray;
                }
                LoadItems();
            }
        }

        private void LoadItems()
        {
            itemsFlowPanel.Controls.Clear();
            var allItems = _itemService.GetAllItems();
            var filtered = selectedCategory == "All Menu"
                ? allItems
                : allItems.Where(i => i.Category == selectedCategory);
            foreach (var item in filtered)
                itemsFlowPanel.Controls.Add(CreateItemCard(item));
        }

        private Panel CreateItemCard(Item item)
        {
            var card = new Panel { Size = new Size(140, 180), BackColor = Color.White, Margin = new Padding(6), Cursor = Cursors.Hand };
            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, card.Width, card.Height, 12, 12));

            PictureBox imgBox = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point((card.Width - 50) / 2, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.None
            };

                if (!string.IsNullOrEmpty(item.ImagePath) && System.IO.File.Exists(item.ImagePath))
                {
                    imgBox.Image = Image.FromFile(item.ImagePath);
                }
                else
                {
                    // create a solid color bitmap as placeholder
                    Bitmap bmp = new Bitmap(50, 50);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(GetCategoryColor(item.Category));
                    }
                    imgBox.Image = bmp;
                }

            imgBox.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, imgBox.Width, imgBox.Height, 25, 25));
            card.Controls.Add(imgBox);

            card.Controls.Add(new Label
            {
                Text = item.Name,
                Font = new Font(Font.FontFamily, 9F, FontStyle.Bold),
                Size = new Size(130, 20),
                Location = new Point(5, 70),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(51, 65, 85)
            });
            card.Controls.Add(new Label
            {
                Text = item.Category,
                Font = new Font(Font.FontFamily, 8F),
                Size = new Size(130, 18),
                Location = new Point(5, 92),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            });
            card.Controls.Add(new Label
            {
                Text = $"${item.UnitPrice:0.00}",
                Font = new Font(Font.FontFamily, 10F, FontStyle.Bold),
                Size = new Size(130, 20),
                Location = new Point(5, 112),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(51, 65, 85)
            });

            card.Click += (s, e) => { _orderService.AddToOrder(item.Code); modernOrderForm.RefreshOrder(); };
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 245, 245);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;
            card.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _selectedItemCode = item.Code;
                    var menu = new ContextMenuStrip();
                    menu.Items.Add("Edit", null, EditMenu_Click);
                    menu.Items.Add("Delete", null, DeleteMenu_Click);
                    menu.Show(card, e.Location);
                }
            };

            return card;
        }

        private Color GetCategoryColor(string category)
        {
            if (categoryColors != null && categoryColors.TryGetValue(category, out var color))
                return color;
            return Color.FromArgb(59, 130, 246);
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;
            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;
            using var form = new EditItemForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Name = form.ItemName;
                item.Category = form.ItemCategory;
                item.UnitPrice = form.ItemPrice;
                _itemService.UpdateItem(item);
                SetupCategorySection();
                LoadItems();
            }
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;
            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;
            if (MessageBox.Show("Confirm delete?", "Delete Item", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _itemService.DeleteItem(item.Id);
                SetupCategorySection();
                LoadItems();
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);
    }
}
