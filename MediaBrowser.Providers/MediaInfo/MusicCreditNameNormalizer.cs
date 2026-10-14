using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaBrowser.Providers.MediaInfo;

/// <summary>
/// Removes role annotations that some taggers append to a music artist name.
/// </summary>
public static class MusicCreditNameNormalizer
{
    private static readonly HashSet<string> KnownRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "Associated Performer",
        "Arranger",
        "Composer",
        "Conductor",
        "Engineer",
        "Featured Artist",
        "Lyricist",
        "Primary Artist",
        "Performer",
        "Producer",
        "Remixer",
        "Vocalist",
        "Writer"
    };

    /// <summary>
    /// Removes a trailing sequence of known credit roles from a single artist value.
    /// Comma-separated artist names remain unchanged unless every trailing component
    /// is a recognized role.
    /// </summary>
    /// <param name="value">The raw artist value.</param>
    /// <returns>The normalized artist value.</returns>
    public static string Normalize(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var parts = value.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length < 2 || parts[0].Length == 0 || !parts[1..].All(KnownRoles.Contains))
        {
            return value.Trim();
        }

        return parts[0];
    }

    /// <summary>
    /// Normalizes music artist values while leaving audiobook credits untouched.
    /// </summary>
    /// <param name="values">The raw artist values.</param>
    /// <param name="isAudioBook">Whether the values belong to an audiobook.</param>
    /// <returns>The values used to build music people entries.</returns>
    public static string[] NormalizeForAudio(IEnumerable<string> values, bool isAudioBook)
    {
        ArgumentNullException.ThrowIfNull(values);

        return isAudioBook
            ? values.ToArray()
            : values.Select(Normalize).ToArray();
    }
}
