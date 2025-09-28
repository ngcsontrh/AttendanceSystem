using Microsoft.Extensions.Options;
using RAttendanceSystem.Application.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RAttendanceSystem.Identity.Implements
{
    internal class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _keycloakOptions;
        private string? _accessToken;
        private DateTime _tokenExpirationTime = DateTime.MinValue;

        public IdentityService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
        {
            _httpClient = httpClient;
            _keycloakOptions = keycloakOptions.Value;
        }

        public async Task<GetRolesResult> GetRolesAsync()
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var rolesUrl = $"{_keycloakOptions.Domain}/admin/realms/{_keycloakOptions.Realm}/roles";
                var rolesResponse = await _httpClient.GetAsync(rolesUrl);

                if (!rolesResponse.IsSuccessStatusCode)
                {
                    var errorContent = await rolesResponse.Content.ReadAsStringAsync();
                    return new GetRolesResult
                    {
                        Succeeded = false,
                        Errors = new[] { errorContent }
                    };
                }

                var rolesJson = await rolesResponse.Content.ReadAsStringAsync();
                var roles = JsonSerializer.Deserialize<List<RoleRepresentation>>(rolesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new GetRolesResult
                {
                    Succeeded = true,
                    Roles = roles ?? [],
                };
            }
            catch (Exception ex)
            {

                throw;
            }            
        }

        public async Task<AssignRoleResult> AssignRoleAsync(AssignRoleRequest request)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var userId = request.UserId;
                var roleId = request.RoleId;
                var roleName = request.RoleName;

                var url = $"{_keycloakOptions.Domain}/admin/realms/{_keycloakOptions.Realm}/users/{userId}/role-mappings/realm";

                var roleRepresentation = new[]
                    {
                    new
                    {
                        id = roleId.ToString(),
                        name = roleName
                    }
                };

                var json = JsonSerializer.Serialize(roleRepresentation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new AssignRoleResult
                    {
                        Succeeded = false,
                        Errors = new[] { errorContent }
                    };
                }

                return new AssignRoleResult { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new AssignRoleResult
                {
                    Succeeded = false,
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var username = request.Username;
                var email = request.Email;
                var password = request.Password;
                var employeeId = request.EmployeeId;
                var adminUrl = $"{_keycloakOptions.Domain}/admin/realms/{_keycloakOptions.Realm}/users";

                var userRepresentation = new
                {
                    username = username,
                    email = email,
                    emailVerified = true,
                    enabled = true,
                    credentials = new[]
                    {
                        new
                        {
                            type = "password",
                            value = password,
                            temporary = false
                        }
                    },
                    attributes = new
                    {
                        employeeId = new[] { employeeId }
                    }
                };

                var json = JsonSerializer.Serialize(userRepresentation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await _httpClient.PostAsync(adminUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new CreateUserResult
                    {
                        Succeeded = false,
                        Errors = new[] { errorContent }
                    };
                }

                var createdUserId = response.Headers.Location?.AbsolutePath.Split('/').Last()!;
                return new CreateUserResult
                {
                    Succeeded = true,
                    UserId = createdUserId
                };
            }
            catch (HttpRequestException ex)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = new[] { $"Network error occurred while creating user in Keycloak: {ex.Message}" }
                };
            }
            catch (TaskCanceledException ex)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = new[] { $"Request timeout occurred while creating user in Keycloak: {ex.Message}" }
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<GetRoleByNameResult> GetRoleByNameAsync(GetRoleByNameRequest request)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var roleName = request.RoleName;

                var getRoleRequest = $"{_keycloakOptions.Domain}/admin/realms/{_keycloakOptions.Realm}/roles/{roleName}";
                var getRoleResponse = await _httpClient.GetAsync(getRoleRequest);
                if (!getRoleResponse.IsSuccessStatusCode)
                {
                    var errorContent = await getRoleResponse.Content.ReadAsStringAsync();
                    return new GetRoleByNameResult
                    {
                        Succeeded = false,
                        Errors = new[] { errorContent }
                    };
                }
                var roleJson = await getRoleResponse.Content.ReadAsStringAsync();
                var role = JsonSerializer.Deserialize<RoleRepresentation>(roleJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new GetRoleByNameResult
                {
                    Succeeded = true,
                    Role = role
                };
            }
            catch (Exception ex)
            {
                return new GetRoleByNameResult
                {
                    Succeeded = false,
                    Errors = new[] { ex.Message }
                };
            }            
        }

        #region Get Access Token
        private async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpirationTime)
            {
                return _accessToken;
            }

            try
            {
                var tokenEndpoint = $"{_keycloakOptions.Domain}/realms/{_keycloakOptions.Realm}/protocol/openid-connect/token";

                var parameters = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", _keycloakOptions.ClientId),
                    new("client_secret", _keycloakOptions.ClientSecret)
                };

                var content = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync(tokenEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        _accessToken = tokenResponse.AccessToken;
                        _tokenExpirationTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn - 30);
                        return _accessToken;
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to get access token from Keycloak. Status: {response.StatusCode}, Error: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Network error occurred while getting access token from Keycloak: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new InvalidOperationException($"Request timeout occurred while getting access token from Keycloak: {ex.Message}", ex);
            }
        }

        private class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; } = string.Empty;
        }
        #endregion
    }

    internal class KeycloakOptions
    {
        public const string SectionName = "Keycloak";

        public string Domain { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
        public string AdminUsername { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}