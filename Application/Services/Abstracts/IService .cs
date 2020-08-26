using Domain.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Abstracts
{
    public interface IService<T, R> where T : BaseDto where R : IEntity<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<R> Create(T dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Update(
            R entity,
            T dto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        Task Delete(R entity, CancellationToken cancellationToken = default);
    }
}
