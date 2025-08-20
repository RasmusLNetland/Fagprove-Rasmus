namespace ShopAppApi.Infrastructure.Common
{
    /// <summary>
    /// Api Exception class
    /// </summary>
    [Serializable]
    public class BadRequestException : Exception
    {
        /// <summary>
        /// .Ctor
        /// </summary>
        public BadRequestException()
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        public BadRequestException( string message ) : base( message )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public BadRequestException( string message, Exception inner ) : base( message, inner )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BadRequestException( System.Runtime.Serialization.SerializationInfo info,
                                       System.Runtime.Serialization.StreamingContext context ) : base( info, context )
        {
        }
    }
}