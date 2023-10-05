using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DTO;
using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.Model;
using SistemaVenta.DAL.Repositorios.Contratos;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;
        private readonly IVentaRepository _ventaRepositorio;

        public DashBoardService(IGenericRepository<Producto> productoRepositorio, IMapper mapper, IVentaRepository ventaRepositorio)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
            _ventaRepositorio = ventaRepositorio;
        }

        private IQueryable<Venta> RetornarVenta(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            //obtiene ultima fecha registrada
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            //de la fecha de la ulitma fecha le resta días
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);
            //Retorna la consulta de ventas que corresponden a la ulitma menos los dias restados.
            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }
        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery, -7); // retorna las ventas de la ultima semana
                total = tablaVenta.Count(); //cantidad de esa última semana
            }
            return total;
        }
        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery, -7);

                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-AR"));
        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;
        }
        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery, -7);
                resultado = tablaVenta
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key).Select(
                    dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }).ToDictionary(
                    keySelector: r => r.fecha, elementSelector: r => r.total);
            }
            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();
            try
            {
                vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await TotalProductos();
                List<VentaSemanaDTO> listaVentaSemana = new List<VentaSemanaDTO>();
                foreach (KeyValuePair<string, int> item in await VentasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value,
                    });
                }
                vmDashBoard.VentasUltimaSemana = listaVentaSemana;
                return vmDashBoard;
            }
            catch
            {
                throw;
            }
        }
    }
}
