using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Topics;
using Nop.Data;

namespace Nop.Services.Topics
{
    /// <summary>
    /// Topic service interface
    /// </summary>
    public partial interface ITopicService
    {
        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="topic">Topic</param>
        void DeleteTopic(Topic topic);

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <returns>Topic</returns>
        Topic GetTopicById(int topicId);

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="systemName">The topic system name</param>
        /// <returns>Topic</returns>
        Topic GetTopicBySystemName(string systemName);

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>Topics</returns>
        IList<Topic> GetAllTopic();

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="topic">Topic</param>
        void InsertTopic(Topic topic);

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="topic">Topic</param>
        void UpdateTopic(Topic topic);
    }
}
