#pragma once
#include "SoundSource.h"

#define DllExport   __declspec( dllexport )

// Auralization selected sound sources
extern "C" DllExport char* __stdcall Auralization(SoundSource sources[]);