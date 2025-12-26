using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Infrastructure.Chatwoot.DTOs;


namespace WootchatCRM.Infrastructure.Chatwoot;

/// <summary>
/// پیاده‌سازی کلاینت Chatwoot API با Lazy Configuration
/// </summary>
public class ChatwootApiClient : IChatwootApiClient
{
   private readonly HttpClient _httpClient;
   private readonly ISettingsService _settingsService;

   private bool _isConfigured = false;
   private int _accountId;
   private readonly SemaphoreSlim _configLock = new(1, 1);

   private static readonly JsonSerializerOptions JsonOptions = new()
   {
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
   };

   public ChatwootApiClient(HttpClient httpClient, ISettingsService settingsService)
   {
      _httpClient = httpClient;
      _settingsService = settingsService;
   }

   // ═══════════════════════════════════════════════════════════════
   // Configuration Management
   // ═══════════════════════════════════════════════════════════════

   public void ResetConfiguration()
   {
      _isConfigured = false;
   }

   private async Task EnsureConfiguredAsync()
   {
      if (_isConfigured) return;

      await _configLock.WaitAsync();
      try
      {
         if (_isConfigured) return;

         var settings = await _settingsService.GetChatwootSettingsAsync();

         if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            throw new InvalidOperationException("Chatwoot Base URL is not configured.");

         if (string.IsNullOrWhiteSpace(settings.ApiKey))
            throw new InvalidOperationException("Chatwoot API Key is not configured.");

         if (settings.AccountId <= 0)
            throw new InvalidOperationException("Chatwoot Account ID is not configured.");

         // پیکربندی HttpClient
         var baseUrl = settings.BaseUrl.TrimEnd('/');
         if (!baseUrl.EndsWith("/"))
            baseUrl += "/";

         _httpClient.BaseAddress = new Uri(baseUrl);
         _httpClient.DefaultRequestHeaders.Clear();
         _httpClient.DefaultRequestHeaders.Add("api_access_token", settings.ApiKey);
         _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

         _accountId = settings.AccountId;
         _isConfigured = true;
      }
      finally
      {
         _configLock.Release();
      }
   }

   private string BuildUrl(string endpoint) => $"api/v1/accounts/{_accountId}/{endpoint.TrimStart('/')}";

   // ═══════════════════════════════════════════════════════════════
   // Generic HTTP Methods
   // ═══════════════════════════════════════════════════════════════

   private async Task<ChatwootApiResult<T>> GetAsync<T>(string endpoint)
   {
      try
      {
         await EnsureConfiguredAsync();
         var response = await _httpClient.GetAsync(BuildUrl(endpoint));
         return await HandleResponseAsync<T>(response);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<T>.Failure($"Request failed: {ex.Message}");
      }
   }

   private async Task<ChatwootApiResult<T>> PostAsync<T>(string endpoint, object? content = null)
   {
      try
      {
         await EnsureConfiguredAsync();

         HttpContent? httpContent = null;
         if (content != null)
         {
            var json = JsonSerializer.Serialize(content, JsonOptions);
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
         }

         var response = await _httpClient.PostAsync(BuildUrl(endpoint), httpContent);
         return await HandleResponseAsync<T>(response);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<T>.Failure($"Request failed: {ex.Message}");
      }
   }

   private async Task<ChatwootApiResult<T>> PatchAsync<T>(string endpoint, object? content = null)
   {
      try
      {
         await EnsureConfiguredAsync();

         HttpContent? httpContent = null;
         if (content != null)
         {
            var json = JsonSerializer.Serialize(content, JsonOptions);
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
         }

         var request = new HttpRequestMessage(HttpMethod.Patch, BuildUrl(endpoint))
         {
            Content = httpContent
         };

         var response = await _httpClient.SendAsync(request);
         return await HandleResponseAsync<T>(response);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<T>.Failure($"Request failed: {ex.Message}");
      }
   }

   private async Task<ChatwootApiResult<bool>> DeleteAsync(string endpoint)
   {
      try
      {
         await EnsureConfiguredAsync();
         var response = await _httpClient.DeleteAsync(BuildUrl(endpoint));

         if (response.IsSuccessStatusCode)
            return ChatwootApiResult<bool>.Success(true);

         var error = await response.Content.ReadAsStringAsync();
         return ChatwootApiResult<bool>.Failure(error, (int)response.StatusCode);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<bool>.Failure($"Request failed: {ex.Message}");
      }
   }

   private async Task<ChatwootApiResult<T>> HandleResponseAsync<T>(HttpResponseMessage response)
   {
      var content = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
         if (string.IsNullOrWhiteSpace(content))
            return ChatwootApiResult<T>.Success(default!);

         try
         {
            var data = JsonSerializer.Deserialize<T>(content, JsonOptions);
            return ChatwootApiResult<T>.Success(data!);
         }
         catch (JsonException ex)
         {
            return ChatwootApiResult<T>.Failure($"JSON parse error: {ex.Message}", (int)response.StatusCode);
         }
      }

