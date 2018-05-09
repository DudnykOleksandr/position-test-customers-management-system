using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public abstract class Base
    {
        [NotMapped]
        public EntityActionType ActionType { get; set; }
    }
}
