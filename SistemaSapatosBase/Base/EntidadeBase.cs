using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SistemaSapatos.Base.Base
{
    /// <summary>
    /// Classe de base para as entidades e algumas viewmodels
    /// </summary>
    public class EntidadeBase : INotifyPropertyChanged,  INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // Evendo de alteração de estado

        /// <summary>
        /// Metodo que notifica se uma propriedade foi alterada
        /// </summary>
        /// <param name="propertyName"> Recebe o nome da propriedade responsavel
        /// pela chamada(CallerMemberName) ou uma string com o nome</param>
        public void Notificacao([CallerMemberName] string propertyName = null)
        {
            // Quando o metodo é chamado é disparado o evento PropertyChanged avisando a interface que ouve alteração na propriedade
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo
        // Dicionario que recebe os erros de validação vindos da entidade através das anotações(DataAnnotations)
        private readonly Dictionary<string, ICollection<string>>
       _validationErrors = new Dictionary<string, ICollection<string>>();
         
        /// <summary>
        /// Metodo que notifica caso exista erros de validação no objeto
        /// </summary>
        /// <param name="value"> O objeto a ser validado</param>
        /// <param name="propertyName"> Nome da propriedade que chamou o metodo(CallMemberName) ou o nome da propriedade
        /// que for passada via string</param>
        protected void ValidateModelProperty(object value,[CallerMemberName] string propertyName = null)
        {
            // Verifica se esse propriedade já existe no dicionario e a retira em caso positivo
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);

            // Lista para os resultados da validação
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            // Contexto onde sera feita a validação, recebe uma instancia do objeto(this, neste caso)
            ValidationContext validationContext =
                new ValidationContext(this, null, null) { MemberName = propertyName }; // Define qual propriedade do objeto sera validada passando seu nome(propertyName)
            // Tenta validar a propriedade
            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                // Caso não valide entrara aqui e entao começa a alimentar o dicionario com a chave(propertyName)  uma lista para os erros
                _validationErrors.Add(propertyName, new List<string>());
                // O metodo TryValidadeProperty alimenta a lista validationResults com os resultados e o foreach vai alimentar o dicionario com os erros vinculando eles a chave setada anteriormente
                foreach (ValidationResult validationResult in validationResults)
                {
                    // Dicionario[chave].add(Mensagem de erro do objeto validation result)
                    _validationErrors[propertyName].Add(validationResult.ErrorMessage);
                }
            }
            // Chama o metodo RaiseErrorsChanged passando a propriedade
            RaiseErrorsChanged(propertyName);
        }

        // Evento que sera atribuido abaixo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        // Recebe a propriedade e notifica a existencia de erros
        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        // Retorna os erros de validação contidos no dicionario
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }
        // Retorna true caso exista 1 ou mais erros na lista e false caso não exista
        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }
        #endregion
    }
}

