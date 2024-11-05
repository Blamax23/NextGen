using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NextGen.Model
{
    public class InstagramPost
    {
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public string Caption { get; set; }
        public string Permalink { get; set; }
        public string MediaType { get; set; }
    }

    public class InstagramProfile
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("followers_count")]
        public int FollowersCount { get; set; }

        [JsonPropertyName("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }
        public List<InstagramPost> RecentPosts { get; set; }
    }

    public class InstagramMediaResponse
    {
        [JsonPropertyName("data")]
        public List<InstagramMedia> Data { get; set; }
    }

    public class InstagramMedia
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }
    }
}
