using MediatR;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// BatchMarkItemsCommand
    /// </summary>
    public class BatchMarkItemsCommand : IRequest
    {
        /// <summary>
        /// Statuses of items (id, isChecked)
        /// </summary>
        public Dictionary<int, bool> ItemStatuses { get; set; }

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            BatchMarkItemsCommand? command = obj as BatchMarkItemsCommand;
            if( command is null )
                return false;

            return ItemStatuses.Equals( command.ItemStatuses );
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return ItemStatuses.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}