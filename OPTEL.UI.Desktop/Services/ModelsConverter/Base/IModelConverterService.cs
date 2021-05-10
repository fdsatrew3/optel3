using OPTEL.Data.Core;
using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Services.ModelsConverter.Base
{
    public interface IModelConverterService<T, T1> : IDataModel, IDataObject
    {
        public T Convert(T1 source);

        public T1 ConvertBack(T source);
    }
}
