#pragma once
#include "SoundSource.h"

#define DllExport   __declspec( dllexport )

/*
**Method name:** "AuralizeFromContent"
**Argument:** a string to accept NAFSnaP-formatted content.
**Response:** a string with location of a generated sound file or an error description, same as the "Auralization" service response.
*/
extern "C" DllExport char* __stdcall AuralizeFromContent(char content[]);

// Auralization selected sound sources
extern "C" DllExport char* __stdcall Auralization(SoundSource sources[]);
