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
using System.Runtime.InteropServices;

namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar( u => 
                u.Correo == correo
                && u.Clave == clave);
                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("Usuario no existe");

                Usuario devolverUsuario = queryUsuario.Include(rol =>rol.IdRolNavigation).First();

                return _mapper.Map <SesionDTO>(devolverUsuario);
            }
            catch
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("No pudo ser creado el usuario");
                var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado.IdUsuario == 0)
                    throw new TaskCanceledException("No se encontró usuario");

                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
                usuarioModelo.FechaRegistro = usuarioModelo.FechaRegistro;
                usuarioModelo.IdRolNavigation = usuarioModelo.IdRolNavigation;

                bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);
                if(!respuesta)
                    throw new TaskCanceledException("No se pudo editar");

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
                var usuarioElimianr = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
                if(usuarioElimianr == null)
                    throw new TaskCanceledException("No se encuentra usuario");
                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioElimianr);
                if (!respuesta) throw new TaskCanceledException("No se pudo eliminar");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

       
    }
}
