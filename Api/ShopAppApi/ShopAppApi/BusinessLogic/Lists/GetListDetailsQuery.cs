using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Lists
{
    /// <summary>
    /// GetListDetailsQuery
    /// </summary>
    public class GetListDetailsQuery : IRequest<ListDetailsResponse>
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
            GetListDetailsQuery? query = obj as GetListDetailsQuery;
            if( query is null )
                return false;

            return ListId == query.ListId;
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