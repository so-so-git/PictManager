﻿//------------------------------------------------------------------------------
// <auto-generated>
//    このコードはテンプレートから生成されました。
//
//    このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//    このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SO.PictManager.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PictManagerEntities : DbContext
    {
        public PictManagerEntities()
            : base("name=PictManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<MstCategory> MstCategories { get; set; }
        public DbSet<MstTag> MstTags { get; set; }
        public DbSet<TblGroup> TblGroups { get; set; }
        public DbSet<TblImage> TblImages { get; set; }
        public DbSet<TblTagging> TblTaggings { get; set; }
    }
}
