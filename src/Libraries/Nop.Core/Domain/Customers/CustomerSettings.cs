﻿
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Customers
{
    public class CustomerSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether anonymous checkout allowed
        /// </summary>
        public bool AnonymousCheckoutAllowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to upload avatars.
        /// </summary>
        public bool AllowUsersToUploadAvatars { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users write product reviews.
        /// </summary>
        public bool AllowAnonymousUsersToReviewProduct { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users to set product ratings.
        /// </summary>
        public bool AllowAnonymousUsersToSetProductRatings { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users to email a friend.
        /// </summary>
        public bool AllowAnonymousUsersToEmailAFriend { get; set; }
    }
}