// Based on Doom 3 BFG Edition GPL Source Code: Copyright (C) 1993-2012 id Software LLC, a ZeniMax Media company. 

sampler2D input : register(s0);

float factor: register(C0); 
float xCenter: register(C1); 
float yCenter: register(C2);
float blueOffset: register(C3);
float redOffset: register(C4);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	const float2 warpCenter = float2( xCenter, yCenter );
  
    float2 centeredTexcoord = uv - warpCenter;

	float2	warped = normalize( centeredTexcoord );

	// get it down into the 0 - PI/2 range

  // If radial length was 0.5, we want rescaled to also come out
  // as 0.5, so the edges of the rendered image are at the edges
  // of the warped image.
	float	rescaled = tan( length( centeredTexcoord ) * factor ) / tan( 0.5 * factor );

	warped *= 0.5 * rescaled;
	warped += warpCenter;

	float4 result = tex2D( input, warped );
	float2 sampleLoc = (warped - warpCenter) * blueOffset + warped;
	result.b = tex2D(input,sampleLoc).b;
	sampleLoc = (warped - warpCenter) * redOffset + warped;
	result.r = tex2D(input,sampleLoc).r;
    return result;
}
