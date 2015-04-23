using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace SO.PictManager.DataModel
{
    /// <summary>
    /// PictManager用DbConfigurationクラス
    /// </summary>
    public class PictManagerDbConfiguration : DbConfiguration
    {
        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public PictManagerDbConfiguration()
        {
            // リトライ設定
            int maxRetryCount = int.Parse(ConfigurationManager.AppSettings["SqlConnectionMaxRetryCount"]);
            var maxDelay = new TimeSpan(0, 0, 0, 0,
                int.Parse(ConfigurationManager.AppSettings["SqlConnectionMaxDelay"]));

            SetExecutionStrategy("System.Data.SqlClient",
                () => new PictManagerDbExecutionStrategy(10, maxDelay));
        }
    }

    /// <summary>
    /// PictManager用DbExecutionStrategyクラス
    /// </summary>
    public class PictManagerDbExecutionStrategy : DbExecutionStrategy
    {
        /// <summary>
        /// デフォルトのコンストラクタです。
        /// リトライ最大回数：5回、リトライの最大ディレイ：26秒 に設定されます。
        /// </summary>
        public PictManagerDbExecutionStrategy() { }

        /// <summary>
        /// リトライ最大回数、リトライの最大ディレイを指定するコンストラクタです。
        /// </summary>
        /// <param name="maxRetryCount">リトライ最大回数</param>
        /// <param name="maxDelay">リトライの最大ディレイ(単位：ミリ秒)</param>
        public PictManagerDbExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay) { }

        /// <summary>
        /// リトライを行うかを判定する処理です。
        /// リトライの必要性が発生した原因を問わず、常にリトライを行います。
        /// </summary>
        /// <param name="ex">リトライの必要性が発生した原因の例外</param>
        /// <returns>常にtrue(リトライを行う)</returns>
        protected override bool ShouldRetryOn(Exception ex)
        {
            return true;
        }
    }
}
