using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using SistemaSapatosBase.Model;

namespace SistemaSapatosBase.DataBase
{
    public class VendaContexto
    {
        private Contexto contexto;
        /// <summary>
        /// Criar um novo contexto(conexão com o banco).
        /// </summary>
        public VendaContexto()
        {
            contexto = new Contexto();
        }
        /// <summary>
        /// Adiciona uma venda ao banco de dados
        /// </summary>
        /// <param name="venda">Venda a ser salva</param>
        /// <returns></returns>
        public int Salvar(Venda venda)
        {     
            contexto.Vendas.Add(venda);
            contexto.SaveChanges();
            return venda.IdVenda;
        }
        /// <summary>
        /// Busca uma venda no banco de dados
        /// </summary>
        /// <param name="id">Id da venda a ser localizada</param>
        /// <returns></returns>
        public Venda BuscarId(int id)
        {
            return contexto.Vendas.Where(x => x.IdVenda == id).SingleOrDefault();
        }
        /// <summary>
        /// Deleta uma venda do banco de dados(Apenas base não utilizado)
        /// </summary>
        /// <param name="venda">Venda a ser deletada</param>
        /// <returns></returns>
        public string Deletar(Venda venda)
        {
            string itemDeletado = venda.Modelo.Nome;

            contexto.Vendas.Attach(venda);
            contexto.Vendas.Remove(venda);
            contexto.SaveChanges();
            return itemDeletado;

        }
        /// <summary>
        /// Carrega todas as vendas cadastradas no banco de dados
        /// </summary>
        /// <returns>Lista de vendas</returns>
        public ObservableCollection<Venda> Carregar()
        {

            contexto.Vendas.Include(v => v.Cliente)
                           .Include(m => m.Modelo)
                           .ToList();
            return contexto.Vendas.Local;

        }
    }
}
