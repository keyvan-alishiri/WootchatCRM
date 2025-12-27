using System.Text.Json.Serialization;

namespace WootchatCRM.Infrastructure.Chatwoot.DTOs;

#region Account & Profile

public class ChatwootProfileResponse
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("email")]
   public string Email { get; set; } = string.Empty;

   [JsonPropertyName("accounts")]
   public List<ChatwootAccountInfo> Accounts { get; set; } = new();
}

public class ChatwootAccountInfo
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("role")]
   public string Role { get; set; } = string.Empty;
}

#endregion

#region Inbox

public class ChatwootInbox
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("channel_type")]
   public string ChannelType { get; set; } = string.Empty;

   [JsonPropertyName("greeting_enabled")]
   public bool GreetingEnabled { get; set; }

   [JsonPropertyName("greeting_message")]
   public string? GreetingMessage { get; set; }

   [JsonPropertyName("working_hours_enabled")]
   public bool WorkingHoursEnabled { get; set; }

   [JsonPropertyName("enable_auto_assignment")]
   public bool EnableAutoAssignment { get; set; }

   [JsonPropertyName("out_of_office_message")]
   public string? OutOfOfficeMessage { get; set; }

   [JsonPropertyName("timezone")]
   public string? Timezone { get; set; }

   [JsonPropertyName("phone_number")]
   public string? PhoneNumber { get; set; }

   [JsonPropertyName("webhook_url")]
   public string? WebhookUrl { get; set; }

   [JsonPropertyName("avatar_url")]
   public string? AvatarUrl { get; set; }

   [JsonPropertyName("page_id")]
   public string? PageId { get; set; }

   [JsonPropertyName("widget_color")]
   public string? WidgetColor { get; set; }
}


#region Contact Inbox

/// <summary>
/// ارتباط Contact با Inbox در Chatwoot
/// </summary>
public class ChatwootContactInbox
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("contact_id")]
   public int ContactId { get; set; }

   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }

   [JsonPropertyName("source_id")]
   public string? SourceId { get; set; }

   [JsonPropertyName("created_at")]
   public string? CreatedAt { get; set; }

   [JsonPropertyName("updated_at")]
   public string? UpdatedAt { get; set; }
}

/// <summary>
/// درخواست ایجاد Contact Inbox
/// </summary>
public class ChatwootContactInboxCreateRequest
{
   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }
}

/// <summary>
/// پاسخ لیست Contact Inbox
/// </summary>
public class ChatwootContactInboxListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootContactInbox> Payload { get; set; } = new();
}

#endregion


/// <summary>
/// پاسخ لیست Inbox
/// </summary>
public class ChatwootInboxListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootInbox> Payload { get; set; } = new();
}

#endregion

#region Contact

public class ChatwootContactListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootContact> Payload { get; set; } = new();

   [JsonPropertyName("meta")]
   public ChatwootMeta? Meta { get; set; }
}

public class ChatwootContactResponse
{
   [JsonPropertyName("payload")]
   public ChatwootContactPayload? Payload { get; set; }
}

public class ChatwootContactPayload
{
   [JsonPropertyName("contact")]
   public ChatwootContact? Contact { get; set; }
}

public class ChatwootContact
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("email")]
   public string? Email { get; set; }

   [JsonPropertyName("phone_number")]
   public string? PhoneNumber { get; set; }

   [JsonPropertyName("thumbnail")]
   public string? Thumbnail { get; set; }

   [JsonPropertyName("identifier")]
   public string? Identifier { get; set; }

   [JsonPropertyName("custom_attributes")]
   public Dictionary<string, object>? CustomAttributes { get; set; }

   [JsonPropertyName("created_at")]
   public long CreatedAt { get; set; }

   [JsonPropertyName("last_activity_at")]
   public long? LastActivityAt { get; set; }

   [JsonPropertyName("availability_status")]
   public string? AvailabilityStatus { get; set; }
}

/// <summary>
/// پاسخ جستجوی مخاطب
/// </summary>
public class ChatwootContactSearchResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootContact> Payload { get; set; } = new();
}

public class ChatwootContactCreateRequest
{
   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("email")]
   public string? Email { get; set; }

   [JsonPropertyName("phone_number")]
   public string? PhoneNumber { get; set; }

   [JsonPropertyName("identifier")]
   public string? Identifier { get; set; }

   [JsonPropertyName("custom_attributes")]
   public Dictionary<string, object>? CustomAttributes { get; set; }
}

