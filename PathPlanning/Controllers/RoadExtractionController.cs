using Microsoft.AspNetCore.Mvc;
using PathPlanning.Tools.DLinkNetInference;

namespace PathPlanning.Controllers
{
    [ApiController]
    [Route("/roadExtraction")]
    public class RoadExtractionController : ControllerBase
    {
        [HttpPost]
        public FileStreamResult RoadExtraction(IFormFile file)
        {
            string imgPath = "./Images/Input/" + file.FileName;
            string imgName = file.FileName;
            FileStream fs = new FileStream(imgPath, FileMode.Create, FileAccess.ReadWrite);
            file.CopyTo(fs);
            fs.Close();
            string outputName = ONNXInference.PathRecognition(imgName);
            fs = new FileStream("./Images/Output/" + outputName, FileMode.Open, FileAccess.ReadWrite);
            FileStreamResult result = new FileStreamResult(fs, "image/png");
            return result;
        }
    }
}