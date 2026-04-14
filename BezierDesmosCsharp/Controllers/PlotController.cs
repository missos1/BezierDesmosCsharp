using Microsoft.AspNetCore.Mvc;
using BezierCsharp.Core;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace BezierCsharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlotController : ControllerBase
    {
        private readonly AppState _appState;
        public PlotController(AppState appState)
        {
            _appState = appState;
        }

        [HttpGet("get-latex/{id}")]
        public IActionResult GetDesmosLatex(int id)
        {
            Console.WriteLine("Received request for LaTeX strings with id: " + id);
            try
            {
                List<string> latexStrings = LatexCache.GetLatexStrings(id);
                //Console.WriteLine(latexStrings);
                return Ok(latexStrings);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("init-data")]
        public IActionResult GetInitData()
        {
            try
            {
                return Ok(new
                {
                    TotalFrames = LatexCache.GetFrameCount(),
                    Width = _appState.ImageWidth,  
                    Height = _appState.ImageHeight,
                    TakeScreenshot = _appState.TakeScreenshot
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("save-frame")]
        public IActionResult SaveFrame([FromBody] FrameData model)
        {
            var base64Data = model.Data.Split(',')[1];
            var bytes = Convert.FromBase64String(base64Data);
            System.IO.File.WriteAllBytes(_appState.OutputDir + $"/frame{model.Frame:D4}.png", bytes);
            return Ok();
        }

        public class FrameData
        {
            public int Frame { get; set; }
            public string Data { get; set; } = "";
        }
    }
}
