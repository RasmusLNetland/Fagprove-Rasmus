using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// UpdateListCommand
    /// </summary>
    public class UpdateListCommand : IRequest
    {
        /// <summary>
        /// UpdateListRequest
        /// </summary>
        public UpdateListRequest Request { get; set; }

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            UpdateListCommand? command = obj as UpdateListCommand;
            if( command is null )
                return false;

            return Request.Equals( command.Request );
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Request.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}