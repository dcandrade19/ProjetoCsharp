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
    //Classe de contexto(db) que salva, altera ou exclui um modelo do banco.
    public class ModeloContexto
    {
        private Contexto contexto;

        //Criar um novo contexto(conexão com o banco).
        public ModeloContexto()
        {
            contexto = new Contexto();
        }

        
        /*
         * Função que tenta salvar o modelo(sapato) no contexto(db), se id do modelo for maior que 0 ele edita o modelo
         * se menor que 0 adiciona um novo modelo no contexto, e retorna o id do sapato salvo(editado ou novo).  
         * Caso não consiga salvar/editar um modelo(sapato), retorna um erro do banco no console.
         */
        public int Salvar(Modelo modelo)
        { 
            int id;
            if(modelo.IdModelo > 0)
            {
                id = Editar(modelo);
                EditarEstoques(modelo.Estoques);
            }
            else
            {
                contexto.Modelos.Add(modelo);
                contexto.SaveChanges();
                id = contexto.Entry(modelo).Entity.IdModelo;
            }
            Modelo mod = BuscarId(id);
            mod.TotalEstoque = 0;
            return id;
        }

        public int Editar(Modelo modelo)
        {
            Modelo modeloEditar = BuscarId(modelo.IdModelo);
            contexto.Entry(modeloEditar).CurrentValues.SetValues(modelo);
            return modeloEditar.IdModelo;
        }
        public void EditarEstoques(ICollection<Estoque> estoques)
        {
            foreach (Estoque estoque in estoques)
            {
                contexto.Entry(estoque).State = estoque.IdEstoque == 0 ? EntityState.Added : EntityState.Modified;

            }
            contexto.SaveChanges();
        }
        /*
         * Busca no db atraves do id do modelo(sapato), primeiro ele tenta carregar os modelos salvo em banco
         * depois salva em uma coleção Observable e depois faz uma expressão lambda para para verificar se o id recebido
         * consta na lista carregada, caso não consiga retorna null.
         */
        public Modelo BuscarId(int id)
        {
            return contexto.Modelos.Include(m => m.Estoques).Where(x => x.IdModelo == id).SingleOrDefault();
        }
        /*
         * Deleta do db o modelo(sapato) recebido
         * 
         * 
         */
        public int Deletar(Modelo modelo)
        {
            int idDeletar = modelo.IdModelo;
            Modelo modeloDeletar = BuscarId(idDeletar);
            contexto.Modelos.Remove(modeloDeletar);
            contexto.SaveChanges();
            return idDeletar;
        }

        //função que carrega os modelos(sapatos) na grid.
        public ObservableCollection<Modelo> Carregar()
        {
            contexto.Modelos
                .Include(m => m.Estoques)
                .ToList();
            return contexto.Modelos.Local;
        }

    }
}
