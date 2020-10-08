// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;
using Web.ViewModels;

namespace Web.Controllers
{
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [Authorize]
    [SecurityHeaders]
    [Route("grants")]
    public class GrantsController : Controller
    {
        private readonly IIdentityServerInteractionService Interaction;
        private readonly IClientStore Clients;
        private readonly IResourceStore Resources;
        private readonly IEventService Events;

        public GrantsController(IIdentityServerInteractionService interaction,
            IClientStore clients,
            IResourceStore resources,
            IEventService events)
        {
            Interaction = interaction;
            Clients = clients;
            Resources = resources;
            Events = events;
        }

        /// <summary>
        /// Show list of grants
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await BuildViewModelAsync());
        }

        /// <summary>
        /// Handle postback to revoke a client
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await Interaction.RevokeUserConsentAsync(clientId);
            await Events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

            return RedirectToAction("Index");
        }

        private async Task<GrantsViewModel> BuildViewModelAsync()
        {
            var grants = await Interaction.GetAllUserGrantsAsync();

            var list = new List<GrantViewModel>();
            foreach (var grant in grants)
            {
                var client = await Clients.FindClientByIdAsync(grant.ClientId);
                if (client != null)
                {
                    var resources = await Resources.FindResourcesByScopeAsync(grant.Scopes);

                    var item = new GrantViewModel()
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Description = grant.Description,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resources.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray()
                    };

                    list.Add(item);
                }
            }

            return new GrantsViewModel
            {
                Grants = list
            };
        }
    }
}