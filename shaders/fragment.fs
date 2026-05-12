#version 450

in vec2 texCoord;

uniform sampler2D txtr;
uniform vec4 vertexColor;

uniform int colormapMode;
uniform float blendFactor;

out vec4 fragColor;

vec3 rainbow(float t) {
    vec3 c;
    if (t < 0.25)
        c = mix(vec3(0.0, 0.0, 1.0), vec3(0.0, 1.0, 1.0), t / 0.25);
    else if (t < 0.5)
        c = mix(vec3(0.0, 1.0, 1.0), vec3(0.0, 1.0, 0.0), (t - 0.25) / 0.25);
    else if (t < 0.75)
        c = mix(vec3(0.0, 1.0, 0.0), vec3(1.0, 1.0, 0.0), (t - 0.5) / 0.25);
    else
        c = mix(vec3(1.0, 1.0, 0.0), vec3(1.0, 0.0, 0.0), (t - 0.75) / 0.25);
    return c;
}

vec3 coolWarm(float t) {
    if (t < 0.5)
        return mix(vec3(0.23, 0.30, 0.75), vec3(0.95, 0.95, 0.95), t / 0.5);
    return mix(vec3(0.95, 0.95, 0.95), vec3(0.70, 0.02, 0.15), (t - 0.5) / 0.5);
}

void main() {
    if (vertexColor.a > 0.001) {
        fragColor = vertexColor;
        return;
    }

    float scalar = texture(txtr, texCoord).r;
    vec3 gray = vec3(scalar);

    if (colormapMode == 0) {
        fragColor = vec4(gray, 1.0);
        return;
    }

    vec3 mapped;
    if (colormapMode == 1) mapped = rainbow(scalar);
    else mapped = coolWarm(scalar);

    fragColor = vec4(mix(gray, mapped, clamp(blendFactor, 0.0, 1.0)), 1.0);
}
