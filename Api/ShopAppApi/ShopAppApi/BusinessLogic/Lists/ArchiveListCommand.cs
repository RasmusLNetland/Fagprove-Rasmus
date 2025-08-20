using MediatR;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// ArchiveListCommand
    /// </summary>
    public class ArchiveListCommand : IRequest
    {
        /// <summary>
        /// Id of list
        /// </summary>
        public int ListId { get; set; }

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            ArchiveListCommand? command = obj as ArchiveListCommand;
            if( command is null )
                return false;

            return ListId.Equals( command.ListId );
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return ListId.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}