      // Parse error response
      try
      {
         var error = JsonSerializer.Deserialize<ChatwootApiError>(content, JsonOptions);
         var errorMessage = error?.Message ?? error?.Error ?? content;
         return ChatwootApiResult<T>.Failure(errorMessage, (int)response.StatusCode);
      }
      catch
      {
         return ChatwootApiResult<T>.Failure(content, (int)response.StatusCode);
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Profile & Connection Test
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<ChatwootProfileResponse>> GetProfileAsync()
   {
      try
      {
         await EnsureConfiguredAsync();
         // Profile endpoint بدون account id
         var response = await _httpClient.GetAsync("api/v1/profile");
         return await HandleResponseAsync<ChatwootProfileResponse>(response);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<ChatwootProfileResponse>.Failure($"Request failed: {ex.Message}");
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Inboxes
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<List<ChatwootInbox>>> GetInboxesAsync()
   {
      var result = await GetAsync<ChatwootInboxListResponse>("inboxes");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<ChatwootInbox>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<ChatwootInbox>>.Success(new List<ChatwootInbox>());

      return ChatwootApiResult<List<ChatwootInbox>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<ChatwootInbox>> GetInboxAsync(int inboxId)
   {
      return await GetAsync<ChatwootInbox>($"inboxes/{inboxId}");
   }

   // ═══════════════════════════════════════════════════════════════
   // Contacts
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<ChatwootContactListResponse>> GetContactsAsync(int page = 1)
   {
      return await GetAsync<ChatwootContactListResponse>($"contacts?page={page}");
   }

   public async Task<ChatwootApiResult<ChatwootContact>> GetContactAsync(int contactId)
   {
      return await GetAsync<ChatwootContact>($"contacts/{contactId}");
   }

   public async Task<ChatwootApiResult<ChatwootContact>> CreateContactAsync(ChatwootContactCreateRequest request)
   {
      return await PostAsync<ChatwootContact>("contacts", request);
   }

   public async Task<ChatwootApiResult<ChatwootContact>> UpdateContactAsync(int contactId, ChatwootContactUpdateRequest request)
   {
      return await PatchAsync<ChatwootContact>($"contacts/{contactId}", request);
   }

   public async Task<ChatwootApiResult<bool>> DeleteContactAsync(int contactId)
   {
      return await DeleteAsync($"contacts/{contactId}");
   }

   public async Task<ChatwootApiResult<ChatwootContact?>> SearchContactByPhoneAsync(string phoneNumber)
   {
      try
      {
         await EnsureConfiguredAsync();

         var encodedPhone = Uri.EscapeDataString(phoneNumber);
         var response = await _httpClient.GetAsync(BuildUrl($"contacts/search?q={encodedPhone}"));
         var result = await HandleResponseAsync<ChatwootContactSearchResponse>(response);

         if (result.IsSuccess && result.Data?.Payload != null && result.Data.Payload.Count > 0)
         {
            var contact = result.Data.Payload.FirstOrDefault(c =>
                c.PhoneNumber != null &&
                c.PhoneNumber.Replace(" ", "").Replace("-", "").Contains(
                    phoneNumber.Replace(" ", "").Replace("-", "")));

            return ChatwootApiResult<ChatwootContact?>.Success(contact);
         }

         return ChatwootApiResult<ChatwootContact?>.Success(null);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<ChatwootContact?>.Failure($"Search failed: {ex.Message}");
      }
   }

   public async Task<ChatwootApiResult<ChatwootContact?>> SearchContactByEmailAsync(string email)
   {
      try
      {
         await EnsureConfiguredAsync();

         var encodedEmail = Uri.EscapeDataString(email);
         var response = await _httpClient.GetAsync(BuildUrl($"contacts/search?q={encodedEmail}"));
         var result = await HandleResponseAsync<ChatwootContactSearchResponse>(response);

         if (result.IsSuccess && result.Data?.Payload != null && result.Data.Payload.Count > 0)
         {
            var contact = result.Data.Payload.FirstOrDefault(c =>
                c.Email != null &&
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return ChatwootApiResult<ChatwootContact?>.Success(contact);
         }

         return ChatwootApiResult<ChatwootContact?>.Success(null);
      }
      catch (Exception ex)
      {
         return ChatwootApiResult<ChatwootContact?>.Failure($"Search failed: {ex.Message}");
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Conversations
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<ChatwootConversationListResponse>> GetConversationsAsync(
       string status = "open",
       int page = 1,
       int? inboxId = null,
       int? assigneeId = null)
   {
      var queryParams = new List<string>
        {
            $"status={status}",
            $"page={page}"
        };

      if (inboxId.HasValue)
         queryParams.Add($"inbox_id={inboxId.Value}");

      if (assigneeId.HasValue)
         queryParams.Add($"assignee_id={assigneeId.Value}");

      var query = string.Join("&", queryParams);
      return await GetAsync<ChatwootConversationListResponse>($"conversations?{query}");
   }

   public async Task<ChatwootApiResult<ChatwootConversation>> GetConversationAsync(int conversationId)
   {
      return await GetAsync<ChatwootConversation>($"conversations/{conversationId}");
   }

   public async Task<ChatwootApiResult<ChatwootConversation>> CreateConversationAsync(ChatwootConversationCreateRequest request)
   {
      return await PostAsync<ChatwootConversation>("conversations", request);
   }

   public async Task<ChatwootApiResult<bool>> UpdateConversationStatusAsync(int conversationId, string status)
   {
      var payload = new { status };
      var result = await PostAsync<object>($"conversations/{conversationId}/toggle_status", payload);

      if (result.IsSuccess)
         return ChatwootApiResult<bool>.Success(true);

      return ChatwootApiResult<bool>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<bool>> AssignAgentToConversationAsync(int conversationId, int agentId)
   {
      var payload = new { assignee_id = agentId };
      var result = await PostAsync<object>($"conversations/{conversationId}/assignments", payload);

      if (result.IsSuccess)
         return ChatwootApiResult<bool>.Success(true);

      return ChatwootApiResult<bool>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   // ═══════════════════════════════════════════════════════════════
   // Messages
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<List<ChatwootMessage>>> GetMessagesAsync(int conversationId)
   {
      var result = await GetAsync<ChatwootMessageListResponse>($"conversations/{conversationId}/messages");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<ChatwootMessage>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<ChatwootMessage>>.Success(new List<ChatwootMessage>());

      return ChatwootApiResult<List<ChatwootMessage>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<ChatwootMessage>> SendMessageAsync(int conversationId, ChatwootMessageCreateRequest request)
   {
      return await PostAsync<ChatwootMessage>($"conversations/{conversationId}/messages", request);
   }

   public async Task<ChatwootApiResult<ChatwootMessage>> SendTextMessageAsync(
     int conversationId,
     string content,
     bool isPrivate = false)
   {
      var request = new ChatwootMessageCreateRequest
      {
         Content = content,
         Private = isPrivate
      };

      return await SendMessageAsync(conversationId, request);
   }

   // ═══════════════════════════════════════════════════════════════
   // Agents
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<List<ChatwootAgent>>> GetAgentsAsync()
   {
      var result = await GetAsync<ChatwootAgentListResponse>("agents");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<ChatwootAgent>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<ChatwootAgent>>.Success(new List<ChatwootAgent>());

      return ChatwootApiResult<List<ChatwootAgent>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<List<ChatwootAgent>>> GetInboxAgentsAsync(int inboxId)
   {
      var result = await GetAsync<ChatwootAgentListResponse>($"inboxes/{inboxId}/agents");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<ChatwootAgent>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<ChatwootAgent>>.Success(new List<ChatwootAgent>());

      return ChatwootApiResult<List<ChatwootAgent>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   // ═══════════════════════════════════════════════════════════════
   // Labels
   // ═══════════════════════════════════════════════════════════════

   public async Task<ChatwootApiResult<List<ChatwootLabel>>> GetLabelsAsync()
   {
      var result = await GetAsync<ChatwootLabelListResponse>("labels");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<ChatwootLabel>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<ChatwootLabel>>.Success(new List<ChatwootLabel>());

      return ChatwootApiResult<List<ChatwootLabel>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<List<string>>> GetConversationLabelsAsync(int conversationId)
   {
      var result = await GetAsync<ChatwootConversationLabelResponse>(
          $"conversations/{conversationId}/labels");

      if (result.IsSuccess && result.Data?.Payload != null)
         return ChatwootApiResult<List<string>>.Success(result.Data.Payload);

      if (result.IsSuccess)
         return ChatwootApiResult<List<string>>.Success(new List<string>());

      return ChatwootApiResult<List<string>>.Failure(result.ErrorMessage!, result.StatusCode);
   }

   public async Task<ChatwootApiResult<bool>> AddLabelsToConversationAsync(
       int conversationId,
       List<string> labels)
   {
      var payload = new
      {
         labels
      };

      var result = await PostAsync<object>(
          $"conversations/{conversationId}/labels",
          payload);

      if (result.IsSuccess)
         return ChatwootApiResult<bool>.Success(true);

      return ChatwootApiResult<bool>.Failure(result.ErrorMessage!, result.StatusCode);
   }
}
