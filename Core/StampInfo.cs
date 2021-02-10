using System;
using System.Collections.Generic;

namespace pdfsvc.Core
{
    public class StampInfo
    {
        // 盖章日期，可为null
        public long SignDate { get; set; }
        // 盖章理由，可为null
        public string Reason { get; set; }
        // 盖章位置，可为null
        public string Location { get; set; }
        // 盖章人，可为null
        public string Creator { get; set; }
        // 盖章人联系方式，可为null
        public string Contact { get; set; }
        // 盖章人联系方式，可为null
        public List<Int32> Pages { get; set; }
        // 盖章关键字查找正则表达式
        public string Regex { get; set; }
        // 是否最后一个关键字处盖章，TODO 暂不支持一次盖多个章
        public bool Latest { get; set; }
    }
}
