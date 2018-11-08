// NAFLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NAFLib.h"
#include "NAFWrapper.h"
#include <combaseapi.h>

#if _DEBUG
// Auralization result file name
const char _fileName[] = "traffic.mp3";

#else // release

const char _fileName[] = "https://s3.us-east-2.amazonaws.com/acro.spez.sounds.sample/traffic.wav";

#endif

extern "C" DllExport char* __stdcall Auralization(SoundSource sources[]) {
	
	test();

	// get size
	size_t sizeOfFileName = sizeof _fileName;

	// allocate some memory for the result
	char* result = (char*)::CoTaskMemAlloc(sizeOfFileName);

	// copy 
	strcpy_s(result, sizeOfFileName, _fileName);

	// return
	return result;
}
