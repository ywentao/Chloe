﻿
namespace Chloe.Sharding
{
    /// <summary>
    /// 分片相关的配置
    /// </summary>
    public class ShardingOptions
    {
        /// <summary>
        /// 每次查询单个数据库最多可打开的连接数
        /// </summary>
        public int MaxConnectionsPerDataSource { get; set; } = 12;
        /// <summary>
        /// in 条件参数最大个数
        /// </summary>
        public int MaxInItems { get; set; } = 1000;
    }
}
