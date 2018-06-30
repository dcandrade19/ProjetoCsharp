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

        /// <summary>
        /// Criar um novo contexto(conexão com o banco).
        /// </summary>
        public ModeloContexto()
        {
            contexto = new Contexto();
        }

       
        /// <summary>
        /// Adiciona ou edita um modelo no banco e atualiza os estoques
        /// referentes a ele
        /// </summary>
        /// <param name="modelo">Modelo a ser salvo</param>
        /// <returns></returns>
        public int Salvar(Modelo modelo)
        { 
            int id = 0;
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
        /// <summary>
        /// Altera um modelo existente no banco
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public int Editar(Modelo modelo)
        {
            Modelo modeloEditar = BuscarId(modelo.IdModelo);
            contexto.Entry(modeloEditar).CurrentValues.SetValues(modelo);
            return modeloEditar.IdModelo;
        }
        /// <summary>
        /// Atualiza ou adiciona itens de estoque no banco
        /// </summary>
        /// <param name="estoques"></param>
        public void EditarEstoques(ICollection<Estoque> estoques)
        {
            foreach (Estoque estoque in estoques)
            {
                contexto.Entry(estoque).State = estoque.IdEstoque == 0 ? EntityState.Added : EntityState.Modified;

            }
            contexto.SaveChanges();
        }
        /// <summary>
        /// Busca um modelo no banca de dados
        /// </summary>
        /// <param name="id"> Id do modelo a ser localizado</param>
        /// <returns>Modelo encontrado ou Default</returns>
        public Modelo BuscarId(int id)
        {
            return contexto.Modelos.Include(m => m.Estoques).Where(x => x.IdModelo == id).SingleOrDefault();
        }
        /// <summary>
        /// Deleta um modelo do banco de dados
        /// </summary>
        /// <param name="modelo">Modelo a ser deletado</param>
        /// <returns>Id do modelo deletado</returns>
        public int Deletar(Modelo modelo)
        {
            try
            {
                int idDeletar = modelo.IdModelo;
                Modelo modeloDeletar = BuscarId(idDeletar);
                contexto.Modelos.Remove(modeloDeletar);
                contexto.SaveChanges();
                return idDeletar;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }        
        }

        public bool Testar()
        {
            return contexto.Database.Exists();
        }
        /// <summary>
        /// Carrega todos os modelos cadastrados em banco
        /// </summary>
        /// <returns>Lista de modelos</returns>
        public ObservableCollection<Modelo> Carregar()
        {
            contexto.Modelos
                .Include(m => m.Estoques)
                .ToList();
            return contexto.Modelos.Local;
        }

    }
}
