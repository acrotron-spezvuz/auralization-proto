// NAFLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NAFLib.h"
#include <combaseapi.h>

// Auralization result file name
const char _fileName[] = "traffic.mp3";

extern "C" DllExport char* __stdcall Auralization(SoundSource sources[]) {
	
	// get size
	size_t sizeOfFileName = sizeof _fileName;

	// allocate some memory for the result
	char* result = (char*)::CoTaskMemAlloc(sizeOfFileName);

	// copy 
	strcpy_s(result, sizeOfFileName, _fileName);

	// return
	return result;
}
