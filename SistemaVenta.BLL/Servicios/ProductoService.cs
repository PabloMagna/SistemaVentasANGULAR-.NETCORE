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
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepositorio.Consultar();
                var listaProducto = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(queryProducto.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));
                if(productoCreado == null)
                    throw new TaskCanceledException("El producto no existe");
                return _mapper.Map<ProductoDTO>(productoCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoAEditar = await _productoRepositorio.Obtener(p => p.IdProducto == modelo.IdProducto);
                if (productoAEditar == null)
                    throw new TaskCanceledException("El producto no existe");
                productoAEditar.Nombre = productoModelo.Nombre;
                productoAEditar.IdCategoria = productoModelo.IdCategoria;
                productoAEditar.Stock = productoModelo.Stock;
                productoAEditar.Precio = productoModelo.Precio;
                productoAEditar.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(productoAEditar);
                if(!respuesta)
                    throw new TaskCanceledException("El producto no pudo ser editado");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEliminar = await  _productoRepositorio.Obtener(p => p.IdProducto == id);
                if (productoEliminar == null)
                    throw new TaskCanceledException("Producto Inexistente");
                bool respuesta = await _productoRepositorio.Eliminar(productoEliminar);
                if (!respuesta) throw new TaskCanceledException("No pudo ser eleminado el producto");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

    }
}
