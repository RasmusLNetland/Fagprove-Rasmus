namespace ShopAppApi.Infrastructure.Common
{
    /// <summary>
    /// Api Exception class
    /// </summary>
    [Serializable]
    public class UnprocessableEntityException : Exception
    {
        /// <summary>
        /// .Ctor
        /// </summary>
        public UnprocessableEntityException()
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        public UnprocessableEntityException( string message ) : base( message )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnprocessableEntityException( string message, Exception inner ) : base( message, inner )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnprocessableEntityException( System.Runtime.Serialization.SerializationInfo info,
                                                System.Runtime.Serialization.StreamingContext context ) : base( info, context )
        {
        }
    }
}