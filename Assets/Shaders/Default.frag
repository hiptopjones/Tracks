#version 330 core

in vec2 vTexCoord;
out vec4 fragColor;

uniform sampler2D ourTexture;
uniform vec4 color;
uniform bool isWireframe;

void main()
{
    // color is only set when drawing as a wireframe
    if (isWireframe)
    {
        fragColor = vec4(0, 1, 0, 1);
    }
    else
    {
        fragColor = color * texture(ourTexture, vTexCoord);
    }
}