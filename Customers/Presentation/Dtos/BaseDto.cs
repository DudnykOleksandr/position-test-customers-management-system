using Data.Models;

namespace Presentation.Dtos
{
    public class BaseDto
    {
        /// <summary>
        /// Entity Action Type (Add, Update, Delete)
        /// </summary>
        public EntityActionType ActionType { get; set; }
    }
}
