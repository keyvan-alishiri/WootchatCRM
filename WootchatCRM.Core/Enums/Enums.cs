using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WootchatCRM.Core.Enums
{
   public enum TaskStatus
   {
      Pending = 0,
      InProgress = 1,
      Completed = 2,
      Cancelled = 3
   }


   public enum Priority
   {
      Low = 0,
      Medium = 1,
      High = 2,
      Urgent = 3
   }

   public enum ConversationStatus
   {
      Open = 0,
      Pending = 1,
      Resolved = 2,
      Snoozed = 3
   }


   public enum MessageDirection
   {
      Incoming = 0,
      Outgoing = 1
   }


   public enum MessageStatus
   {
      Pending = 0,
      Sent = 1,
      Delivered = 2,
      Read = 3,
      Failed = 4
   }


   public enum ChannelType
   {
      Chatwoot = 0,
      WhatsApp = 1,
      WhatsAppDirect = 2,
      Telegram = 3,
      TelegramDirect = 4,
      Email = 5,
      SMS = 6,
      WebChat = 7
   }

   public enum CampaignStatus
   {
      Draft = 0,
      Scheduled = 1,
      Running = 2,
      Paused = 3,
      Completed = 4,
      Cancelled = 5,
      Failed = 6
   }

  
}
