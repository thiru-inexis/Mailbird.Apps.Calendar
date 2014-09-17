using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Mailbird.Apps.Calendar.Engine.Enums;
using Mailbird.Apps.Calendar.Engine.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Clones google calender event to mailbirds's appointment type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="calender"></param>
        /// <returns></returns>
        public static Appointment Clone(this Event source, string calenderId)
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
                             :source.Status.Equals("tentative", StringComparison.CurrentCultureIgnoreCase) ?  AppointmentStatus.UnCertain
                             :AppointmentStatus.Confirmed),
                    Transparency = (source.Transparency == null || source.Transparency.Equals("transparent", StringComparison.CurrentCultureIgnoreCase) ? AppointmentTransparency.Transparent
                                    :AppointmentTransparency.Opaque),
                    Reminders = (source.Reminders.Overrides != null) 
                                ? source.Reminders.Overrides.Select(m => m.Clone()).ToList() 
                                : new List<Reminder>()
                };
            }

            return result;
        }


        public static Reminder Clone(this EventReminder source)
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


        public static EventReminder Clone(this Reminder source)
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
        public static Event Clone(this Appointment source)
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
                             : source.Status == AppointmentStatus.UnCertain ? "tentative" : "confirmed"),
                    Transparency = (source.Transparency == AppointmentTransparency.Transparent ? "transparent" : "opaque"),

                    Reminders = (source.Reminders == null ? null
                                : new Event.RemindersData() 
                                {
                                     UseDefault = false,
                                     Overrides = source.Reminders.Select(m => m.Clone()).ToList()
                                })
                };
            }

            return result;
        }



        public static Metadata.Calendar Clone(this Google.Apis.Calendar.v3.Data.Calendar source)
        {
            Metadata.Calendar result = null;

            if (source != null)
            {
                result = new Metadata.Calendar()
                {
                    Id = source.Id,
                    Kind = source.Kind,
                    Etag = source.ETag,
                    Summary = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    TimeZone = source.TimeZone
                };
            }

            return result;
        }


        public static Google.Apis.Calendar.v3.Data.Calendar Clone(this Metadata.Calendar source)
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
                    Location = source.Location,
                    TimeZone = source.TimeZone
                };
            }

            return result;
        }


        public static Metadata.CalendarList Clone(this CalendarListEntry source)
        {
            Metadata.CalendarList result = null;
            if (source != null)
            {
                var gAccessRole = Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum.FreeBusyReader;
                Enum.TryParse(source.AccessRole, out gAccessRole);

                result = new Metadata.CalendarList()
                {
                    Id = source.Id,
                    Summary = source.Summary,
                    Description = source.Description,
                    BackgroundColor = source.BackgroundColor,
                    ForegroundColor = source.ForegroundColor,
                    ColorId = source.ColorId,
                    IsDeleted = source.Deleted ?? false,
                    IsSelected = source.Selected ?? false,
                    IsPrimary = source.Primary ?? false,
                    AccessRole = gAccessRole.Clone()
                };
            }

            return result;
        }


        public static CalendarListEntry Clone(this Metadata.CalendarList source)
        {
            CalendarListEntry result = null;

            if (source != null)
            {
                result = new CalendarListEntry()
                {
                    Id = source.Id,
                    Summary = source.Summary,
                    Description = source.Description,
                    BackgroundColor = source.BackgroundColor,
                    ForegroundColor = source.ForegroundColor,
                    ColorId = source.ColorId,
                    Deleted = source.IsDeleted,
                    Selected = source.IsSelected,
                    Primary = source.IsPrimary,
                    AccessRole = source.AccessRole.Clone().ToString()
                };
            }

            return result;
        }






        public static Access Clone(this Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum source)
        {
            Access result = Access.Read;

            if (source == CalendarListResource.ListRequest.MinAccessRoleEnum.Writer)
            {
                result = Access.Write;
            }

            return result;
        }


        public static Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum Clone(this Access source)
        {
            Google.Apis.Calendar.v3.CalendarListResource.ListRequest.MinAccessRoleEnum result = CalendarListResource.ListRequest.MinAccessRoleEnum.FreeBusyReader;

            if (source == Access.Write)
            {
                result = CalendarListResource.ListRequest.MinAccessRoleEnum.Writer;
            }

            return result;
        }




    }
}
