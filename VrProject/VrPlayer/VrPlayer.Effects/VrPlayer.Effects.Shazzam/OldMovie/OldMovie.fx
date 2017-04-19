/// <description>Pixel shader which produces random scratches, noise and other FX like an old projector</description>
///
//   Contributed by Rene Schulte
//   Copyright (c) 2009 Rene Schulte

/// <summary>The amount of scratches.</summary>
/// <minValue>0</minValue>
/// <maxValue>0.1</maxValue>
/// <defaultValue>0.0044/defaultValue>
float ScratchAmount : register(C0);

/// <summary>The amount of noise.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.000001/defaultValue>
float NoiseAmount : register(C1);

/// <summary>The random coordinate 1 that is used for lookup in the noise texture.</summary>
float2 RandomCoord1 : register(C2);

/// <summary>The random coordinate 2 that is used for lookup in the noise texture.</summary>
float2 RandomCoord2 : register(C3);

/// <summary>The current frame.</summary>
float Frame : register(C4);

// Static
static float ScratchAmountInv = 1.0 / ScratchAmount;

// Sampler
sampler2D inputSampler : register(S0);
sampler2D noiseSampler : register(S1);


float4 main(float2 uv : TEXCOORD) : COLOR
{
    // Sample texture
    float4 color = tex2D(inputSampler, uv);

    // Add Scratch
    float2 sc = Frame * float2(0.001f, 0.4f);
    sc.x = frac(uv.x + sc.x);
    float scratch = tex2D(noiseSampler, sc).r;
    scratch = 2 * scratch * ScratchAmountInv;
    scratch = 1 - abs(1 - scratch);
    scratch = max(0, scratch);
    color.rgb += scratch.rrr;

    // Calculate random coord + sample
    float2 rCoord = (uv + RandomCoord1 + RandomCoord2) * 0.33;
    float3 rand = tex2D(noiseSampler, rCoord);
    // Add noise
    if(NoiseAmount > rand.r)
    {
        color.rgb = 0.1 + rand.b * 0.4;
    }

    // Convert to gray + desaturated Sepia
    float gray = dot(color, float4(0.3, 0.59, 0.11, 0));
    color = float4(gray * float3(0.9, 0.8, 0.6) , 1); // Sepia original (0.9, 0.7, 0.3)

    // Calc distance to center
    float2 dist = 0.5 - uv;
    // Random light fluctuation
    float fluc = RandomCoord2.x * 0.04 - 0.02;
    // Vignette effect
    color.rgb *= (0.4 + fluc - dot(dist, dist))  * 2.8;

    return color;
}