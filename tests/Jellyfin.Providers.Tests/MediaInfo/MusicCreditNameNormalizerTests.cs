using MediaBrowser.Providers.MediaInfo;
using Xunit;

namespace Jellyfin.Providers.Tests.MediaInfo;

public class MusicCreditNameNormalizerTests
{
    [Fact]
    public void RemovesTrailingCreditRoles()
    {
        Assert.Equal(
            "Christina Aguilera",
            MusicCreditNameNormalizer.Normalize("Christina Aguilera, Associated Performer, Composer, Lyricist"));
    }

    [Theory]
    [InlineData("Earth, Wind & Fire")]
    [InlineData("Simon & Garfunkel, Paul Simon")]
    [InlineData("The Comma, Composer's Daughter")]
    public void PreservesCommaContainingArtistNames(string value)
    {
        Assert.Equal(value, MusicCreditNameNormalizer.Normalize(value));
    }

    [Fact]
    public void LeavesAudiobookCreditsUntouched()
    {
        const string value = "Author, Composer, Lyricist";

        Assert.Equal([value], MusicCreditNameNormalizer.NormalizeForAudio([value], isAudioBook: true));
    }

    [Fact]
    public void NormalizesMusicCreditsBeforePeopleAreCreated()
    {
        var values = MusicCreditNameNormalizer.NormalizeForAudio(
            ["Christina Aguilera, Associated Performer, Composer, Lyricist", "Earth, Wind & Fire"],
            isAudioBook: false);

        Assert.Equal(["Christina Aguilera", "Earth, Wind & Fire"], values);
    }
}
