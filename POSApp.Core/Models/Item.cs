﻿namespace POSApp.Core.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public string Category { get; set; }
    }
}