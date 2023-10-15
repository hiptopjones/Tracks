#version 330 core

// structs
struct ColorMap
{
	bool has_tex;
	sampler2D tex;
	vec4 color;
};

// ins
in vec2 vs_out_texcoord;

// outs
out vec4 fs_out_color;

// uniforms
uniform ColorMap map_diffuse;

// functions
vec4 sample_colormap(ColorMap map, vec2 uv)
{
	return map.has_tex ? texture(map.tex, uv) : map.color;
}

// main
void main()
{
	fs_out_color = sample_colormap(map_diffuse, vs_out_texcoord);
}