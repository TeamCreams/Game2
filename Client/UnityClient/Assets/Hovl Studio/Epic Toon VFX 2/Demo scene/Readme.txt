Asset Creator - Vladyslav Horobets (Hovl).
-----------------------------------------------------

If you want to use post-effects like in the demo video:
https://youtu.be/hZSZ2Q8MF3k

2) Shaders
2.1)The "Use depth" on the material from the custom shaders is the Soft Particle Factor.
2.2)Use "Center glow"[MaterialToggle] only with particle system. This option is used to darken the main texture with a white texture (white is visible, black is invisible).
    If you turn on this feature, you need to use "Custom vertex stream" (Uv0.Custom.xy) in tab "Render". And don't forget to use "Custom data" parameters in your PS.
2.3)The distortion shader only works with standard rendering. Delete (if exist) distortion particles from effects if you use LWRP or HDRP!
2.4)You can change the cutoff in all shaders (except Add_CenterGlow and Blend_CenterGlow ) using (Uv0.Custom.xy) in particle system.

BiRP, URP or HDRP support is here --> Tools > RP changer for Hovl Studio Assets

Contact me if you have any questions.
My email: hovlstudio1@gmail.com