namespace TrackXpert_WebApp.Services
{
	public class TokenService
	{

		private string? _token = null;

		public void SetToken(string? token)
		{
			_token = token;
		}

		public string? GetToken()
		{
			if (_token is not null || _token != string.Empty)
			{
				return _token;
			} else
			{
				return null;
			}
		}
	}
}
