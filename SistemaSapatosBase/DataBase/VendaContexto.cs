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

        public VendaContexto()
        {
            contexto = new Contexto();
        }

        public int Salvar(Venda venda)
        {     
            contexto.Vendas.Add(venda);
            contexto.SaveChanges();
            return venda.IdVenda;
        }

        public Venda BuscarId(int id)
        {
            return contexto.Vendas.Where(x => x.IdVenda == id).SingleOrDefault();
        }
        //Apenas como base, não é utilizado
        public string Deletar(Venda venda)
        {
            string itemDeletado = venda.Modelo.Nome;

            contexto.Vendas.Attach(venda);
            contexto.Vendas.Remove(venda);
            contexto.SaveChanges();
            return itemDeletado;

        }

        public ObservableCollection<Venda> Carregar()
        {

            contexto.Vendas.Include(v => v.Cliente)
                           .Include(m => m.Modelo)
                           .ToList();
            return contexto.Vendas.Local;

        }
    }
}
