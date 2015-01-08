using System.Collections.Generic;

namespace MetroAPP.Models.Galerija
{
    public class GalleryPictureItemsAndPage
    {
        public List<GalleryPictureItem> GpItems = new List<GalleryPictureItem>();
        public HashSet<string> Pages;
    }
}