using Microsoft.Extensions.Primitives;
using Nest;

namespace ACCMSElasticSearchAPI
{
    public static class ElasticsearchExtensions 
    {
       

        public static  string Search(IConfiguration configuration) 
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

           

            var client = new ElasticClient(settings);
            var searchResponse = client.Search<DocketSearch>(s => s
                    .StoredFields(sf => sf
                    .Fields(
                         f => f.case_no,
                         f => f.status,
                         f => f.note_content
                     )
                 )
                 .Query(q => q
                     .MatchAll()
                 )
             ) ;

            return searchResponse.ToString();
        }


        
    }
}
