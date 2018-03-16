using System.Collections.Generic;

namespace BachelorThesis.Business.DataModels
{
    public interface IChildrenAware<T>
    {
        List<T> GetChildren();
    }
}