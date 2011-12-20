using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Channel9Downloader.Entities;

namespace Channel9Downloader.DataAccess
{
    /// <summary>
    /// This interface contains methods to download strings.
    /// </summary>
    public interface IWebDownloader
    {
        #region Methods

        /// <summary>
        /// Downloads the resource with the specified URI to a local file.
        /// </summary>
        /// <param name="address">The URI from which to download data.</param>
        /// <param name="filename">The name of the local file that is to receive the data.</param>
        void DownloadData(string address, string filename);

        /// <summary>
        /// Downloads a file asynchronously.
        /// </summary>
        /// <param name="address">The address of the resource to download.</param>
        /// <param name="filename">The name of the local file that is to receive the data.</param>
        /// <param name="downloadItem">The download item.</param>
        /// <param name="cancellationToken">The token used for cancelling the operation.</param>
        /// <returns>Returns a Task.</returns>
        Task<object> DownloadFileAsync(
            string address,
            string filename,
            DownloadItem downloadItem,
            CancellationToken cancellationToken);

        /// <summary>
        /// Downloads the requested resource as a String. 
        /// The resource to download is specified as a String containing the URI.
        /// </summary>
        /// <param name="address">The address of the resource to download.</param>
        /// <returns>Returns the downloaded resource as a String.</returns>
        string DownloadString(string address);

        /// <summary>
        /// Downloads the requested resource as XHTML.
        /// The resource to download is specified as a String containing the URI.
        /// </summary>
        /// <param name="address">The address of the resource to download.</param>
        /// <returns>Returns the downloaded resource as a XDocument.</returns>
        XDocument DownloadXHTML(string address);

        #endregion Methods
    }
}