#version 330 core

in vec3 vs_out_normal;
in vec3 vs_out_world_pos;

out vec4 frag_color;

uniform vec4 color;

uniform vec3 light_pos;
uniform vec4 light_color;

void main()
{
	float ambientStrength = 0.1;
	vec3 ambient = ambientStrength * light_color.xyz;

	vec3 normal = normalize(vs_out_normal);
	vec3 light_dir = normalize(light_pos - vs_out_world_pos);

	float diff = max(dot(normal, light_dir), 0.0);
	vec3 diffuse = diff * light_color.xyz;

	vec3 result = (ambient + diffuse) * color.xyz;
	frag_color = vec4(result, 1.0);
}