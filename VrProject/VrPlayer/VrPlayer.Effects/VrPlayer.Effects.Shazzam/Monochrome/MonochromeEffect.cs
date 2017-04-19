using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.Monochrome
{
    [DataContract]
    public class MonochromeEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
                    RegisterPixelShaderSamplerProperty("implicitInputSampler", typeof(MonochromeEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty FilterColorProperty =
            DependencyProperty.Register("filterColor", typeof(Color), typeof(MonochromeEffect), new UIPropertyMetadata(Color.FromArgb(255, 255, 255, 0), PixelShaderConstantCallback(0)));
        [DataMember]
        public Color FilterColor
        {
            get { return ((Color)(GetValue(FilterColorProperty))); }
            set { SetValue(FilterColorProperty, value); }
        }


        public MonochromeEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "Monochrome/MonochromeEffect.ps"));
            PixelShader = pixelShader;
            
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FilterColorProperty);
        }
    }
}
