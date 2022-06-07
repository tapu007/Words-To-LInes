using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using WordToLineApp.RequestModels;
using WordToLineApp.ResponseModels;

namespace WordToLineApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsToLinesController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(WordsRequest words)
        {
            try
            {

                var pageSize = Convert.ToInt32(Request.Headers.Where(e => e.Key.Equals("page-size")).FirstOrDefault().Value);
                StringBuilder stringBuilder = new();
                List<string> lines = new();

                string line = "";

                int itemcount = 0;

                foreach (var item in words.Words)
                {
                    itemcount++;
                    int indexNo = words.Words.Count;
                    int lineLength = line.Trim().Length + item.Trim().Length + 1;
                    if (!string.IsNullOrEmpty(item) && lineLength <= pageSize)
                    {
                        line = (line.Trim() + " " + item).Trim();
                    }
                    else if (string.IsNullOrEmpty(line))
                    {
                        line = item;

                        if (line.Length <= pageSize)
                        {

                            lines.Add(line);
                        }
                        line = "";
                    }
                    else
                    {
                        if (line.Length <= pageSize)
                        {
                            lines.Add(line);
                            line = item;
                        }
                    }

                    if (itemcount == indexNo && line.Length <= pageSize)
                    {
                        lines.Add(line);
                        line = "";
                    }
                }
                var data = JsonSerializer.Serialize(lines);
                HttpContext.Session.SetString("lines", data);
                return Ok(StatusCode(201));
            }
            catch (Exception ex)
            {

                return Ok(StatusCode(400));

            }
            
        }

        [HttpGet]
        public LinesResponse Get()
        {
            LinesResponse response = new();
            response.StatusCode = 200;
            try
            {

                var getdata = HttpContext.Session.GetString("lines");
                var data = JsonSerializer.Deserialize<List<string>>(getdata);

                response.Lines = data;
                response.StatusCode = 200;

                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 404;
                return response;
            }

        }
    }
}
