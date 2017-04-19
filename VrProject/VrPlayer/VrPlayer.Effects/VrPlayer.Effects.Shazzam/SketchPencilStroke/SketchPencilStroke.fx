// =======================================================
// Created by  Ali Daneshmandi
// http://daneshmandi.spaces.live.com/
// http://vimeo.com/user2530151
// =======================================================

/// <description>A pencil stroke effect.</description>
sampler inputSampler : register(s0);

/// <summary>The brush size of the sketch effect.</summary>
/// <minValue>0.001</minValue>
/// <maxValue>0.019</maxValue>
/// <defaultValue>0.005</defaultValue>
float brushSize : register(C0);

float4 main(float2 texCoord: TEXCOORD,uniform float scale,uniform float pixelSize) : COLOR
{
    float4 color = tex2D( inputSampler, texCoord );
	float2 samples[4] = {0, -1,	-1,  0, 1, 0, 0, 1 };
	float4 complement = -4 * color;

	for (int i = 0; i < 4; i++)
	{
 		complement += tex2D(inputSampler, texCoord + (brushSize * samples[i]));
 		complement.r=complement.rgb;
		complement.g=complement.rgb;
		complement.b=complement.rgb;
		complement.a=color.a;
	}

    complement.rgb=1-complement.rgb;
	return complement;

}