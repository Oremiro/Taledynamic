using System;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.DTOs
{
    public class TableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsImmutable { get; set; }
        public TableDto() {}

        public TableDto(Table table)
        {
            if (table == null)
            {
                throw new NullReferenceException("Table entity is empty");
            }

            Id = table.Id;
            Name = table.Name;
        }
    }
}