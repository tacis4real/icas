﻿namespace ICAS.Areas.Helpers.DataTable
{
    public class DataTableColumn
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public Search Search { get; set; }
    }
}