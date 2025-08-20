using System.Data;
using Microsoft.Data.SqlClient;
using ShopAppApi.Auxiliary;
using ShopAppApi.Common.Extensions;
using ShopAppApi.Infrastructure.Common;
using ShopAppApi.Infrastructure.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Infrastructure.DataStorage;

public class InternalDataStorage : IInternalDataStorage
{
    public InternalDataStorage( IConfiguration configuration, ILogger<InternalDataStorage> logger )
    {
        if( configuration is null )
            throw new ArgumentNullException( nameof(configuration) );
        _connectionString = configuration.GetConnectionString( "InternalDbConnection" ) ??
                            throw new ArgumentNullException( nameof(_connectionString) );
        _logger = logger ?? throw new ArgumentNullException( nameof(logger) );
    }

    #region Auth

    public async Task<UserResponse?> GetUserAuthenticationInfoAsync( string email, CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = GetUserAuthenticationInfoSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "email", email );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );
            if( !await reader.ReadAsync( cancellationToken ) )
                return null;

            return new UserResponse()
            {
                Id = Convert.ToInt32( reader["id"] ),
#pragma warning disable CS8601 // Possible null reference assignment. We check DbNull, so it is not null here.
                FullName = DBNull.Value == reader["full_name"] ? string.Empty : Convert.ToString( reader["full_name"] ),
                Email = email,
                PasswordHash = DBNull.Value == reader["passwd_md5"] ? string.Empty : Convert.ToString( reader["passwd_md5"] ),
                PasswordSalt = DBNull.Value == reader["passwd_salt"] ? string.Empty : Convert.ToString( reader["passwd_salt"] )
#pragma warning restore CS8601 // Possible null reference assignment. We check DbNull, so it is not null here.
            };
        }
        catch(Exception ex)
        {
            ex.ProcessAndLogException( _logger );
            return null;
        }
    }

    public async Task<UserResponse> CreateUserAsync( string email, string fullName, string passwordHash, string passwordSalt,
                                                     CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = CreateUserSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "email", email );
            command.Parameters.AddWithValue( "full_name", fullName );
            command.Parameters.AddWithValue( "passwd_md5", passwordHash );
            command.Parameters.AddWithValue( "passwd_salt", passwordSalt );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );
            if( !await reader.ReadAsync( cancellationToken ) )
                throw new UnprocessableEntityException( "User was not created." );

            int id = Convert.ToInt32( reader["id"] );

            return new UserResponse
            {
                Id = id,
                FullName = fullName,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task<string> CreateRefreshTokenAsync( string email, CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = CreateRefreshTokenSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "email", email );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );
            if( !await reader.ReadAsync( cancellationToken ) || DBNull.Value == reader["token"] )
                throw new UnprocessableEntityException( "Refresh token was not created" );

            return Convert.ToString( reader["token"] )!;
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task<(string, UserResponse)> RenewRefreshTokenAsync( string token, CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = RenewRefreshTokenSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "token", token );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );
            if( !await reader.ReadAsync( cancellationToken ) )
                throw new UnprocessableEntityException( "Failed to renew refresh token" );

            int userId = Convert.ToInt32( reader["id"] );
            string? email = DBNull.Value == reader["email"] ? null : Convert.ToString( reader["email"] );
            string? fullName = DBNull.Value == reader["full_name"] ? null : Convert.ToString( reader["full_name"] );

            string? newRefreshToken = await reader.NextResultAsync( cancellationToken ) && await reader.ReadAsync( cancellationToken )
                ? Convert.ToString( reader["token"] )
                : null;

            if( newRefreshToken is null || userId <= 0 || string.IsNullOrEmpty( email ) || string.IsNullOrEmpty( fullName ) )
                throw new UnprocessableEntityException( "Failed to renew refresh token" );

            return (newRefreshToken, new UserResponse
            {
                Id = userId,
                Email = email,
                FullName = fullName
            });
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    #endregion

    #region Lists

    public async Task<IEnumerable<ListResponse>> GetListsByUserIdAsync( int userId, CancellationToken cancellationToken = default )
    {
        try
        {
            List<ListResponse> lists = new();
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = GetListsByUserIdSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "user_id", userId );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );

            while( await reader.ReadAsync( cancellationToken ) )
            {
                int id = Convert.ToInt32( reader["id"] );
                string name = Convert.ToString( reader["name"] ) ?? string.Empty;
                DateTime createdOn = Convert.ToDateTime( reader["created_on"] );
                DateTime? completedOn = DBNull.Value == reader["completed_on"] ? null : Convert.ToDateTime( reader["completed_on"] );

                lists.Add( new ListResponse()
                {
                    Id = id,
                    Name = name,
                    CreatedOn = createdOn,
                    CompletedOn = completedOn,
                    UserId = userId
                } );
            }

            return lists;
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task<ListResponse> CreateListAsync( string listName, IEnumerable<ItemCreationRequest> listItems, int userId,
                                                     CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = CreateListSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;

            DataTable dtItems = CreateListItemsTable( listItems );
            command.Parameters.AddWithValue( "name", listName );
            command.Parameters.AddWithValue( "user_id", userId );
            command.Parameters.AddWithValue( "list_items", dtItems );

            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );
            if( !await reader.ReadAsync( cancellationToken ) )
                throw new Exception( "List was not created." );

            int id = Convert.ToInt32( reader["id"] );
            DateTime createdOn = Convert.ToDateTime( reader["created_on"] );
            DateTime? completedOn = DBNull.Value == reader["completed_on"] ? null : Convert.ToDateTime( reader["completed_on"] );

            if( !await reader.NextResultAsync( cancellationToken ) )
                throw new Exception( "Items were not returned" );

            List<ItemResponse> items = new();

            while( await reader.ReadAsync( cancellationToken ) )
            {
                int itemId = Convert.ToInt32( reader["id"] );
                int listId = Convert.ToInt32( reader["list_id"] );
                string name = Convert.ToString( reader["name"] ) ?? string.Empty;
                DateTime itemCreatedOn = Convert.ToDateTime( reader["created_on"] );
                DateTime? checkedOn = DBNull.Value == reader["checked_on"] ? null : Convert.ToDateTime( reader["checked_on"] );

                items.Add( new ItemResponse
                {
                    Id = itemId,
                    ListId = listId,
                    Name = name,
                    CreatedOn = itemCreatedOn,
                    CheckedOn = checkedOn
                } );
            }

            return new ListResponse
            {
                Id = id,
                Name = listName,
                CreatedOn = createdOn,
                CompletedOn = completedOn,
                UserId = userId,
                Items = items
            };
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task<IEnumerable<ItemResponse>> GetItemsForListAsync( int listId, CancellationToken cancellationToken = default )
    {
        try
        {
            List<ItemResponse> items = new();
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = GetItemsForListSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddWithValue( "list_id", listId );
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );

            while( await reader.ReadAsync( cancellationToken ) )
            {
                int id = Convert.ToInt32( reader["id"] );
                string name = Convert.ToString( reader["name"] ) ?? string.Empty;
                DateTime createdOn = Convert.ToDateTime( reader["created_on"] );
                DateTime? checkedOn = DBNull.Value == reader["checked_on"] ? null : Convert.ToDateTime( reader["checked_on"] );
                int count = DBNull.Value == reader["count"] ? 1 : Convert.ToInt32( reader["count"] );

                items.Add( new ItemResponse
                {
                    Id = id,
                    Name = name,
                    CreatedOn = createdOn,
                    CheckedOn = checkedOn,
                    ListId = listId,
                    Count = count
                } );
            }

            return items;
        }
        catch(Exception ex)
        {
            ex.ProcessAndLogException( _logger );
            return new List<ItemResponse>();
        }
    }

    public async Task BatchMarkItemsAsCheckedAsync( Dictionary<int, bool> itemStatuses,
                                                    CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = SetItemsCheckedSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;

            DataTable dtItems = CreateItemsChecksTable( itemStatuses );
            command.Parameters.AddWithValue( "items", dtItems );

            await SqlDatabaseUtils.ExecuteSqlNonQueryWithRetry( async () => await command.ExecuteNonQueryAsync( cancellationToken ) );
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task<IEnumerable<ListResponse>> GetTemplateListsAsync( CancellationToken cancellationToken = default )
    {
        try
        {
            List<ListResponse> lists = new();
            Dictionary<int, List<ItemResponse>> itemsPerListDict = new();
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = GetTemplateListsSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;
            await using SqlDataReader reader =
                await SqlDatabaseUtils.ExecuteSqlWithRetry( async () => await command.ExecuteReaderAsync( cancellationToken ) );

            while( await reader.ReadAsync( cancellationToken ) )
            {
                int id = Convert.ToInt32( reader["id"] );
                string name = Convert.ToString( reader["name"] ) ?? string.Empty;

                lists.Add( new ListResponse()
                {
                    Id = id,
                    Name = name
                } );
            }

            if( !await reader.NextResultAsync( cancellationToken ) )
                throw new Exception( "Items were not returned." );

            while( await reader.ReadAsync( cancellationToken ) )
            {
                int id = Convert.ToInt32( reader["id"] );
                int listId = Convert.ToInt32( reader["list_id"] );
                int count = Convert.ToInt32( reader["count"] );
                string name = Convert.ToString( reader["name"] ) ?? string.Empty;

                if( !itemsPerListDict.ContainsKey( listId ) )
                    itemsPerListDict[listId] = new List<ItemResponse>();
                itemsPerListDict[listId].Add( new ItemResponse
                {
                    Id = id,
                    ListId = listId,
                    Name = name,
                    Count = count
                } );
            }

            return MergeItemsWithList( lists, itemsPerListDict );
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task UpdateListAsync( UpdateListRequest request, CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = UpdateListSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;

            DataTable dtItems = CreateEditListItemsTable( request.Items );
            command.Parameters.AddWithValue( "list_id", request.Id );

            command.Parameters.AddWithValue( "name", request.Name );
            command.Parameters.AddWithValue( "items", dtItems );

            await SqlDatabaseUtils.ExecuteSqlNonQueryWithRetry( async () => await command.ExecuteNonQueryAsync( cancellationToken ) );
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    public async Task DeleteListAsync( int id, CancellationToken cancellationToken = default )
    {
        try
        {
            await using SqlConnection connection = await SqlDatabaseUtils.GetConnectionAsync( _connectionString, cancellationToken );
            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = DeleteListSpName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = _timeout;

            command.Parameters.AddWithValue( "id", id );

            await SqlDatabaseUtils.ExecuteSqlNonQueryWithRetry( async () => await command.ExecuteNonQueryAsync( cancellationToken ) );
        }
        catch(Exception ex)
        {
            throw ex.ProcessAndLogException( _logger );
        }
    }

    #endregion

    private DataTable CreateListItemsTable( IEnumerable<ItemCreationRequest> items )
    {
        DataTable dtItems = new("listItems");
        dtItems.Columns.Add( "list_id", typeof(int) );
        dtItems.Columns.Add( "name", typeof(string) );
        dtItems.Columns.Add( "created_on", typeof(DateTime) );
        dtItems.Columns.Add( "checked_on", typeof(DateTime) );
        dtItems.Columns.Add( "count", typeof(int) );
        foreach( ItemCreationRequest item in items )
        {
            dtItems.Rows.Add( 0, item.Name, DateTime.UtcNow, null, item.Count );
        }

        return dtItems;
    }

    private DataTable CreateEditListItemsTable( IEnumerable<UpdateItemRequest> items )
    {
        DataTable dtItems = new("items");
        DataColumn idRow = new("id", typeof(int));
        idRow.AllowDBNull = true;
        dtItems.Columns.Add( idRow );
        dtItems.Columns.Add( "name", typeof(string) );
        dtItems.Columns.Add( "count", typeof(int) );
        foreach( UpdateItemRequest item in items )
        {
            object idValue = item.Id.HasValue ? item.Id.Value : DBNull.Value;
            dtItems.Rows.Add( idValue, item.Name, item.Count );
        }

        return dtItems;
    }

    private DataTable CreateItemsChecksTable( Dictionary<int, bool> itemStatuses )
    {
        DataTable dtItems = new("ItemChecksTable");
        dtItems.Columns.Add( "Id", typeof(int) );
        dtItems.Columns.Add( "IsChecked", typeof(bool) );
        foreach( int id in itemStatuses.Keys )
        {
            dtItems.Rows.Add( id, itemStatuses[id] );
        }

        return dtItems;
    }

    private IEnumerable<ListResponse> MergeItemsWithList( List<ListResponse> lists, Dictionary<int, List<ItemResponse>> itemsPerListDict )
    {
        foreach( ListResponse list in lists )
        {
            if( itemsPerListDict.TryGetValue( list.Id, out List<ItemResponse>? items ) )
                list.Items = items;
        }

        return lists;
    }

    public const string GetUserAuthenticationInfoSpName = "ShopApp.GetUserAuthenticationInfo";
    public const string CreateUserSpName = "ShopApp.CreateUser";
    public const string CreateRefreshTokenSpName = "ShopApp.CreateRefreshToken";
    public const string RenewRefreshTokenSpName = "ShopApp.RenewRefreshToken";

    public const string CreateListSpName = "ShopApp.CreateList";
    public const string GetListsByUserIdSpName = "ShopApp.GetListsByUser";
    public const string GetItemsForListSpName = "ShopApp.GetItemsForList";
    public const string SetItemsCheckedSpName = "ShopApp.MarkItemsAsChecked";
    public const string GetTemplateListsSpName = "ShopApp.GetTemplateLists";
    public const string UpdateListSpName = "ShopApp.UpdateList";
    public const string DeleteListSpName = "ShopApp.DeleteList";

    private readonly int _timeout = (int)TimeSpan.FromMinutes( 3 ).TotalSeconds;
    private readonly string _connectionString;
    private readonly ILogger<InternalDataStorage> _logger;
}