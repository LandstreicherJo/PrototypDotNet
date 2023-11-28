using Microsoft.AspNetCore.Mvc;
using PrototypDotNet.Dtos;
using PrototypDotNet.Entities;
using PrototypDotNet.Services;

namespace PrototypDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasuredDataEntryController : ControllerBase
    {
        private readonly ILogger<MeasuredDataEntryController> _logger;
        private readonly MeasuredDataService _measuredDataService;

        public MeasuredDataEntryController(ILogger<MeasuredDataEntryController> logger, MeasuredDataService measuredDataService)
        {
            _logger = logger;
            _measuredDataService = measuredDataService;
        }

        [HttpPost("Create")]
        public async Task<HttpResponseMessage> Create(MeasuredDataEntryDto e)
        {
            return await _measuredDataService.CreateEntryAsync(e);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<MeasuredDataEntry>?> ReadAll()
        {
            return await _measuredDataService.ReadEntriesAsync();
        }

        [HttpGet("Get/{guid}")]
        public async Task<MeasuredDataEntry?> Read(Guid guid)
        {
            return await _measuredDataService.FindByIdAsync(guid);
        }

        [HttpPut("Update")]
        public async Task<HttpResponseMessage> Update(MeasuredDataEntry e)
        {
            return await _measuredDataService.UpdateEntryAsync(e);
        }

        [HttpDelete("Delete/{guid}")]
        public async Task<HttpResponseMessage> Delete(Guid guid)
        {
            return await _measuredDataService.DeleteEntryAsync(guid);
        }



        [HttpGet("ReadByTimespan/{timespan}")]
        public async Task<IEnumerable<MeasuredDataEntry>?> ReadByTimespan(string timespan)
        {
            return await _measuredDataService.ReadByTimespan(timespan);
        }

        [HttpGet("AvgTempByTimespan/{timespan}")]
        public async Task<double> AvgTempByTimespan(string timespan)
        {
            IEnumerable<MeasuredDataEntry>? matchedEntries = await _measuredDataService.ReadByTimespan(timespan);
            return (matchedEntries?.Count() == 0) ? 0 : _measuredDataService.CalculateAverage(matchedEntries);
        }
    }
}