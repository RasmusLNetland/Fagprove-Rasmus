using Microsoft.Data.SqlClient;
using Polly;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Auxiliary;

public static class SqlDatabaseUtils
{
    public static async Task<SqlConnection> GetConnectionAsync( string connectionString, CancellationToken cancellationToken )
    {
        try
        {
            SqlConnection connection = new(connectionString);

            await ExecuteSqlWithRetry( async () => await connection.OpenAsync( cancellationToken ) );

            return connection;
        }
        catch(Exception exception)
        {
            throw new Exception( "Failed to get db connection. ", exception );
        }
    }

    public static async Task ExecuteSqlWithRetry( Func<Task> action )
    {
        await Policy.Handle<TimeoutException>()
                    .WaitAndRetryAsync( 3, retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2, retryAttempt ) ) )
                    .ExecuteAsync( async () => { await action(); } );
    }

    public static async Task<SqlDataReader> ExecuteSqlWithRetry( Func<Task<SqlDataReader>> action )
    {
        return await Policy.Handle<TimeoutException>()
                           .WaitAndRetryAsync( 3, retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2, retryAttempt ) ) )
                           .ExecuteAsync( async () => await action() );
    }

    public static async Task<int> ExecuteSqlNonQueryWithRetry( Func<Task<int>> action )
    {
        return await Policy.Handle<TimeoutException>()
                           .WaitAndRetryAsync( 3, retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2, retryAttempt ) ) )
                           .ExecuteAsync( async () => await action() );
    }
}