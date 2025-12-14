using System.Net.Mime;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Template.Api;

/// <summary>
/// The Collections API controller.
/// </summary>
[ApiController]
[Authorize]
[Route("CollectionUtil")]
[Produces(MediaTypeNames.Application.Json)]
#pragma warning disable CA1001
public class CollectionsController : ControllerBase
#pragma warning restore CA1001
{
    private readonly CollectionUtilsManager _collectionUtilsManager;
    private readonly ILogger<CollectionUtilsManager> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionsController"/> class.
    /// </summary>
    /// <param name="libraryManager">a.</param>
    /// <param name="logger">b.</param>
    /// <param name="fileSystem">c.</param>
    public CollectionsController(ILibraryManager libraryManager, ILogger<CollectionUtilsManager> logger, IFileSystem fileSystem)
    {
        _collectionUtilsManager = new CollectionUtilsManager(libraryManager, logger, fileSystem);
        _logger = logger;
    }

    /// <summary>
    /// dsad.
    /// </summary>
    /// <returns>s.</returns>
    [HttpPost("PurgeEmptyCollections")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult PurgeEmptyCollectionsRequest()
    {
        _logger.LogInformation("Starting manual collection purging, searching for empty collections");
        _collectionUtilsManager.PurgeEmptyCollections(null!);
        _logger.LogInformation("Finished collection purging");
        return NoContent();
    }
}
