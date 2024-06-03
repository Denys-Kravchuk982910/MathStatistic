using ChartServer.Data;
using ChartServer.Data.Entities;
using ChartServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChartServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private EFContext _context;

        public DataController(EFContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("set")]
        public IActionResult SetData([FromBody] GraphicEnvelope graphic)
        {
            List<GraphicType> graphics = graphic.graphics;
                //JsonConvert.DeserializeObject<List<GraphicType>>(graphicsContent);

            if (this._context.Records.Any())
            {
                StatRecord[] statRecord = this._context.Records.ToArray();

                for (int i = 0; i < graphics.Count; i++)
                {
                    StatRecord? record = statRecord[i];

                    if (record != null)
                    {
                        record.Middle = graphics[i].Middle;
                        record.Count = (int)graphics[i].Count;
                        
                        this._context.Records.Update(record);
                    }
                }
            } 
            else
            {
                for (int i = 0; i < graphics.Count; i++)
                {
                    StatRecord record = new StatRecord
                    {
                        Middle = graphics[i].Middle,
                        Count = (int)graphics[i].Count
                    };

                    this._context.Add(record);
                }
            }

            this._context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetData()
        {
            return Ok(this._context.Records.OrderBy(x => x.Middle));
        }
    }
}
