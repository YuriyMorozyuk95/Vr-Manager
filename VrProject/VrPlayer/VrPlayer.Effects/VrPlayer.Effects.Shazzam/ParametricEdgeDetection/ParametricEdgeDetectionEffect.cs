using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.ParametricEdgeDetection
{
    [DataContract]
    public class ParametricEdgeDetectionEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("inputSampler", typeof(ParametricEdgeDetectionEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty ThreshholdProperty =
            DependencyProperty.Register("Threshhold", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(0.5D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double Threshhold
        {
            get { return ((double)(GetValue(ThreshholdProperty))); }
            set { SetValue(ThreshholdProperty, value); }
        }

        public static readonly DependencyProperty K00Property =
            DependencyProperty.Register("K00", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(1D, PixelShaderConstantCallback(1)));
        [DataMember]
        public double K00
        {
            get { return ((double)(GetValue(K00Property))); }
            set { SetValue(K00Property, value); }
        }

        public static readonly DependencyProperty K01Property =
            DependencyProperty.Register("K01", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(2D, PixelShaderConstantCallback(2)));
        [DataMember]
        public double K01
        {
            get { return ((double)(GetValue(K01Property))); }
            set { SetValue(K01Property, value); }
        }

        public static readonly DependencyProperty K02Property =
            DependencyProperty.Register("K02", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(1D, PixelShaderConstantCallback(3)));
        [DataMember]
        public double K02
        {
            get { return ((double)(GetValue(K02Property))); }
            set { SetValue(K02Property, value); }
        }

        public static readonly DependencyProperty TextureSizeProperty = 
            DependencyProperty.Register("TextureSize", typeof(Point), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(new Point(512D, 512D), PixelShaderConstantCallback(4)));
        public Point TextureSize
        {
            get { return ((Point)(GetValue(TextureSizeProperty))); }
            set { SetValue(TextureSizeProperty, value); }
        }

        public ParametricEdgeDetectionEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "ParametricEdgeDetection/ParametricEdgeDetectionEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ThreshholdProperty);
            UpdateShaderValue(K00Property);
            UpdateShaderValue(K01Property);
            UpdateShaderValue(K02Property);
            UpdateShaderValue(TextureSizeProperty);
        }
    }
}
