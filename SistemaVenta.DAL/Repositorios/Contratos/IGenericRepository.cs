using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;


namespace SistemaVenta.DAL.Repositorios.Contratos
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        //obtener menu, rol, cateogira, etc.
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro); 
        // Crear un menu, prodducto, etc.
        Task<TModel> Crear(TModel modelo);
        Task<bool> Editar(TModel modelo);
        Task<bool> Eliminar(TModel modelo);
        //devuelve la consulta del modelo
        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel,bool>>filtro = null);

    }
}
