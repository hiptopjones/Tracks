#version 330 core

// ins
in vec3 vs_in_pos;
in vec2 vs_in_texcoord;

// outs
out vec2 vs_out_texcoord;

// uniforms
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

// functions

// main
void main()
{
    gl_Position = projection * view * model * vec4(vs_in_pos, 1.0);
    vs_out_texcoord = vs_in_texcoord;
}