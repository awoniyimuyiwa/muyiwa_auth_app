using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Abstracts.IdentityServer
{
    public interface ICustomClientStore : IClientStore
    {
        /// <summary>
        /// Adds a client       
        /// </summary>
        /// <param name="client">New client to add</param>
        /// <param name="cancellationToken"></param>
        Task Add(Client client, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a client    
        /// </summary>
        /// <param name="client">Client to update</param>
        void Update(Client client);

        /// <summary>
        /// Deletes a client
        /// </summary>
        /// <param name="client">Cleint to delete</param>
        void Delete(Client client);

        /// <summary>
        /// Check if repository is empty
        /// </summary> 
        /// <param name="cancellationToken"></param>
        /// <returns>True if repository is empty, false otherwise</returns>
        Task<bool> IsEmpty(CancellationToken cancellationToken = default);
    }
}
