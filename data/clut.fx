sampler s0 : register(s0);
sampler PALETTE : register(s1); //lookup table
float PALETTES : register(c0); //num rows
float COLORS : register(c1); //num of colors per row
float INDEX : register(c2); //target row

//float4 main(float2 uv : TEXCOORD0) : COLOR
float4 main(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	float4 pixel = tex2D(s0, uv);
	float colorIndex, palIndex;
	
	if (pixel.a == 0.0) return pixel;
	if (pixel.a > 0.25) return pixel;
	
	//We know now that we have a LUT color.
	//Red ranges from 0 to the width of the palette.
	//Convert that to a range from 0.0 to 1.0.

	colorIndex = (pixel.r * 256) / COLORS;
	palIndex = (INDEX / PALETTES);

	pixel = tex2D(PALETTE, float2(colorIndex, palIndex) + 0.02);
	
	return pixel;
}

technique Plain
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 main();
    }
}
