using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ACCMSElasticSearchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ACCMSElasticSearchController : ControllerBase
    {

        private IConfiguration _configuration;
        private readonly ILogger<ACCMSElasticSearchController> _logger;

        public ACCMSElasticSearchController(ILogger<ACCMSElasticSearchController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
            

        [HttpGet("/search/dockets/{begindate}/{enddate}/{rowcount}/{codename}/{notes}")]
        public IActionResult SearchDockets(DateTime begindate, DateTime enddate,  int rowcount, string codename="", string notes="")
        {
         
            var settings = new ConnectionSettings(new Uri(_configuration["elasticsearch:url"]))
                .DefaultIndex(_configuration["elasticsearch:docketindex"]);
            string codeName = "*";
            string comments = "*";
            int rowCount = 250;

            if (rowcount > 0) rowCount = rowcount;

            if (codename != "") codeName = codename;
            if (notes != "") comments = notes;

            var client = new ElasticClient(settings);
            var searchResponse = client.Search<DocketSearch>(s => s
            .Size(rowCount)
            .Query(q => 
               q.DateRange(r => 
                    r.Field(f => f.create_date)
                        .GreaterThanOrEquals(begindate)
                        .LessThan(enddate)
                    
               ) &&
               q.QueryString(q =>  q.Fields(t => t.Field(nc => nc.note_content))
               .Query(notes)
               .DefaultOperator(Operator.Or)
               )

               &&
                /*
                q.Match( m => m.Field(f => f.code_name)
                .Query(codeName)))  
                */
                q.Wildcard(m => m.Field(p => p.code_name)
               .Value(codename)))
               
                /*&&
                q.Wildcard(m => m.Field(p => p.note_content)
               .Value(notes))
                )*/
             );
            
             return Ok(searchResponse.HitsMetadata.Hits.Select(h => h.Source).ToList());
        }
    }
}