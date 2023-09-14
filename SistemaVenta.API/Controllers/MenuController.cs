using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuServicio;
        public MenuController(IMenuService menuServicio)
        {
            _menuServicio = menuServicio;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int id)
        {
            var rsp = new Response<List<MenuDTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _menuServicio.Lista(id);
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

