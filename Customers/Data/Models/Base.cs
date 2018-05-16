using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public abstract class Base
    {
        // / <summary>
        // / Entity Action Type (Add, Update, Delete)
        // / </summary>
        [NotMapped]
        public EntityActionType ActionType { get; set; }
    }
}
