using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.ToonShader
{
    [DataContract]
    public class ToonShaderEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("inputSampler", typeof(ToonShaderEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty LevelsProperty =
            DependencyProperty.Register("Levels", typeof(double), typeof(ToonShaderEffect), new UIPropertyMetadata(5D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double Levels
        {
            get { return ((double)(GetValue(LevelsProperty))); }
            set { SetValue(LevelsProperty, value); }
        }

        public ToonShaderEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "ToonShader/ToonShaderEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(LevelsProperty);
        }
    }
}
