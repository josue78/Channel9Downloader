﻿using System.Runtime.Serialization;

namespace Channel9Downloader.Entities
{
    /// <summary>
    /// This class holds information about media content.
    /// </summary>
    [DataContract]
    public class MediaContent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        [DataMember]
        public int FileSize
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the medium.
        /// </summary>
        [DataMember]
        public string Medium
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        [DataMember]
        public string Type
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [DataMember]
        public string Url
        {
            get; set;
        }

        #endregion Public Properties
    }
}