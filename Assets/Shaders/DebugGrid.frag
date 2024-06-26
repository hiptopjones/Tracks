﻿#version 450 core

// Adapted from here:
//  * https://asliceofrendering.com/scene%20helper/2020/01/05/InfiniteGrid/
// Additional help with depth issues from here:
//  * https://stackoverflow.com/questions/72855848/depth-issue-with-infinite-grid-in-opengl-3-3-with-glsl
//  * https://stackoverflow.com/questions/72791713/issues-with-infinite-grid-in-opengl-4-5-with-glsl

uniform float near;
uniform float far;

in vec3 nearPoint;
in vec3 farPoint;
in mat4 fragModel;
in mat4 fragView;
in mat4 fragProjection;

out vec4 outColor;

vec4 grid(vec3 fragPos3D, float scale, bool drawAxis) {
    vec2 coord = fragPos3D.xz * scale;
    vec2 derivative = fwidth(coord);
    vec2 grid = abs(fract(coord - 0.5) - 0.5) / derivative;
    float line = min(grid.x, grid.y);
    float minimumz = min(derivative.y, 1);
    float minimumx = min(derivative.x, 1);
    vec4 color = vec4(0.2, 0.2, 0.2, 1.0 - min(line, 1.0));
    // z axis
    if(fragPos3D.x > -0.1 * minimumx && fragPos3D.x < 0.1 * minimumx)
        color.z = 1.0;
    // x axis
    if(fragPos3D.z > -0.1 * minimumz && fragPos3D.z < 0.1 * minimumz)
        color.x = 1.0;
    return color;
}

float computeDepth(vec3 pos) {
    vec4 clip_space_pos = fragProjection * fragView * fragModel * vec4(pos, 1.0);
    float ndc_depth = clip_space_pos.z / clip_space_pos.w;
    return ((gl_DepthRange.diff * ndc_depth) + gl_DepthRange.near + gl_DepthRange.far) / 2.0;
}

float computeLinearDepth(vec3 pos) {
    vec4 clip_space_pos = fragProjection * fragView * fragModel * vec4(pos.xyz, 1.0);
    float clip_space_depth = (clip_space_pos.z / clip_space_pos.w) * 2.0 - 1.0; // put back between -1 and 1
    float linearDepth = (2.0 * near * far) / (far + near - clip_space_depth * (far - near)); // get linear value between 0.01 and 100
    return linearDepth / far; // normalize
}

void main() {
    float t = -nearPoint.y / (farPoint.y - nearPoint.y);
    if (t > 0)
    {
        vec3 fragPos3D = nearPoint + t * (farPoint - nearPoint);

        gl_FragDepth = computeDepth(fragPos3D);

        float linearDepth = computeLinearDepth(fragPos3D);
        float fading = max(0, (0.5 - linearDepth));

        outColor = (grid(fragPos3D, 1, true) + grid(fragPos3D, 10, true)); // adding multiple resolution for the grid
        outColor.a *= fading;
    }
    else
    {
        discard;
    }
}