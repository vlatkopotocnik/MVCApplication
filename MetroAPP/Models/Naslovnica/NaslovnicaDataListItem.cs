namespace MetroAPP.Models.Naslovnica
{
    public class NaslovnicaDataListItem
    {
        public NaslovnicaDataListItem(int dataListId, string dataListItemNaslov, string dataListItemText, string dataListImgSrc, string dataListActionLink)
        {
            DataListId = dataListId;
            DataListItemNaslov = dataListItemNaslov;
            DataListItemText = dataListItemText;
            DataListItemImgSrc = dataListImgSrc;
            DataListActionLink = dataListActionLink;
        }

        public int DataListId;
        public string DataListItemNaslov;
        public string DataListItemText;
        public string DataListItemImgSrc;
        public string DataListActionLink;
    }
}