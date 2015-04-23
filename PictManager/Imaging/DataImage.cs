using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SO.PictManager.DataModel;

namespace SO.PictManager.Imaging
{
    public class DataImage : IImage
    {
        private int _imageId;

        public string Key
        {
            get { return _imageId.ToString(); }
            set { _imageId = int.Parse(value); }
        }

        public DateTime Timestamp
        {
            get { throw new NotImplementedException(); }
        }

        public long DataSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDeleted
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DataImage(int imageId)
        {
            _imageId = imageId;
        }

        public Image GetImage()
        {
            using (var entity = new PictManagerEntities())
            {
                byte[] imageData = (from i in entity.TblImages
                                    where i.ImageId == _imageId
                                    select i.ImageData).FirstOrDefault();

                if (imageData == null)
                {
                    throw new ApplicationException("指定されたIDの画像が存在しません。");
                }

                var converter = new ImageConverter();
                var img = converter.ConvertFrom(imageData) as Image;

                return img;
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
