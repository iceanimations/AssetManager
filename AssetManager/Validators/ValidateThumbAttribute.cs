using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace AssetManager.Validators
{
    public class ValidateThumbAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //return base.IsValid(value);
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file == null)
                return true;
            if (file.ContentLength > 10 * 1024 * 1024)
                return false;
            try
            {
                using (Image img = Image.FromStream(file.InputStream))
                {
                    if (img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg))
                        if (img.Width == img.Height)
                            return img.Width >= 200 && img.Height >= 200;
                }
            }
            catch { }
            return false;
        }
    }
}