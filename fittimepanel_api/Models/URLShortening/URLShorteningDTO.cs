using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.URLShortening
{
    public class URLShorteningErrorDTO : IURLShorteningResponseDTO
    {
        public bool Success { get; set; }
        public KeyValuePair<string, string>? Errors { get; set; }
    }

    public class URLShorteningResponseDTO : IURLShorteningResponseDTO
    {
        public bool Success { get; set; }
        public URLShorteningEntityDTO Doc { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }

    public class URLShorteningEntityDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Url { get; set; }
        public string StatsUrl { get; set; }
        public string UpdateUrl { get; set; }
        public EntityItem Entity { get; set; }
        public class EntityItem
        {
            public string Url { get; set; }
            public IList<DirectedUrlsItem> DirectedUrls { get; set; }
            public bool Enable { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int RedirectStatusCode { get; set; }
            public DateTime ActivationDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string Slug { get; set; }
            public string Password { get; set; }
            public bool Featured { get; set; }
            public int Hits { get; set; }
            public class DirectedUrlsItem
            {
                public string Platform { get; set; }
                public string OperatingSystem { get; set; }
                public string Location { get; set; }
                public string Url { get; set; }
            }
        }
    }

    public class URLShorteningRequestDTO
    {
        public string Url { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

}
