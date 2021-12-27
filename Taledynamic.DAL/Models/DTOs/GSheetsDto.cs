using System;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.DTOs
{
    public class GSheetsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GSheetsDto() {}

        public GSheetsDto(GSheets sheets)
        {
            if (sheets == null)
            {
                throw new NullReferenceException("Table entity is empty");
            }

            Id = sheets.Id;
            Name = sheets.Name;
        }
    }
}