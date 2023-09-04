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

namespace SistemaVenta.BLL.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;
        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategoria = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategoria.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}
