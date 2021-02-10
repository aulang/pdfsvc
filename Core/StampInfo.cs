using System;
using System.Collections.Generic;

namespace pdfsvc.Core
{
    public class StampInfo
    {
        public StampInfo()
        {

        }

        public StampInfo(string regex, List<int> pages, bool latest = true)
        {
            if (regex != null && regex == "")
            {
                Regex = regex;
            }

            if (pages == null || pages.Count == 0)
            {
                pages.Add(-1);
            }
            else
            {
                Pages.AddRange(pages);
            }

            Latest = latest;
        }

        // 盖章日期，可为null
        public long SignDate { get; set; } = 0;
        // 盖章理由，可为null
        public string Reason { get; set; } = "";
        // 盖章位置，可为null
        public string Location { get; set; } = "";
        // 盖章人，可为null
        public string Creator { get; set; } = "";
        // 盖章人联系方式，可为null
        public string Contact { get; set; } = "";
        // 盖章人联系方式，可为null
        public List<int> Pages { get; set; } = new List<int>();
        // 盖章关键字查找正则表达式
        public string Regex { get; set; } = "盖章";
        // 是否最后一个关键字处盖章，TODO 暂不支持一次盖多个章
        public bool Latest { get; set; } = true;
    }
}
