using System;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.DTOs
{
    public class WorkspaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int UserId { get; set; }
        
        public WorkspaceDto() {}

        public WorkspaceDto(Workspace workspace)
        {
            if (workspace == null)
            {
                throw new NullReferenceException("Table entity is empty");
            }

            Id = workspace.Id;
            Name = workspace.Name;
            Created = workspace.Created;
            Modified = workspace.Modified;
            UserId = workspace.UserId;
        }
    }
}