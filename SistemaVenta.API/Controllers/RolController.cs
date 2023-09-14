using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolServicio;
        public RolController(IRolService rolServicio)
        {
            _rolServicio = rolServicio;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<RolDTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _rolServicio.Lista();
            }
            catch (Exception ex) 
            { 
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
