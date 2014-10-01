using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Google calender uses tokens for incremental sync.
    /// This entry stores sync tokens to be used for next sync
    /// </summary>
    public class SyncToken : LocalStorageData
    {
        #region Properties

        /// <summary>
        /// Token Type
        /// [Required]
        /// </summary>
        public SyncTokenType TokenType { get; set; }

        /// <summary>
        /// Token Entity's parent object id.
        /// Some entities are depended on other entities [Eg : Event belongs to a calendar, Calendar belongs to a user]
        /// Hence a sync token is differentiated by it parentId.
        /// [Nullable]
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// The actually token used for sync
        /// [Required]
        /// </summary>
        public string Token { get; set; }

        #endregion



        #region Constructor(s)

        /// <summary>
        /// Creates a synctoken with defautl values
        /// </summary>
        /// <param name="tokenType"></param>
        public SyncToken(SyncTokenType tokenType)
            : base()
        {
            this.TokenType = tokenType;
            this.ParentId = null;
            this.Token = null;
        }

        #endregion


    }

}
