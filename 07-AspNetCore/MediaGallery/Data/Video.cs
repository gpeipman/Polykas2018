namespace MediaGallery.Data
{
    public class Video : MediaFile
    {
        public override string Thumbnail
        {
            get { return FileName; }
            set { }
        }
    }
}
