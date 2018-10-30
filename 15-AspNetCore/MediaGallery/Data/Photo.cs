namespace MediaGallery.Data
{
    public class Photo : MediaFile
    {
        public override string Thumbnail
        {
            get { return FileName; }
            set { }
        }
    }
}
