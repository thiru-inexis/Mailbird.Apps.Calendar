using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Represents a color definition of Calendars and Appointments 
    /// </summary>
    public class ColorDefinition: LocalStorageData
    {

        #region Contants

        public const string DEFAULT_BACKGROUND = "#000000";
        public const string DEFAULT_FOREGROUND = "#FFFFFF";

        #endregion


        #region Properties

        /// <summary>
        /// Backgound fill color in Hex format 
        /// </summary>
        /// <example>#990000</example>
        public string Background { get; set; }

        /// <summary>
        /// Foreground/Text fill color in Hex format
        /// </summary>
        /// <example>#990000</example>
        public string Foreground { get; set; }

        #endregion



        #region Contructor(s)

        /// <summary>
        /// Creates a Instance with default values
        /// </summary>
        public ColorDefinition()
            :base()
        {
            this.Background = DEFAULT_BACKGROUND;
            this.Foreground = DEFAULT_FOREGROUND;
        }
        
        #endregion
    }
}
