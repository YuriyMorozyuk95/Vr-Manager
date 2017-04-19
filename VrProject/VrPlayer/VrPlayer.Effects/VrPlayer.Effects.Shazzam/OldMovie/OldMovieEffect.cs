using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.OldMovie
{
    [DataContract]
    public class OldMovieEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty = 
            RegisterPixelShaderSamplerProperty("Input", typeof(OldMovieEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty ScratchAmountProperty = 
            DependencyProperty.Register("ScratchAmount", typeof(double), typeof(OldMovieEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public double ScratchAmount
        {
            get { return ((double)(GetValue(ScratchAmountProperty))); }
            set { SetValue(ScratchAmountProperty, value); }
        }

        public static readonly DependencyProperty NoiseAmountProperty = 
            DependencyProperty.Register("NoiseAmount", typeof(double), typeof(OldMovieEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public double NoiseAmount
        {
            get { return ((double)(GetValue(NoiseAmountProperty))); }
            set { SetValue(NoiseAmountProperty, value); }
        }
        
        public static readonly DependencyProperty RandomCoord1Property = 
            DependencyProperty.Register("RandomCoord1", typeof(Point), typeof(OldMovieEffect), new UIPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(2)));
        public Point RandomCoord1
        {
            get { return ((Point)(GetValue(RandomCoord1Property))); }
            set { SetValue(RandomCoord1Property, value); }
        }
        
        public static readonly DependencyProperty RandomCoord2Property = 
            DependencyProperty.Register("RandomCoord2", typeof(Point), typeof(OldMovieEffect), new UIPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(3)));
        public Point RandomCoord2
        {
            get { return ((Point)(GetValue(RandomCoord2Property))); }
            set { SetValue(RandomCoord2Property, value); }
        }
        
        public static readonly DependencyProperty FrameProperty = 
            DependencyProperty.Register("Frame", typeof(double), typeof(OldMovieEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
        public double Frame
        {
            get { return ((double)(GetValue(FrameProperty))); }
            set { SetValue(FrameProperty, value); }
        }

        public static readonly DependencyProperty NoiseSamplerProperty = 
            RegisterPixelShaderSamplerProperty("NoiseSampler", typeof(OldMovieEffect), 1);
        public Brush NoiseSampler
        {
            get { return ((Brush)(GetValue(NoiseSamplerProperty))); }
            set { SetValue(NoiseSamplerProperty, value); }
        }

        public OldMovieEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "OldMovie/OldMovieEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ScratchAmountProperty);
            UpdateShaderValue(NoiseAmountProperty);
            UpdateShaderValue(RandomCoord1Property);
            UpdateShaderValue(RandomCoord2Property);
            UpdateShaderValue(FrameProperty);
            UpdateShaderValue(NoiseSamplerProperty);
        }
    }
}
