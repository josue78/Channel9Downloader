﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using Channel9Downloader.Entities;

namespace Channel9Downloader.DataAccess
{
    /// <summary>
    /// This class is used to browse channel 9 categories.
    /// </summary>
    [Export(typeof(IChannel9CategoryBrowser))]
    public class Channel9CategoryBrowser : IChannel9CategoryBrowser
    {
        #region Fields

        /// <summary>
        /// The web downloader used for retrieving web resources.
        /// </summary>
        private readonly IWebDownloader _webDownloader;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel9CategoryBrowser"/> class.
        /// </summary>
        /// <param name="webDownloader">The web downloader used for retrieving web resources.</param>
        [ImportingConstructor]
        public Channel9CategoryBrowser(IWebDownloader webDownloader)
        {
            _webDownloader = webDownloader;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets a list of all available series.
        /// </summary>
        /// <returns>Returns a list of all available series.</returns>
        public List<Series> GetAllSeries()
        {
            var recurringCategories = RetrieveShowsOrSeries("http://channel9.msdn.com/Browse/Series?sort=atoz&page={0}");
            return recurringCategories.Select(recurringCategory => new Series(recurringCategory)).ToList();
        }

        /// <summary>
        /// Gets a list of all available shows.
        /// </summary>
        /// <returns>Returns a list of all available shows.</returns>
        public List<Show> GetAllShows()
        {
            var recurringCategories = RetrieveShowsOrSeries("http://channel9.msdn.com/Browse/Shows?sort=atoz&page={0}");
            return recurringCategories.Select(recurringCategory => new Show(recurringCategory)).ToList();
        }

        /// <summary>
        /// Gets a list of all available tags.
        /// </summary>
        /// <returns>Returns a list of all available tags.</returns>
        public List<Tag> GetAllTags()
        {
            return RetrieveTags();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates an XML document from a specific URL.
        /// </summary>
        /// <param name="url">The URL to retrieve.</param>
        /// <returns>Returns an XML document.</returns>
        private XDocument GetDocument(string url)
        {
            var data = _webDownloader.DownloadString(url);
            data = data.Remove(0, 17);
            data = data.Replace("&", string.Empty);
            return XDocument.Parse(data);
        }

        /// <summary>
        /// Retrieves all shows or series from a specific page number.
        /// </summary>
        /// <param name="baseUrl">The base URL used for retrieving shows or series.</param>
        /// <param name="pageNumber">The number of the page to retrieve.</param>
        /// <returns>Returns a list of recurring categories from a specific page.</returns>
        private List<RecurringCategory> GetShowsOrSeriesFromPage(string baseUrl, int pageNumber)
        {
            var url = string.Format(baseUrl, pageNumber);
            var document = GetDocument(url);
            var tabContent = GetTabContent(document);

            var entries = from item in tabContent.Descendants("div")
                          where item.Attribute("class") != null && item.Attribute("class").Value == "entry-meta"
                          select item;

            return (from entry in entries
                    let title = entry.Element("a").Value
                    let relativePath = entry.Element("a").Attribute("href").Value
                    let description =
                        entry.Elements("div").Where(p => p.Attribute("class").Value == "description").FirstOrDefault().Value.Trim()
                    select
                        new RecurringCategory { Description = description, RelativePath = relativePath, Title = title })
                .ToList();
        }

        /// <summary>
        /// Gets the div element which has class tab-content. This is essentially where all the content is stored
        /// in the HTML page.
        /// </summary>
        /// <param name="document">The document to parse.</param>
        /// <returns>Returns the content div element.</returns>
        private XElement GetTabContent(XDocument document)
        {
            var bodyElement = document.Element("html").Element("body");

            var tabContentDiv = from item in bodyElement.Descendants("div")
                                where item.Attribute("class") != null && item.Attribute("class").Value == "tab-content"
                                select item;

            return tabContentDiv.ElementAt(0);
        }

        /// <summary>
        /// Gets all tags for a specific character.
        /// </summary>
        /// <param name="character">The character for which tags should be retrieved.</param>
        /// <returns>Returns a list of tags for a specific character.</returns>
        private IEnumerable<Tag> GetTagsForCharacter(char character)
        {
            var url = string.Format("http://channel9.msdn.com/Browse/Tags/FirstLetter/{0}#{0}", character);
            var document = GetDocument(url);
            var tabContent = GetTabContent(document);

            var lists = from item in tabContent.Descendants("ul")
                        where item.Attribute("class") != null && item.Attribute("class").Value == "tagList default"
                        select item.Descendants("a");

            if (lists.Count() < 1)
            {
                return new List<Tag>();
            }

            var list = lists.ElementAt(0);

            return from item in list select new Tag { RelativePath = item.Attribute("href").Value, Title = item.Value };
        }

        /// <summary>
        /// Retrieves shows or series. Shows and series are shown on paginated websites and differ only in
        /// the URL that is used to show them.
        /// </summary>
        /// <param name="baseUrl">The base URL used for retrieving shows or series.</param>
        /// <returns>Returns a list of recurring categories.</returns>
        private IEnumerable<RecurringCategory> RetrieveShowsOrSeries(string baseUrl)
        {
            var result = new List<RecurringCategory>();

            int i = 0;
            List<RecurringCategory> shows;
            while ((shows = GetShowsOrSeriesFromPage(baseUrl, i++)) != null && shows.Count > 0)
            {
                result.AddRange(shows);
            }

            return result;
        }

        /// <summary>
        /// Retrieves tags for all characters A to Z.
        /// </summary>
        /// <returns>Returns a list of all tags.</returns>
        private List<Tag> RetrieveTags()
        {
            var result = new List<Tag>();

            for (var c = (char)65; c <= 90; c++)
            {
                var links = GetTagsForCharacter(c);
                result.AddRange(links);
            }

            return result;
        }

        #endregion Private Methods
    }
}