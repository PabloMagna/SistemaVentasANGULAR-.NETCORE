using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;


namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;
        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            return Ok(rsp);
        }
        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string?fechaInicio, string?fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta is null ? "" : numeroVenta;
            fechaFin = fechaFin is null ? "" : fechaFin;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Historial(buscarPor,numeroVenta,fechaInicio,fechaFin);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            return Ok(rsp);
        }
        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<ReporteDTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Reporte(fechaInicio, fechaFin);
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
