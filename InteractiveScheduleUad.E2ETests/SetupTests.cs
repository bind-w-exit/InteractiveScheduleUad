using InteractiveScheduleUad.Core.Utils;

namespace InteractiveScheduleUad.E2ETests;

// setup tests verify whether API server is accessible and can be authenticated against

public class SetupTests
{
    public SetupTests()
    {
    }

    // TODO: Implement
    //[Fact]
    //public async Task ApiServerIsAccessible()
    //{
    //}

    [Fact]
    public async Task GetConfig_ReturnsTokenAndBasePath()
    {
        // Act
        var conf = await ApiConfigRetriever.GetBasePathAndAccessToken();

        // Assert
        Assert.IsType<string>(conf.BasePath);
        Assert.IsType<string>(conf.AccessToken);

        // assert that the strings aren't empty
        Assert.False(string.IsNullOrEmpty(conf.BasePath));
        Assert.False(string.IsNullOrEmpty(conf.AccessToken));
    }
}