using Microsoft.AspNetCore.Mvc;
using BezierCsharp.Core;

namespace BezierCsharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlotController : ControllerBase
    {
        public PlotController()
        {

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

        [HttpGet("get-total-frames")]
        public IActionResult GetFrameCount()
        {
            try
            {
                return Ok(LatexCache.GetFrameCount());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("get-frame-size")]
        public IActionResult GetFrameSize()
        {
            try
            {
                return Ok(new
                {
                    Width = ImagesProcesor.ImageWidth,  
                    Height = ImagesProcesor.ImageHeight
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
            System.IO.File.WriteAllBytes(ConsoleVariable.outputDir + $"/frame{model.Frame:D4}.png", bytes);
            return Ok();
        }

        public class FrameData
        {
            public int Frame { get; set; }
            public string Data { get; set; } = "";
        }
    }
}
