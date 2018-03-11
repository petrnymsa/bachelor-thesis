using System.Collections.Generic;

namespace BachelorThesis.Bussiness.DataModels
{
    public interface IChildrenAware<T>
    {
        List<T> GetChildren();
    }
}