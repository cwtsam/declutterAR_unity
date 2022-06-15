#ifndef OpenCVPlugin_hpp
#define OpenCVPlugin_hpp

#include <stdio.h>


__declspec(dllexport) void PixMixImage(unsigned char* bytes, int width, int height, bool isReset);
__declspec(dllexport) void ResetPixMix(bool reset);
//__declspec(dllexport) void SetBackground(unsigned char* bytes, int width, int height, bool mirror, bool rotate);
//__declspec(dllexport) void RecieveImage(unsigned char* bytes, int width, int height, bool isGreen);


unsigned char* GetCurrImage();

#endif /* OpenCVPlugin_hpp */