public class ChatwootContactUpdateRequest
{
   [JsonPropertyName("name")]
   public string? Name { get; set; }

   [JsonPropertyName("email")]
   public string? Email { get; set; }

   [JsonPropertyName("phone_number")]
   public string? PhoneNumber { get; set; }

   [JsonPropertyName("custom_attributes")]
   public Dictionary<string, object>? CustomAttributes { get; set; }
}

#endregion

#region Conversation

public class ChatwootConversationListResponse
{
   [JsonPropertyName("data")]
   public ChatwootConversationData Data { get; set; } = new();

   [JsonPropertyName("meta")]
   public ChatwootMeta? Meta { get; set; }
}

public class ChatwootConversationData
{
   [JsonPropertyName("payload")]
   public List<ChatwootConversation> Payload { get; set; } = new();
}

public class ChatwootConversation
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("account_id")]
   public int AccountId { get; set; }

   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }

   [JsonPropertyName("status")]
   public string Status { get; set; } = string.Empty;

   [JsonPropertyName("priority")]
   public string? Priority { get; set; }

   [JsonPropertyName("unread_count")]
   public int UnreadCount { get; set; }

   [JsonPropertyName("agent_last_seen_at")]
   public long? AgentLastSeenAt { get; set; }

   [JsonPropertyName("contact_last_seen_at")]
   public long? ContactLastSeenAt { get; set; }

   [JsonPropertyName("last_non_activity_message")]
   public ChatwootMessage? LastMessage { get; set; }

   [JsonPropertyName("meta")]
   public ChatwootConversationMeta? Meta { get; set; }

   [JsonPropertyName("created_at")]
   public long CreatedAt { get; set; }

   [JsonPropertyName("timestamp")]
   public long? Timestamp { get; set; }

   [JsonPropertyName("can_reply")]
   public bool CanReply { get; set; }

   [JsonPropertyName("channel")]
   public string? Channel { get; set; }
}

public class ChatwootConversationMeta
{
   [JsonPropertyName("sender")]
   public ChatwootContact? Sender { get; set; }

   [JsonPropertyName("assignee")]
   public ChatwootAgent? Assignee { get; set; }

   [JsonPropertyName("team")]
   public ChatwootTeam? Team { get; set; }
}

public class ChatwootTeam
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;
}

public class ChatwootConversationCreateRequest
{
   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }

   [JsonPropertyName("contact_id")]
   public int ContactId { get; set; }

   [JsonPropertyName("source_id")]
   public string? SourceId { get; set; }  // 🆕 اضافه شد

   [JsonPropertyName("status")]
   public string Status { get; set; } = "open";

   [JsonPropertyName("assignee_id")]
   public int? AssigneeId { get; set; }

   [JsonPropertyName("team_id")]
   public int? TeamId { get; set; }

   [JsonPropertyName("message")]
   public ChatwootInitialMessage? Message { get; set; }
}


public class ChatwootInitialMessage
{
   [JsonPropertyName("content")]
   public string Content { get; set; } = string.Empty;
}

public class ChatwootConversationStatusUpdateRequest
{
   [JsonPropertyName("status")]
   public string Status { get; set; } = string.Empty;
}

/// <summary>
/// پاسخ برچسب‌های مکالمه
/// </summary>
public class ChatwootConversationLabelResponse
{
   [JsonPropertyName("payload")]
   public List<string> Payload { get; set; } = new();
}

#endregion

#region Message

public class ChatwootMessageListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootMessage> Payload { get; set; } = new();

   [JsonPropertyName("meta")]
   public ChatwootMeta? Meta { get; set; }
}

public class ChatwootMessage
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("content")]
   public string? Content { get; set; }

   [JsonPropertyName("account_id")]
   public int AccountId { get; set; }

   [JsonPropertyName("inbox_id")]
   public int InboxId { get; set; }

   [JsonPropertyName("conversation_id")]
   public int ConversationId { get; set; }

   [JsonPropertyName("message_type")]
   public int MessageType { get; set; } // 0=incoming, 1=outgoing, 2=activity

   [JsonPropertyName("content_type")]
   public string ContentType { get; set; } = "text";

   [JsonPropertyName("content_attributes")]
   public Dictionary<string, object>? ContentAttributes { get; set; }

   [JsonPropertyName("private")]
   public bool Private { get; set; }

   [JsonPropertyName("sender")]
   public ChatwootSender? Sender { get; set; }

   [JsonPropertyName("attachments")]
   public List<ChatwootAttachment>? Attachments { get; set; }

   [JsonPropertyName("created_at")]
   public long CreatedAt { get; set; }
}

