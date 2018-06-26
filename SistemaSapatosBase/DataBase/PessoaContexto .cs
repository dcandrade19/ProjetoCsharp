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
    public class PessoaContexto
    {
        private Contexto contexto;

        public PessoaContexto()
        {
            contexto = new Contexto();
        }

        public int Salvar(Pessoa pessoa)
        {

            int id;
            if (pessoa.IdPessoa > 0)
            {
                id = Editar(pessoa);
            }
            else
            {
                contexto.Clientes.Add(pessoa);
                contexto.SaveChanges();
                id = contexto.Entry(pessoa).Entity.IdPessoa;
            }
            Pessoa pes = BuscarId(id);

            return id;
        }

        public int Editar(Pessoa pessoa)
        {
            Pessoa pessoaEditar = BuscarId(pessoa.IdPessoa);
            contexto.Entry(pessoaEditar).CurrentValues.SetValues(pessoa);
            foreach (Venda venda in pessoa.Compras)
            {
                contexto.Entry(venda).State = venda.IdVenda == 0 ? EntityState.Added : EntityState.Modified;

            }
            contexto.SaveChanges();
            return pessoaEditar.IdPessoa;
        }

        public Pessoa BuscarId(int id)
        {
            return contexto.Clientes.Include(c => c.Compras).Where(x => x.IdPessoa == id).SingleOrDefault();
        }

        public int Deletar(Pessoa pessoa)
        {
            int idDeletar = pessoa.IdPessoa;
            Pessoa pessoaDeletar = BuscarId(idDeletar);
            contexto.Clientes.Remove(pessoaDeletar);
            contexto.SaveChanges();
            return idDeletar;
        }

        public ObservableCollection<Pessoa> Carregar()
        {
            contexto.Clientes
                .Include(m => m.Compras)
                .ToList();
            return contexto.Clientes.Local;
        }
    }
}
