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
    }
}
