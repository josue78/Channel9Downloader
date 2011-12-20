using System;

using Channel9Downloader.DataAccess.Events;
using Channel9Downloader.Entities;

namespace Channel9Downloader.DataAccess
{
    /// <summary>
    /// This interface provides methods for downloading videos.
    /// </summary>
    public interface IDownloadManager
    {
        #region Events

        /// <summary>
        /// This event is raised when a new download has been added.
        /// </summary>
        event EventHandler<DownloadAddedEventArgs> DownloadAdded;

        /// <summary>
        /// This event is raised when downloading has started.
        /// </summary>
        event EventHandler<EventArgs> DownloadingStarted;

        /// <summary>
        /// This event is raised when downloading has stopped.
        /// </summary>
        event EventHandler<EventArgs> DownloadingStopped;

        #endregion Events

        #region Methods

        /// <summary>
        /// Initializes the class.
        /// </summary>
        /// <param name="settings">The application settings.</param>
        void Initialize(Settings settings);

        /// <summary>
        /// Starts downloads.
        /// </summary>
        void StartDownloads();

        /// <summary>
        /// Stops downloads.
        /// </summary>
        void StopDownloads();

        /// <summary>
        /// Updates all available downloads.
        /// </summary>
        void UpdateAvailableDownloads();

        #endregion Methods
    }
}