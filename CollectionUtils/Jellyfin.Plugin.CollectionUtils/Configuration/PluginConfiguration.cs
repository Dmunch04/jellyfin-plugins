using System;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Template.Configuration;

/// <summary>
/// The configuration options.
/// </summary>
public enum SomeOptions
{
    /// <summary>
    /// Option one.
    /// </summary>
    OneOption,

    /// <summary>
    /// Second option.
    /// </summary>
    AnotherOption
}

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets fuck this.
    /// </summary>
#pragma warning disable CA1819
    public string[] NamesExcluded { get; set; }
#pragma warning restore CA1819

    /// <summary>
    /// Gets or sets this shit sucks.
    /// </summary>
    public int DesiredCollectionSize { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
#pragma warning disable SA1201
    public PluginConfiguration()
#pragma warning restore SA1201
    {
        NamesExcluded = Array.Empty<string>();
        DesiredCollectionSize = 3;
    }
}
