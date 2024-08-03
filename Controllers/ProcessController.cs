using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class ProcessController : ControllerBase
{
    private readonly ProcessService _processService;

    public ProcessController(ProcessService processService)
    {
        _processService = processService;
    }

    [HttpPost("start")]
    public IActionResult StartProcess()
    {
        _processService.StartProcess();
        return Ok();
    }

    [HttpGet("data")]
    public IActionResult GetData()
    {
        var data = _processService.GetData();
        return Ok(data);
    }
}
