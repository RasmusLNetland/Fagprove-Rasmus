using MediatR;
using ShopAppApi.Infrastructure.Models;

namespace ShopAppApi.BusinessLogic.Auth
{
    /// <summary>
    /// Command for creating user
    /// </summary>
    public class CreateUserCommand : IRequest<TokenResponse>
    {
        /// <summary>
        /// Registration Request
        /// </summary>
        public RegistrationRequest? Request { get; set; }

        /// <summary>
        /// Compares current object with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object? obj )
        {
            CreateUserCommand? command = obj as CreateUserCommand;
            if( command is null )
                return false;

            return (Request is null && command.Request is null || Request is not null && Request.Equals( command.Request ));
        }

        /// <summary>
        /// Calculates hash-code for current object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Request?.GetHashCode() ?? 0;
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}