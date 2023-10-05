using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region ROL
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion
            #region MENU
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion
            #region USUARIO
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>
                 destino.RolDescripcion, op =>
                 op.MapFrom(origen => origen.IdRolNavigation.Nombre))
                .ForMember(destino =>
                destino.EsActivo, op =>
                op.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                 destino.IdRolNavigation, op =>
                 op.Ignore()) // Usuario no necesita datos de otra tabla (descrip)
                .ForMember(destino =>
                destino.EsActivo, op =>
                op.MapFrom(origen => origen.EsActivo == 1 ? true : false));

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino => destino.RolDescripcion, op => op.MapFrom(origen => origen.IdRolNavigation.Nombre));

            #endregion
            #region CATEGORIA
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion
            #region Producto
            CreateMap<Producto, ProductoDTO>().
                ForMember(destino =>
                destino.DescripcionCategoria, op =>
                op.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)).
                ForMember(destino =>
                destino.Precio, op =>
                op.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))))
                .ForMember(destino =>
                destino.EsActivo, op =>
                op.MapFrom(origen => origen.EsActivo == true ? 1 : 0));
            CreateMap<ProductoDTO, Producto>().
                ForMember(destino =>
                destino.IdCategoriaNavigation, op =>
                op.Ignore()).
                ForMember(destino =>
                destino.Precio, op =>
                op.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-AR"))))
                .ForMember(destino =>
                destino.EsActivo, op =>
                op.MapFrom(origen => origen.EsActivo == 1 ? true : false));
            #endregion
            #region VENTA
            CreateMap<Venta, VentaDTO>().
                ForMember(destino =>
                destino.TotalTexto, op =>
                op.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))))
                .ForMember(destino =>
                destino.FechaRegistro, op =>
                op.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy")));
            CreateMap<VentaDTO, Venta>().
                ForMember(destino =>
                destino.Total, op =>
                op.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR"))));

            #endregion
            #region DETALLEVENTA
            CreateMap<DetalleVenta, DetalleVentaDTO>().
                ForMember(destino =>
                destino.DescripcionProducto, op =>
                op.MapFrom(origen => origen.IdProductoNavigation.Nombre)).
                ForMember(destino =>
                destino.PrecioTexto, op =>
                op.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR")))).
                ForMember(destino =>
                destino.TotalTexto, op =>
                op.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))));
            CreateMap<DetalleVentaDTO, DetalleVenta>().
                ForMember(destino =>
                destino.Precio, op =>
                op.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-AR")))).
                ForMember(destino =>
 
                destino.Total, op =>
                op.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR"))));
            #endregion
            #region REPORTE
            CreateMap<DetalleVenta, ReporteDTO>()
                .ForMember(destino =>
                destino.FechaRegistro, op =>
                op.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
                .ForMember(destino =>
                destino.NumeroDocumento, op =>
                op.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento))
                .ForMember(destino =>
                destino.TipoPago, op =>
                op.MapFrom(origen => origen.IdVentaNavigation.TipoPago))
                .ForMember(destino =>
                destino.TotalVenta, op =>
                op.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-AR"))))
                .ForMember(destino => destino.Producto,
                opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
                .ForMember(destino => destino.PrecioTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))))
                .ForMember(destino => destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))));
            #endregion
        }
    }
}
