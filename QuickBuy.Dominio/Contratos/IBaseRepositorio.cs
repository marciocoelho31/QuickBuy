using System;
using System.Collections.Generic;

namespace QuickBuy.Dominio.Contratos
{
    // IDisposable - a interface herda de uma outra interface
    // TEntity - desconhecido pro VS no momento mas logo depois no where mostra que será class
    public interface IBaseRepositorio<TEntity> : IDisposable where TEntity : class
    {
        void Adicionar(TEntity entity);
        TEntity ObterPorId(int id);
        IEnumerable<TEntity> ObterTodos();
        void Atualizar(TEntity entity);
        void Remover(TEntity entity);
    }
}
