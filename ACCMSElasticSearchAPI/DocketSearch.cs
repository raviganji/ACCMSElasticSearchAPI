namespace ACCMSElasticSearchAPI
{
    public class DocketSearch
    {
        public string? case_no { get; set; }
        public string? cause { get; set; }
        public string? status { get; set; }
        public string?  case_id { get; set; }
        public string? create_date { get; set; }
        public string? note_content { get; set; }
        public string? code_name { get; set; }

        public string CreateDate
        {
            get
            {
                if (this.create_date != null) {
                    return (DateTime.Parse(this.create_date).ToShortDateString());
                }
                else
                {
                    return "";
                }
                ;
            } }
    }
}
