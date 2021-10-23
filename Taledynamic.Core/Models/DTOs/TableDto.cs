using System.Threading.Tasks;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;

namespace Taledynamic.Core.Models.DTOs
{
    public class TableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableDto() {}

        public TableDto(Table table)
        {
            if (table == null)
            {
                throw new BadRequestException("Table entity is empty");
            }

            Id = table.Id;
            Name = table.Name;
        }
    }
}