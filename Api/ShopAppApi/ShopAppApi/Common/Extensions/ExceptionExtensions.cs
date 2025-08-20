using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Common.Extensions;

public static class ExceptionExtensions
{
    public static Exception ProcessAndLogException( this Exception exception, ILogger logger,
                                                    [CallerMemberName] string callingFunction = "",
                                                    [CallerFilePath] string sourceFilePath = "" )
    {
        if( string.IsNullOrEmpty( callingFunction ) )
            callingFunction = "Unknown calling function";

        switch( exception )
        {
            case TaskCanceledException _:
                logger.LogWarning( $"Task was cancelled: {callingFunction} in: {sourceFilePath}" );
                break;
            case OperationCanceledException _:
                logger.LogWarning( $"Operation was cancelled: {callingFunction} in: {sourceFilePath}" );
                break;
            default:
                logger.LogError( "{@exception}: {@callingFunction} in: {@sourceFilePath}", exception, callingFunction, sourceFilePath );
                break;
        }

        return exception;
    }
}