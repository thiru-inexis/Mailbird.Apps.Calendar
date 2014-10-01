using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Mailbird.Apps.Calendar.Engine.Enums;
using Mailbird.Apps.Calendar.Engine.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Extensions
{
    /// <summary>
    /// This helper extension makes uses of reflection to Clone values form and object to another
    /// </summary>
    /// <remarks>
    /// This could be performed by using AutoMapper as an alternate. 
    /// But why this ? To Ignore dependencies.
    /// </remarks>

    public static class CloneExtension
    {

        #region Self Clones

        /// <summary>
        /// Clones ColorDefinition
        /// </summary>
        /// <param name="source">Object to clone from</param>
        /// <returns>Deep copy of the source</returns>
        public static Metadata.ColorDefinition Clone(this Metadata.ColorDefinition source)
        {
            Metadata.ColorDefinition result = null;
            if (source != null)
            {
                result = new Metadata.ColorDefinition()
                {
                    Id = source.Id,
                    LocalStorageState = source.LocalStorageState,
                    Background = source.Background,
                    Foreground = source.Foreground
                };
            }

            return result;
        }


        /// <summary>
        /// Clones CalendarList
        /// </summary>
        /// <param name="source">Object to clone from</param>
        /// <returns>Deep copy of the source</returns>
        public static Metadata.CalendarList Clone(this Metadata.CalendarList source)
        {
            Metadata.CalendarList result = null;
            if (source != null)
            {
                result = new Metadata.CalendarList()
                {
                    Etag = source.Etag,
                    Kind = source.Kind,

                    Id = source.Id,
                    LocalStorageState = source.LocalStorageState,
                    IsSelected = source.IsSelected,
                    IsHidden = source.IsHidden,
                    IsDeleted = source.IsDeleted,
                    AccessRole = source.AccessRole,
                    ColorId = source.ColorId,
                    Color = source.Color.Clone(),
                    SummaryOverride = source.SummaryOverride
                };
            }

            return result;
        }


        /// <summary>
        /// Clones Calendar
        /// </summary>
        /// <param name="source">Object to clone from</param>
        /// <returns>Deep copy of the source</returns>
        public static Metadata.Calendar Clone(this Metadata.Calendar source)
        {
            Metadata.Calendar result = null;
            if (source != null)
            {
                result = new Metadata.Calendar()
                {
                    Etag = source.Etag,
                    Kind = source.Kind,

                    Id = source.Id,
                    UserId = source.UserId,
                    LocalStorageState = source.LocalStorageState,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(source.TimeZone.Id),
                    CalenderList = source.CalenderList.Clone()
                };
            }

            return result;
        }


        /// <summary>
        /// Clones Reminder
        /// </summary>
        /// <param name="source">Object to clone from</param>
        /// <returns>Deep copy of the source</returns>
        public static Metadata.Reminder Clone(this Metadata.Reminder source)
        {
            Metadata.Reminder result = null;
            if (source != null)
            {
                result = new Metadata.Reminder()
                {
                     Duration = new TimeSpan(source.Duration.Ticks),
                      Type = source.Type
                };
            }

            return result;
        }



        /// <summary>
        /// Clones Appointment
        /// </summary>
        /// <param name="source">Object to clone from</param>
        /// <returns>Deep copy of the source</returns>
        public static Metadata.Appointment Clone(this Metadata.Appointment source)
        {
            Metadata.Appointment result = null;
            if (source != null)
            {
                result = new Metadata.Appointment()
                {
                    Etag = source.Etag,
                    Kind = source.Kind,

                    Id = source.Id,
                    CalendarId = source.CalendarId,
                    ICalUID = source.ICalUID,
                    LocalStorageState = source.LocalStorageState,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    ColorId = source.ColorId,
                    Color = source.Color.Clone(),
                    StartTime = new DateTime(source.StartTime.Ticks),
                    Transparency = source.Transparency,
                    Status = source.Status,
                    Reminders = source.Reminders.Select(m => m.Clone()).ToList(),
                    EndTime = new DateTime(source.EndTime.Ticks),
                    HtmlLink = source.HtmlLink,
                    IsLocked = source.IsLocked,
                    IsPrivateCopy = source.IsPrivateCopy,
                    IsAnyoneCanAddSelf = source.IsAnyoneCanAddSelf,
                    IsGuestsCanInviteOthers = source.IsGuestsCanInviteOthers,
                    IsGuestsCanModify = source.IsGuestsCanModify,
                    IsGuestsCanSeeOtherGuests = source.IsGuestsCanSeeOtherGuests,
                    Created = new DateTime(source.Created.Ticks),
                    Updated = new DateTime(source.Updated.Ticks)
                };
            }

            return result;
        }

        #endregion


        /// <summary>
        /// Clones google calender event to mailbirds's appointment type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="calender"></param>
        /// <returns></returns>
        public static Appointment CloneTo(this Event source, string calenderId)
        {
            Appointment result = null;

            if (source != null)
            {
                result = new Appointment()
                {
                    Id = source.Id,
                    CalendarId = calenderId,
                    Kind = source.Kind,
                    Etag = source.ETag,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    ColorId = source.ColorId,
                    Created = source.Created ?? DateTime.MinValue,
                    Updated = source.Updated ?? DateTime.MinValue,
                    StartTime = (source.Start != null && source.Start.DateTime.HasValue) ? source.Start.DateTime.Value : DateTime.Now,
                    EndTime = (source.End != null && source.End.DateTime.HasValue) ? source.End.DateTime.Value : DateTime.Now,
                    ICalUID = source.ICalUID,
                    IsAnyoneCanAddSelf = source.AnyoneCanAddSelf ?? false,
                    IsGuestsCanInviteOthers = source.GuestsCanInviteOthers ?? false,
                    IsGuestsCanModify = source.GuestsCanModify ?? false,
                    IsGuestsCanSeeOtherGuests = source.GuestsCanSeeOtherGuests ?? false,
                    IsPrivateCopy = source.PrivateCopy ?? false,
                    IsLocked = source.Locked ?? false,

                    Status = (source.Status.Equals("cancelled", StringComparison.CurrentCultureIgnoreCase) ?  AppointmentStatus.Cancelled
                             :source.Status.Equals("tentative", StringComparison.CurrentCultureIgnoreCase) ?  AppointmentStatus.Tentative
                             :AppointmentStatus.Confirmed),
                    Transparency = (source.Transparency == null || source.Transparency.Equals("transparent", StringComparison.CurrentCultureIgnoreCase) ? AppointmentTransparency.Transparent
                                    :AppointmentTransparency.Opaque),
                    Reminders = (source.Reminders.Overrides != null) 
                                ? source.Reminders.Overrides.Select(m => m.CloneTo()).ToList() 
                                : new List<Reminder>()
                };
            }

            return result;
        }


        public static Reminder CloneTo(this EventReminder source)
        {
            Reminder result = null;
            if (source != null)
            {
                result = new Reminder()
                {
                    Type = (source.Method.Equals("popup", StringComparison.InvariantCultureIgnoreCase) ? ReminderType.PopUp :
                           source.Method.Equals("sms", StringComparison.InvariantCultureIgnoreCase) ? ReminderType.Sms : ReminderType.Email),

                    Duration = TimeSpan.FromMinutes(source.Minutes ?? 0)

                };
            }
            return result;
        }


        public static EventReminder CloneTo(this Reminder source)
        {
            EventReminder result = null;
            if (source != null)
            {
                result = new EventReminder()
                {
                    Method = (source.Type == ReminderType.Sms ? "sms" 
                             :source.Type == ReminderType.Email ? "email"  : "popup"),
                    Minutes = (int)source.Duration.TotalMinutes
                };
            }
            return result;
        }


        /// <summary>
        /// Clones mailbirds's appointment type to google calender event 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Event CloneTo(this Appointment source)
        {
            Event result = null;

            if (source != null)
            {
                result = new Event()
                {
                    Id = source.Id,
                    Kind = source.Kind,
                    ETag = source.Etag,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    ColorId = source.ColorId,
                    Created = source.Created,
                    Updated = source.Updated,
                    Start = new EventDateTime { DateTime = source.EndTime },
                    End = new EventDateTime { DateTime = source.EndTime },
                    ICalUID = source.ICalUID,
                    AnyoneCanAddSelf = source.IsAnyoneCanAddSelf,
                    GuestsCanInviteOthers = source.IsGuestsCanInviteOthers,
                    GuestsCanModify = source.IsGuestsCanModify,
                    GuestsCanSeeOtherGuests = source.IsGuestsCanSeeOtherGuests,
                    PrivateCopy = source.IsPrivateCopy,
                    Locked = source.IsLocked,

                    Status = (source.Status == AppointmentStatus.Cancelled ? "cancelled"
                             : source.Status == AppointmentStatus.Tentative ? "tentative" : "confirmed"),
                    Transparency = (source.Transparency == AppointmentTransparency.Transparent ? "transparent" : "opaque"),

                    Reminders = (source.Reminders == null ? null
                                : new Event.RemindersData() 
                                {
                                     UseDefault = false,
                                     Overrides = source.Reminders.Select(m => m.CloneTo()).ToList()
                                })
                };
            }

            return result;
        }



        public static Metadata.Calendar CloneTo(this Google.Apis.Calendar.v3.Data.Calendar source, string userId)
        {
            Metadata.Calendar result = null;

            if (source != null)
            {
                result = new Metadata.Calendar()
                {
                    Id = source.Id,
                    UserId = userId,
                    Kind = source.Kind,
                    Etag = source.ETag,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location
                    //TimeZone = source.TimeZone
                };
            }

            return result;
        }


        public static Google.Apis.Calendar.v3.Data.Calendar CloneTo(this Metadata.Calendar source)
        {
            Google.Apis.Calendar.v3.Data.Calendar result = null;

            if (source != null)
            {
                result = new Google.Apis.Calendar.v3.Data.Calendar()
                {
                    Id = source.Id,
                    Kind = source.Kind,
                    ETag = source.Etag,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location
                   // TimeZone = source.TimeZone
                };
            }

            return result;
        }


        public static Metadata.CalendarList CloneTo(this CalendarListEntry source)
        {
            Metadata.CalendarList result = null;
            if (source != null)
            {
                var gAccessRole = Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum.FreeBusyReader;
                Enum.TryParse(source.AccessRole, out gAccessRole);

                result = new Metadata.CalendarList(source.Primary ?? false)
                {
                    Id = source.Id,
                    //Summary = source.Summary,
                    //Description = source.Description,
                    //BackgroundColor = source.BackgroundColor,
                    //ForegroundColor = source.ForegroundColor,
                    ColorId = source.ColorId,
                    IsDeleted = source.Deleted ?? false,
                    IsSelected = source.Selected ?? false,
                    AccessRole = gAccessRole.CloneTo()
                };
            }

            return result;
        }


        public static CalendarListEntry CloneTo(this Metadata.CalendarList source)
        {
            CalendarListEntry result = null;

            if (source != null)
            {
                result = new CalendarListEntry()
                {
                    Id = source.Id,
                    //Summary = source.Summary,
                    //Description = source.Description,
                    //BackgroundColor = source.BackgroundColor,
                    //ForegroundColor = source.ForegroundColor,
                    ColorId = source.ColorId,
                    Deleted = source.IsDeleted,
                    Selected = source.IsSelected,
                    Primary = source.IsPrimary,
                    AccessRole = source.AccessRole.CloneTo().ToString()
                };
            }

            return result;
        }






        public static AccessRole CloneTo(this Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum source)
        {
            AccessRole result = AccessRole.Reader;

            if (source == CalendarListResource.ListRequest.MinAccessRoleEnum.Writer)
            {
                result = AccessRole.Writer;
            }

            return result;
        }


        public static Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum CloneTo(this AccessRole source)
        {
            Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum result = CalendarListResource.ListRequest.MinAccessRoleEnum.FreeBusyReader;

            if (source == AccessRole.Writer)
            {
                result = CalendarListResource.ListRequest.MinAccessRoleEnum.Writer;
            }

            return result;
        }




    }
}
