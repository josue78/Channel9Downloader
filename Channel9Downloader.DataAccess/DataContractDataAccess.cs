﻿using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Channel9Downloader.DataAccess
{
    /// <summary>
    /// This class serves as base class for data contract serialization.
    /// </summary>
    /// <typeparam name="T">Type that should be serialized.</typeparam>
    public abstract class DataContractDataAccess<T>
    {
        #region Protected Methods

        /// <summary>
        /// Load an object of type T.
        /// </summary>
        /// <param name="filename">File to open.</param>
        /// <returns>Returns the deserialized object of type T.</returns>
        protected T Load(string filename)
        {
            if (!File.Exists(filename))
            {
                return default(T);
            }

            try
            {
                var ds = new DataContractSerializer(typeof(T));
                using (var reader = XmlReader.Create(filename))
                {
                    return (T)ds.ReadObject(reader);
                }
            }
            catch (SerializationException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Save an object of type T.
        /// </summary>
        /// <param name="data">Object to save.</param>
        /// <param name="filename">Filename of the serialized object.</param>
        protected void Save(T data, string filename)
        {
            var ds = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings { Indent = true };

            using (var w = XmlWriter.Create(filename, settings))
            {
                ds.WriteObject(w, data);
            }
        }

        #endregion Protected Methods
    }
}