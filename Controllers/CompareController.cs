using JSONCompareApi.Api.Models;
using JSONCompareApi.Domain.Enums;

using JSONCompareApi.Models;
using JSONCompareApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace JSONCompareApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompareController : ControllerBase
    {

        CacheService _cacheService = new CacheService();
        private readonly ILogger<CompareController> _logger;
        public CompareController(ILogger<CompareController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("{id}/left")]
        public async Task<IActionResult> PostLeftOperation(string id, [FromBody] DiffDataRequest jsonData)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(80.0);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(jsonData.Data))
                return StatusCode(400, new DiffDataResponse() { Message = "Required data not found" });
            JsonBase64Item entity = new JsonBase64Item() { Id = id, Position = Domain.Enums.EJsonBase64Position.Left.ToString(), Data = jsonData.Data };

            try
            {
                bool success = _cacheService.SetData("left", entity, expirationTime);
                if (success)
                    return StatusCode(201, new DiffDataResponse() { Message = "OK" });
                else
                    return StatusCode(400, new DiffDataResponse() { Message = "Error when input data" });
            }
            catch (Exception)
            {
                //write log error
                return StatusCode(400, new DiffDataResponse() { Message = "Error when input data" });
            }

        }

		[HttpPost]
        [Route("{id}/right")]
        public async Task<IActionResult> PostRightOperation(string id, [FromBody] DiffDataRequest jsonData)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(80.0);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(jsonData.Data))
                return StatusCode(400, new DiffDataResponse() { Message = "Required data not found" });
            JsonBase64Item entity = new JsonBase64Item() { Id = id, Position = Domain.Enums.EJsonBase64Position.Right.ToString(), Data = jsonData.Data };
            try
            {
                bool success = _cacheService.SetData("right", entity, expirationTime); ;

                if (success)
                    return StatusCode(201, new DiffDataResponse() { Message = "OK" });
                else
                    return StatusCode(400, new DiffDataResponse() { Message = "Error when input data" });
            }
            catch (Exception)
            {
                //write log error
                return StatusCode(400, new DiffDataResponse() { Message = "Error when input data" });
            }
        }
		[HttpGet]
        [Route("{id}/Difference")]
        public async Task<JsonDiff> Diff(string id)
        {

            List<JsonBase64Item> dictCollection = new List<JsonBase64Item>();
            ObjectCache allCacheInfo = _cacheService.GetAllData();
            if (allCacheInfo != null)
            {
                foreach (var item in allCacheInfo)
                {
                    string pos = ((JSONCompareApi.Models.JsonBase64Item)item.Value).Position;
                    JsonBase64Item collection = new JsonBase64Item();
                    collection.Id = ((JSONCompareApi.Models.JsonBase64Item)item.Value).Id;
                    collection.Position = pos;
                    collection.Data = ((JSONCompareApi.Models.JsonBase64Item)item.Value).Data;
                    dictCollection.Add(collection);

                    Console.WriteLine("cache object key-value: " + item.Key + "-" + item.Value);
                }
            }

            JsonBase64Item itemLeft = dictCollection.Where(row => row.Id == id && row.Position == Domain.Enums.EJsonBase64Position.Left.ToString()).FirstOrDefault();
            JsonBase64Item itemRight = dictCollection.Where(row => row.Id == id && row.Position == Domain.Enums.EJsonBase64Position.Right.ToString()).FirstOrDefault();

            JsonDiff result = new JsonDiff();

            byte[] leftArray = Convert.FromBase64String(itemLeft.Data);
            byte[] right = Convert.FromBase64String(itemRight.Data);
            List<int> offsetList = new List<int>();

            var base64EncodedBytes = Convert.FromBase64String(itemLeft.Data);            
            var inputString = Encoding.UTF8.GetString(base64EncodedBytes);
            var base64EncodedBytesRight = Convert.FromBase64String(itemRight.Data);            
            var inputStringRight = Encoding.UTF8.GetString(base64EncodedBytesRight);
            if (itemLeft == null || itemRight == null)
            {
                result.Message = "Data not found. Send both sides again.";
                return result;
            }

            if (itemLeft.Data.Length != itemRight.Data.Length)
            {
                result.Message = "inputs are of different size";
                return result;
            }

            result.Message = "inputs were equal";
            for (int i = 0; i < leftArray.Length; i++)
            {
                if (leftArray[i] != right[i])
                {
                    offsetList.Add(i);
                }
            }
            result.Message = result.Message + ", OffsetLength: " + offsetList.Count;
            return result;
        }

    }
}