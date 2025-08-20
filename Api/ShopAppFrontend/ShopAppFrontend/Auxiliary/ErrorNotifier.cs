namespace ShopAppFrontend.Auxiliary
{
    public class ErrorNotifier
    {
        public event Action<string>? OnError;

        public void NotifyError( string message )
        {
            OnError?.Invoke( string.IsNullOrEmpty( message ) ? "Unknown error occured" : message );
        }
    }
}