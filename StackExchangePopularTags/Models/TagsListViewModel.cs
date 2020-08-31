using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StackExchangePopularTags.Models
{
    public class TagsListViewModel
    {

        [JsonProperty("items")]
        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public void SetPopularity()
        {
            var countSum = 0;

            foreach (var tag in Tags)
            {
                countSum += tag.Count;
            }

            foreach (var tag in Tags)
            {
                tag.Popularity = (Convert.ToDecimal(tag.Count) / countSum * 100);
                
            }
        }

    }
}
