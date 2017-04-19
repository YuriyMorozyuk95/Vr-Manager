using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.InvertColor
{
    [DataContract]
    public class InvertColorEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof (InvertColorEffect), 0);

        public Brush Input
        {
            get { return ((Brush) (GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public InvertColorEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "InvertColor/InvertColorEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
        }
    }
}
