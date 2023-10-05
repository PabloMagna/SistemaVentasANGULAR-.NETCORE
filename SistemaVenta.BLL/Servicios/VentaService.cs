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
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService :IVentaService
    {
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IMapper _mapper;
        private readonly IVentaRepository _ventaRepositorio;

        public VentaService(IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper, IVentaRepository ventaRepository)
        {
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
            _ventaRepositorio = ventaRepository;
        }
        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaRegistrada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(modelo));
                if (ventaRegistrada.IdVenta == 0)
                    throw new TaskCanceledException("La venta no se pudo crear");
                return  _mapper.Map<VentaDTO>(ventaRegistrada);
            }
            catch 
            {
                throw;
            }
        }


        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var listaResultado = new List<Venta>();
            try
            {
                if(buscarPor == "fecha")
                {
                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    listaResultado = await query.Where(q =>
                           q.FechaRegistro.Value.Date >= fech_inicio.Date &&
                           q.FechaRegistro.Value.Date <= fech_fin.Date
                        ).Include(dv => dv.DetalleVenta).ThenInclude(
                            p => p.IdProductoNavigation
                        ).ToListAsync();
                }
                else
                {
                    listaResultado = await query.Where(q =>
                          q.NumeroDocumento == numeroVenta
                       ).Include(dv => dv.DetalleVenta).ThenInclude(
                           p => p.IdProductoNavigation
                       ).ToListAsync();
                }
                return _mapper.Map<List<VentaDTO>>(listaResultado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepositorio.Consultar();
            var listaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));
                
                listaResultado = await query
                    .Include(p => p.IdProductoNavigation).Include(v => v.IdVentaNavigation).Where(dv =>
                    dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_inicio.Date &&
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_fin.Date
                    ).ToListAsync();
                return _mapper.Map<List<ReporteDTO>>(listaResultado);
            }
            catch
            {
                throw;
            }
        }
    }
}
