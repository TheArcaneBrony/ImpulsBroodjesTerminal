using System;
using System.Collections.Generic;

#nullable disable

namespace ThumbnailGenerator
{
    public partial class Broodje
    {
        public int BroodjeId { get; set; }
        public string BroodjeName { get; set; }
        public string BroodjeType { get; set; }
        public string BroodjeIngredients { get; set; }
        public float BroodjePrice { get; set; }
        public string BroodjeImage { get; set; }
        public string BroodjeImageThumbnail128 { get; set; }
        public string BroodjeImageThumbnail1024 { get; set; }
    }
}
