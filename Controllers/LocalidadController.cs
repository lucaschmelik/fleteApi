using fleteApi.Context;
using Microsoft.AspNetCore.Mvc;


namespace fleteApi.Controllers
{
    [Route("api/localidades")]
    public class LocalidadController : ControllerBase
    {
        private readonly FleteContext _context;

        public LocalidadController(FleteContext oContext)
        {
            _context = oContext;
        }

        [HttpGet]
        public ActionResult GetLocalidades()
        {
            try
            {
                return Ok(_context.Localidades.OrderBy(x => x.Nombre).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
