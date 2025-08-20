namespace ShopAppApi.Infrastructure.Common
{
    /// <summary>
    /// Api Exception class
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        /// <summary>
        /// .Ctor
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException( string message ) : base( message )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NotFoundException( string message, Exception inner ) : base( message, inner )
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NotFoundException( System.Runtime.Serialization.SerializationInfo info,
                                     System.Runtime.Serialization.StreamingContext context ) : base( info, context )
        {
        }
    }
}