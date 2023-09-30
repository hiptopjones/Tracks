#version 330 core
in vec3 vColor;
in vec2 vTexCoord;
out vec4 fragColor;

uniform sampler2D ourTexture;

void main()
{
    fragColor = texture(ourTexture, vTexCoord) * vec4(vColor,1.0);
}