public class ChatwootSender
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("email")]
   public string? Email { get; set; }

   [JsonPropertyName("type")]
   public string Type { get; set; } = string.Empty; // "contact" or "user"

   [JsonPropertyName("thumbnail")]
   public string? Thumbnail { get; set; }
}

public class ChatwootMessageCreateRequest
{
   [JsonPropertyName("content")]
   public string Content { get; set; } = string.Empty;

   [JsonPropertyName("message_type")]
   public string MessageType { get; set; } = "outgoing";

   [JsonPropertyName("private")]
   public bool Private { get; set; } = false;

   [JsonPropertyName("content_type")]
   public string ContentType { get; set; } = "text";

   [JsonPropertyName("content_attributes")]
   public Dictionary<string, object>? ContentAttributes { get; set; }
}

public class ChatwootAttachment
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("message_id")]
   public int MessageId { get; set; }

   [JsonPropertyName("file_type")]
   public string FileType { get; set; } = string.Empty;

   [JsonPropertyName("account_id")]
   public int AccountId { get; set; }

   [JsonPropertyName("data_url")]
   public string DataUrl { get; set; } = string.Empty;

   [JsonPropertyName("thumb_url")]
   public string? ThumbUrl { get; set; }

   [JsonPropertyName("file_size")]
   public long? FileSize { get; set; }
}

#endregion

#region Agent

public class ChatwootAgentListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootAgent> Payload { get; set; } = new();
}

public class ChatwootAgent
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;

   [JsonPropertyName("email")]
   public string Email { get; set; } = string.Empty;

   [JsonPropertyName("available_name")]
   public string? AvailableName { get; set; }

   [JsonPropertyName("thumbnail")]
   public string? Thumbnail { get; set; }

   [JsonPropertyName("availability_status")]
   public string? AvailabilityStatus { get; set; }

   [JsonPropertyName("role")]
   public string? Role { get; set; }
}

#endregion

#region Label (Tags)

public class ChatwootLabelListResponse
{
   [JsonPropertyName("payload")]
   public List<ChatwootLabel> Payload { get; set; } = new();
}

public class ChatwootLabel
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("title")]
   public string Title { get; set; } = string.Empty;

   [JsonPropertyName("description")]
   public string? Description { get; set; }

   [JsonPropertyName("color")]
   public string Color { get; set; } = "#1f93ff";

   [JsonPropertyName("show_on_sidebar")]
   public bool ShowOnSidebar { get; set; }
}

#endregion

#region Common

public class ChatwootMeta
{
   [JsonPropertyName("count")]
   public int? Count { get; set; }

   [JsonPropertyName("current_page")]
   public int? CurrentPage { get; set; }

   [JsonPropertyName("all_count")]
   public int? AllCount { get; set; }

   [JsonPropertyName("mine_count")]
   public int? MineCount { get; set; }

   [JsonPropertyName("unassigned_count")]
   public int? UnassignedCount { get; set; }
}

public class ChatwootApiError
{
   [JsonPropertyName("success")]
   public bool Success { get; set; }

   [JsonPropertyName("error")]
   public string? Error { get; set; }

   [JsonPropertyName("message")]
   public string? Message { get; set; }

   [JsonPropertyName("errors")]
   public List<string>? Errors { get; set; }
}

//// <summary>
/// نتیجه عملیات API
/// </summary>
public class ChatwootApiResult<T>
{
   public bool IsSuccess { get; set; }
   public T? Data { get; set; }
   public string? ErrorMessage { get; set; }
   public int StatusCode { get; set; }

   public static ChatwootApiResult<T> Success(T data) => new()
   {
      IsSuccess = true,
      Data = data,
      StatusCode = 200
   };

   public static ChatwootApiResult<T> Failure(string error, int statusCode = 0) => new()
   {
      IsSuccess = false,
      ErrorMessage = error,
      StatusCode = statusCode
   };
}

#endregion
