using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatos.Base.Base
{
    /// <summary>
    /// Classe para validação de listas via dataannotations
    /// </summary>
    public class ValidarLista : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var list = value as ObservableCollection<object>;
            if (list.Count > 1 && list != null)
            {
                return true;
            }
            return false;
        }
    }
}
