using Bunit;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.UI.Shared;
using ClearMeasure.Bootcamp.UI.Shared.Pages;
using ClearMeasure.Bootcamp.UnitTests.UI.Shared.Pages;
using Microsoft.Extensions.DependencyInjection;
using Palermo.BlazorMvc;
using Shouldly;
using TestContext = Bunit.TestContext;

namespace ClearMeasure.Bootcamp.UnitTests.UI.Shared.Pages;

[TestFixture]
public class ImpostorGameTests
{
    private TestContext CreateTestContext()
    {
        var ctx = new TestContext();
        ctx.Services.AddSingleton<IBus>(new Bus(null!));
        ctx.Services.AddSingleton<IUiBus>(new StubUiBus());
        return ctx;
    }

    [Test]
    public void ShouldDisplayGameSetupOnInitialLoad()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var setupCard = component.Find(".setup-card");
        setupCard.ShouldNotBeNull();
        
        var header = component.Find(".setup-card h3");
        header.TextContent.ShouldBe("Game Setup");
    }

    [Test]
    public void ShouldDisplayPlayerCountDropdown()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var select = component.Find("select#playerCount");
        select.ShouldNotBeNull();
        
        var options = component.FindAll("select#playerCount option");
        options.Count.ShouldBe(9); // 2 through 10 players
    }

    [Test]
    public void ShouldDisplayThemeDropdown()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var select = component.Find("select#theme");
        select.ShouldNotBeNull();
        
        var options = component.FindAll("select#theme option");
        options.Count.ShouldBe(9); // 9 themes
        
        // Verify all required themes are present
        var themeTexts = options.Select(o => o.TextContent).ToList();
        themeTexts.ShouldContain("Technology");
        themeTexts.ShouldContain("Animals");
        themeTexts.ShouldContain("People");
        themeTexts.ShouldContain("Sports");
        themeTexts.ShouldContain("Old Testament");
        themeTexts.ShouldContain("New Testament");
        themeTexts.ShouldContain("Jesus");
        themeTexts.ShouldContain("The Flood");
        themeTexts.ShouldContain("Revelation");
    }

    [Test]
    public void ShouldDisplayStartGameButton()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var button = component.Find(".start-btn");
        button.ShouldNotBeNull();
        button.TextContent.ShouldContain("Start Game");
    }

    [Test]
    public void ShouldTransitionToPlayerRevealStateWhenStartGameClicked()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var revealCard = component.Find(".reveal-card");
        revealCard.ShouldNotBeNull();
    }

    [Test]
    public void ShouldDisplayPlayerNumberDuringReveal()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var header = component.Find(".reveal-card h3");
        header.TextContent.ShouldContain("Player 1 of");
    }

    [Test]
    public void ShouldDisplayRevealWordButton()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var revealButton = component.Find(".reveal-btn");
        revealButton.ShouldNotBeNull();
        revealButton.TextContent.ShouldContain("Reveal My Word");
    }

    [Test]
    public void ShouldShowWordOrImpostorMessageWhenRevealClicked()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var revealButton = component.Find(".reveal-btn");
        revealButton.Click();

        var wordDisplay = component.Find(".word-display");
        wordDisplay.ShouldNotBeNull();
        
        // Should show either secret word or impostor message
        var hasSecretWord = component.FindAll(".secret-word").Any();
        var hasImpostorMessage = component.FindAll(".impostor-message").Any();
        (hasSecretWord || hasImpostorMessage).ShouldBeTrue();
    }

    [Test]
    public void ShouldDisplayHideWordButton()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var revealButton = component.Find(".reveal-btn");
        revealButton.Click();

        var hideButton = component.Find(".hide-btn");
        hideButton.ShouldNotBeNull();
        hideButton.TextContent.ShouldContain("Hide Word");
    }

    [Test]
    public void ShouldProgressToNextPlayerWhenHideWordClicked()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        var revealButton = component.Find(".reveal-btn");
        revealButton.Click();

        var hideButton = component.Find(".hide-btn");
        hideButton.Click();

        var header = component.Find(".reveal-card h3");
        header.TextContent.ShouldContain("Player 2 of");
    }

    [Test]
    public void ShouldTransitionToPlayingStateAfterAllPlayersReveal()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        // Default is 4 players, cycle through all
        for (int i = 0; i < 4; i++)
        {
            var revealButton = component.Find(".reveal-btn");
            revealButton.Click();

            var hideButton = component.Find(".hide-btn");
            hideButton.Click();
        }

        var playingCard = component.Find(".playing-card");
        playingCard.ShouldNotBeNull();
        
        var header = component.Find(".playing-card h3");
        header.TextContent.ShouldContain("Game in Progress");
    }

    [Test]
    public void ShouldDisplayGameInstructionsDuringPlay()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        // Cycle through all 4 players
        for (int i = 0; i < 4; i++)
        {
            var revealButton = component.Find(".reveal-btn");
            revealButton.Click();

            var hideButton = component.Find(".hide-btn");
            hideButton.Click();
        }

        var instructions = component.Find(".instructions");
        instructions.ShouldNotBeNull();
        instructions.TextContent.ShouldContain("How to Play");
    }

    [Test]
    public void ShouldDisplayResetButton()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        // Cycle through all 4 players
        for (int i = 0; i < 4; i++)
        {
            var revealButton = component.Find(".reveal-btn");
            revealButton.Click();

            var hideButton = component.Find(".hide-btn");
            hideButton.Click();
        }

        var resetButton = component.Find(".reset-btn");
        resetButton.ShouldNotBeNull();
        resetButton.TextContent.ShouldContain("Reset Game");
    }

    [Test]
    public void ShouldReturnToSetupWhenResetClicked()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        // Cycle through all 4 players
        for (int i = 0; i < 4; i++)
        {
            var revealButton = component.Find(".reveal-btn");
            revealButton.Click();

            var hideButton = component.Find(".hide-btn");
            hideButton.Click();
        }

        var resetButton = component.Find(".reset-btn");
        resetButton.Click();

        var setupCard = component.Find(".setup-card");
        setupCard.ShouldNotBeNull();
    }

    [Test]
    public void ShouldDisplayPlayerCountAndThemeDuringPlay()
    {
        using var ctx = CreateTestContext();
        var component = ctx.RenderComponent<ImpostorGame>();

        var startButton = component.Find(".start-btn");
        startButton.Click();

        // Cycle through all 4 players
        for (int i = 0; i < 4; i++)
        {
            var revealButton = component.Find(".reveal-btn");
            revealButton.Click();

            var hideButton = component.Find(".hide-btn");
            hideButton.Click();
        }

        var gameInfo = component.Find(".game-info");
        gameInfo.ShouldNotBeNull();
        gameInfo.TextContent.ShouldContain("Players:");
        gameInfo.TextContent.ShouldContain("Theme:");
    }
}
