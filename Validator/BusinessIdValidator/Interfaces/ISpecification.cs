using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIV.Interfaces
{
    /// <summary>
    /// Interface for validating given BusinessIds
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<in TEntity>
    {
        IEnumerable<string> ReasonsForDissatisfaction { get; }
        bool IsSatisfiedBy(TEntity entity);
    }
}
