namespace MetroAPP.Models.Naslovnica
{
    public class NaslovnicaSliderItem
    {
        public int SliderListItemId;
        public string SliderListItemImgUrl;
        public string SliderLitsItemNaslov;
        public string SliderListItemText;
        public string SliderListItemActionLink;
        public string SliderListItemActive;

        public NaslovnicaSliderItem(int id,string imgUrl,string naslov,string text,string actionLink, string active)
        {
            SliderListItemId = id;
            SliderListItemImgUrl = imgUrl;
            SliderLitsItemNaslov = naslov;
            SliderListItemText = text;
            SliderListItemActionLink = actionLink;
            SliderListItemActive = active;
        }
    }
}