using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Template;

/// <summary>
/// f.
/// </summary>
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "fucking styleguides")]
public class CollectionUtilsManager : IDisposable
{
    private readonly ILibraryManager _libraryManager;
    private readonly Timer _timer;
    private readonly ILogger<CollectionUtilsManager> _logger;
    private readonly SessionInfo? _session;
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionUtilsManager"/> class.
    /// </summary>
    /// <param name="libraryManager">a.</param>
    /// <param name="logger">b.</param>
    /// <param name="fileSystem">c.</param>
    public CollectionUtilsManager(ILibraryManager libraryManager, ILogger<CollectionUtilsManager> logger, IFileSystem fileSystem)
    {
        _libraryManager = libraryManager;
        _logger = logger;
        _fileSystem = fileSystem;
        _session = null;
        _timer = new Timer(_ => OnTimerElapsed(), null, Timeout.Infinite, Timeout.Infinite);
    }

    /// <summary>
    /// d.
    /// </summary>
    /// <param name="progress">a.</param>
    public void PurgeEmptyCollections(IProgress<double> progress)
    {
        _logger.LogInformation("Searching for empty collections");

        var emptyCollections = GetBoxSetsFromLibrary().Where(x => x.LinkedChildren.Length < Plugin.Instance!.Configuration.DesiredCollectionSize).ToList();

        foreach (var collection in emptyCollections)
        {
            _logger.LogInformation("Found empty collection {Name}, with movies:", collection.Name);
            foreach (var child in collection.LinkedChildren)
            {
                _logger.LogInformation("  - {Child}", child.Id);
            }
        }

        _logger.LogInformation("Found {Count} empty collections", emptyCollections.Count);
        progress?.Report(100);
    }

    private List<BoxSet> GetBoxSetsFromLibrary()
    {
        return _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = [BaseItemKind.BoxSet]
            })
            .Select(s => s as BoxSet)
            .Where(IsElegible!)
            .ToList()!;
    }

    private List<CollectionFolder> GetCollectionsFromLibrary()
    {
        return _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = [BaseItemKind.CollectionFolder]
            })
            .Select(s => s as CollectionFolder)
            // .Where(IsElegible!)
            .ToList()!;
    }

    private bool IsElegible(BaseItem item)
    {
        if (Plugin.Instance!.Configuration.NamesExcluded != null && Plugin.Instance.Configuration.NamesExcluded.Contains(item.Name))
        {
            return false;
        }

        return true;
    }

    private void OnTimerElapsed()
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// b.
    /// </summary>
    /// <param name="disposing">a.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer?.Dispose();
            _session?.Dispose();
        }
    }
}
