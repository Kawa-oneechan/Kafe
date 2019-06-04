sampler s0 : register(s0);
sampler PaletteMap : register(s1)
{
	addressU = Wrap;
	addressV = Wrap;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;
};

float NumPalettes;
float NumColors;
float TargetPalette;
float4 ColorMult;
float4 ColorAdd;

float4 main(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	float4 pixel = tex2D(s0, uv);
	float colorIndex, palIndex;
	
	if (pixel.a == 0.0) return pixel;
	if (pixel.a > 0.25) return pixel;
	
	colorIndex = (pixel.r * 256) / NumColors;
	palIndex = (TargetPalette / NumPalettes);

	pixel = tex2D(PaletteMap, float2(colorIndex, palIndex));
	
	return pixel * ColorMult + ColorAdd;
}

technique Palette { pass Pass1 { PixelShader = compile ps_4_0 main(); } }
