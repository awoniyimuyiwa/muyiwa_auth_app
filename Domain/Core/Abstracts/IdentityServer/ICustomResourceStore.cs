using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Abstracts.IdentityServer
{
    public interface ICustomResourceStore : IResourceStore
    {
        /// <summary>
        /// Adds an api scope    
        /// </summary>
        /// <param name="apiScope">Api scope to add</param>
        /// <param name="cancellationToken"></param>
        Task Add(ApiScope apiScope, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an api resource      
        /// </summary>
        /// <param name="apiResource">Api resource to add</param>
        /// <param name="cancellationToken"></param>
        Task Add(ApiResource apiResource, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an identity resource      
        /// </summary>
        /// <param name="identityResources">Identity resource to add</param>
        /// <param name="cancellationToken"></param>
        Task Add(IdentityResource identityResource, CancellationToken cancellationToken = default);


        /// <summary>
        /// Updates an api scope   
        /// </summary>
        /// <param name="apiScope">Api scope to update</param>
        void Update(ApiScope apiScope);

        /// <summary>
        /// Updates an api resource    
        /// </summary>
        /// <param name="apiResource">Api resource to update</param>
        void Update(ApiResource apiResource);

        /// <summary>
        /// Updates an identity resource    
        /// </summary>
        /// <param name="identityResource">Identity resource to update</param>
        void Update(IdentityResource identityResource);


        /// <summary>
        /// Deletes an api scope
        /// </summary>
        /// <param name="apiScope">Api scope to delete</param>
        void Delete(ApiScope apiScope);

        /// <summary>
        /// Deletes an api resource
        /// </summary>
        /// <param name="apiResource">Api resource to delete</param>
        void Delete(ApiResource apiResource);

        /// <summary>
        /// Deletes an identity resource
        /// </summary>
        /// <param name="identityResource">Identity resource to delete</param>
        void Delete(IdentityResource identityResource);


        /// <summary>
        /// Check if api scope repository is empty
        /// </summary> 
        /// <param name="cancellationToken"></param>
        /// <returns>True if api scope repository is empty, false otherwise</returns>
        Task<bool> IsApiScopesEmpty(CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if api resource repository is empty
        /// </summary> 
        /// <param name="cancellationToken"></param>
        /// <returns>True if api resource repository is empty, false otherwise</returns>
        Task<bool> IsApiResourcesEmpty(CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if idenity resource repository is empty
        /// </summary> 
        /// <param name="cancellationToken"></param>
        /// <returns>True if api resource repository is empty, false otherwise</returns>
        Task<bool> IsIdentityResourcesEmpty(CancellationToken cancellationToken = default);
    }
}
