namespace MetroAPP.Models.Galerija
{
    public class GalleryPictureItem
    {
        public int PictureId;
        public string PictureSrc;
        public string PicturePage;

        public GalleryPictureItem(string pictureSrc, string picturePage, int pictureId)
        {
            PictureSrc = pictureSrc;
            PicturePage = picturePage;
            PictureId = pictureId;
        }
    }
}