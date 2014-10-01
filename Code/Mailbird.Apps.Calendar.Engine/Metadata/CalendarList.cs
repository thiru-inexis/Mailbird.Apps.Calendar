using Mailbird.Apps.Calendar.Engine.Enums;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Represents the visual elements for a calendar
    /// </summary>
    public class CalendarList : LocalStorageData
    {
        #region Properties

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public string Kind { get; set; } 

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public string Etag { get; set; } 

        /// <summary>
        /// An override Title/Name of the calendar 
        /// </summary>
        public string SummaryOverride { get; set; } 

        /// <summary>
        /// A Google Calender specific propertry.
        /// Google calendar provides and interface to choose a color 
        /// from a pallet of 24 default {Backgorund, Foreground} set.
        /// </summary>
        public string ColorId { get; set; } // 0 -24
        
        /// <summary>
        /// Background and Foregound of calendar 
        /// </summary>
        public ColorDefinition Color { get; set; } 
       
        /// <summary>
        /// User's role on the calendar
        /// </summary>
        public AccessRole AccessRole { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// Calendar's visiblity.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// If True, calendar related appointments will be visible on the view.
        /// Else the calendar related appointments will not be visible.
        /// </summary>
        public bool IsSelected { get; set; } 

        /// <summary>
        /// A Google Calender specific propertry.
        /// A provider will have a primary calendar.
        /// [Readonly]
        /// </summary>
        public readonly bool IsPrimary;

        /// <summary>
        /// A Google Calender specific propertry.
        /// When calendar's are fetched via sync mechanism, this property denotes
        /// a calendar is deleted or not.
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion



        #region Constructor(s)

        /// <summary>
        /// Create an instance with default values
        /// </summary>
        public CalendarList()
            : this(false)
        {}


        /// <summary>
        /// Creates an instance with default values
        /// </summary>
        /// <param name="isPrimary"></param>
        public CalendarList(bool isPrimary)
            :base()
        {
            this.AccessRole = Enums.AccessRole.FreeBusyReader;
            this.Color = new ColorDefinition();
            this.IsDeleted = this.IsHidden = false;
            this.IsSelected = true;
            this.IsPrimary = isPrimary;
        }

        #endregion
    }
}