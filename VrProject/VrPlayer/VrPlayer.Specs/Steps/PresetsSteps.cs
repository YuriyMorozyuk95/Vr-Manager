using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.State;
using VrPlayer.Projections.Sphere;
using VrPlayer.ViewModels;
using System.Windows.Input;

namespace VrPlayer.Specs.Steps
{
    [Binding]
    public class PresetsSteps
    {
        private Mock<IApplicationState> _stateMock = new Mock<IApplicationState>();
        private readonly Mock<IPluginManager> _pluginManagerMock = new Mock<IPluginManager>();    
        private readonly string _tempPresetFilePath = Path.GetTempPath() + "preset.json";

        [Given(@"my current projection is spherical")]
        public void GivenMyCurrentProjectionIsSpherical()
        {
            var sphereProjectionPlugin = new SpherePlugin();
            var projections = new List<IPlugin<IProjection>> {sphereProjectionPlugin};
            _stateMock.Setup(mock => mock.ProjectionPlugin).Returns(sphereProjectionPlugin);
            _pluginManagerMock.Setup(mock => mock.Projections).Returns(projections);
        }

        [Given(@"my current stereo format is side by side")]
        public void GivenMyCurrentStereoFormatIsSideBySide()
        {
            _stateMock
                .Setup(mock => mock.StereoInput)
                .Returns(StereoMode.SideBySide);
        }
        
        [Given(@"there are no effects")]
        public void GivenThereAreNoEffects()
        {
            _stateMock
                .Setup(mock => mock.EffectPlugin)
                .Returns((IPlugin<EffectBase>) null);
        }
        
        [When(@"I press save from the preset menu")]
        public void WhenIPressSaveFromThePresetMenu()
        {
            var configMock = new Mock<IApplicationConfig>();
            var presetManager = new PresetsManager(configMock.Object, _stateMock.Object, _pluginManagerMock.Object);
            var viewModel = new MenuViewModel(_stateMock.Object, _pluginManagerMock.Object, configMock.Object, presetManager);

            viewModel.SaveMediaPresetCommand.Execute(_tempPresetFilePath);
        }
        
        [Then(@"a file is created with the content of ""(.*)""")]
        public void ThenAFileIsCreatedWithTheContentOf(string filePath)
        {
            var expectedFileInfo = new FileInfo(filePath);
            var actualFileInfo = new FileInfo(_tempPresetFilePath);

            //Todo: Find why this is not working even if files are the same.
            //FileAssert.AreEqual(expectedFileInfo, actualFileInfo);

            Assert.That(actualFileInfo.OpenText().ReadToEnd(), Is.EqualTo(expectedFileInfo.OpenText().ReadToEnd()));
        }

        [When(@"I load the file ""(.*)"" using the preset menu")]
        public void WhenILoadTheFileUsingThePresetMenu(string filePath)
        {
            var sphereProjectionPlugin = new SpherePlugin();
            var projections = new List<IPlugin<IProjection>> { sphereProjectionPlugin };
            _pluginManagerMock.Setup(mock => mock.Projections).Returns(projections);
            _stateMock = new Mock<IApplicationState>();
            _stateMock.SetupAllProperties();
            var configMock = new Mock<IApplicationConfig>();
            var presetManager = new PresetsManager(configMock.Object, _stateMock.Object, _pluginManagerMock.Object);
            var viewModel = new MenuViewModel(_stateMock.Object, _pluginManagerMock.Object, configMock.Object, presetManager);

            viewModel.LoadMediaPresetCommand.Execute(filePath);
        }

        [Then(@"my new projection is spherical")]
        public void ThenMyNewProjectionIsSpherical()
        {
            Assert.That(_stateMock.Object.ProjectionPlugin, Is.TypeOf(typeof(SpherePlugin)));
        }

        [Then(@"my new stereo format is side by side")]
        public void ThenMyNewStereoFormatIsSideBySide()
        {
            Assert.That(_stateMock.Object.StereoInput, Is.EqualTo(StereoMode.SideBySide));
        }

        [Then(@"there are no more effects")]
        public void ThenThereAreNoMoreEffects()
        {
            Assert.That(_stateMock.Object.EffectPlugin, Is.Null);
        }
    }
}