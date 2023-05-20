using BBComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Abstract
{
    public interface IComboBoxDataProvider<TValue>
    {
        Task<List<SelectItem<TValue>>> GetCollectionAsync();
        Task<SelectItem<TValue>> GetItemAsync(TValue key);
    }
}
