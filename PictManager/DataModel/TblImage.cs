//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TblImage
    {
        public int ImageId { get; set; }
        public byte[] ImageData { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> TagId1 { get; set; }
        public Nullable<int> TagId2 { get; set; }
        public Nullable<int> TagId3 { get; set; }
        public Nullable<int> TagId4 { get; set; }
        public Nullable<int> TagId5 { get; set; }
        public Nullable<int> TagId6 { get; set; }
        public Nullable<int> TagId7 { get; set; }
        public Nullable<int> TagId8 { get; set; }
        public Nullable<int> TagId9 { get; set; }
        public Nullable<int> SetId { get; set; }
        public Nullable<int> SetOrder { get; set; }
        public string Description { get; set; }
        public System.DateTime InsertedDateTime { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
    }
}
