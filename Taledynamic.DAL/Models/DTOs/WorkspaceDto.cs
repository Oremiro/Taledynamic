using System;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.DTOs
{
    public class WorkspaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WorkspaceDto() {}

        public WorkspaceDto(Workspace table)
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