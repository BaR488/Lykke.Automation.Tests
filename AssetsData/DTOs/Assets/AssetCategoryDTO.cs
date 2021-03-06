﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetCategoryDTO : BaseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IosIconUrl { get; set; }
        public string AndroidIconUrl { get; set; }
        public int? SortOrder { get; set; }
    }
}
