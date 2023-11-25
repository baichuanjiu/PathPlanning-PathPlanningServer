using Microsoft.AspNetCore.Mvc;
using PathPlanning.Tools.RoadNetworkConstruction;

namespace PathPlanning.Controllers
{
    [ApiController]
    [Route("/roadNetworkConstruction")]
    public class RoadNetworkConstructionController : ControllerBase
    {
        [HttpPost]
        public IActionResult RoadExtraction([FromBody] string base64)
        {
            string fileExtension = base64.Substring(base64.IndexOf("/")+1, base64.IndexOf(";")- base64.IndexOf("/") - 1);
            base64 = base64.Substring(base64.IndexOf(",") + 1);
            byte[] bytes = Convert.FromBase64String(base64);
            string imgName = DateTime.Now.ToFileTime().ToString()+"."+fileExtension;
            string imgPath = @"./Images/RoadNetworkConstruction/" + imgName;
            FileStream fs = new FileStream(imgPath, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(bytes);
            fs.Close();
            return Ok(RoadNetwork.Construct(imgName));
        }
    }
}
