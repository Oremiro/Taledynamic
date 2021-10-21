using System;

namespace Taledynamic.Core.Entities
{
    public class Workspace: BaseEntity
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public User User { get; set; }
    }
}