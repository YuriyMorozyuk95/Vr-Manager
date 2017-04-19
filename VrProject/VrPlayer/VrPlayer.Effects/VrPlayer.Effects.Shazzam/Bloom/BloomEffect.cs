using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.Bloom
{
    [DataContract]
    public class BloomEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("implicitInputSampler", typeof(BloomEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty BloomIntensityProperty =
            DependencyProperty.Register("BloomIntensity", typeof(double), typeof(BloomEffect), new UIPropertyMetadata(1D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double BloomIntensity
        {
            get { return ((double)(GetValue(BloomIntensityProperty))); }
            set { SetValue(BloomIntensityProperty, value); }
        }

        public static readonly DependencyProperty BaseIntensityProperty =
            DependencyProperty.Register("BaseIntensity", typeof(double), typeof(BloomEffect), new UIPropertyMetadata(0.5D, PixelShaderConstantCallback(1)));
        [DataMember]
        public double BaseIntensity
        {
            get { return ((double)(GetValue(BaseIntensityProperty))); }
            set { SetValue(BaseIntensityProperty, value); }
        }

        public static readonly DependencyProperty BloomSaturationProperty =
            DependencyProperty.Register("BloomSaturation", typeof(double), typeof(BloomEffect), new UIPropertyMetadata(1D, PixelShaderConstantCallback(2)));
        [DataMember]
        public double BloomSaturation
        {
            get { return ((double)(GetValue(BloomSaturationProperty))); }
            set { SetValue(BloomSaturationProperty, value); }
        }

        public static readonly DependencyProperty BaseSaturationProperty =
            DependencyProperty.Register("BaseSaturation", typeof(double), typeof(BloomEffect), new UIPropertyMetadata(0.5D, PixelShaderConstantCallback(3)));
        [DataMember]
        public double BaseSaturation
        {
            get { return ((double)(GetValue(BaseSaturationProperty))); }
            set { SetValue(BaseSaturationProperty, value); }
        }

        public BloomEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "Bloom/BloomEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BloomIntensityProperty);
            UpdateShaderValue(BaseIntensityProperty);
            UpdateShaderValue(BloomSaturationProperty);
            UpdateShaderValue(BaseSaturationProperty);
        }
    }
}
