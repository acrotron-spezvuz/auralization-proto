// NAFLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NAFLib.h"
#include <combaseapi.h>

#if _DEBUG
// Auralization result file name
const char _fileNameFromSources[] = "traffic.mp3";
const char _fileNameFromContent[] = "from_content.mp3";

#else // release

const char _fileNameFromSources[] = "https://s3.us-east-2.amazonaws.com/acro.spez.sounds.sample/traffic.wav";
const char _fileNameFromContent[] = "https://s3.us-east-2.amazonaws.com/acro.spez.sounds.sample/traffic.wav";

#endif

extern "C" DllExport char* __stdcall Auralization(SoundSource sources[]) {
	
	// get size
	size_t sizeOfFileName = sizeof _fileNameFromSources;

	// allocate some memory for the result
	char* result = (char*)::CoTaskMemAlloc(sizeOfFileName);

	// copy 
	strcpy_s(result, sizeOfFileName, _fileNameFromSources);

	// return
	return result;
}

extern "C" DllExport char* __stdcall AuralizeFromContent(char content[]) {

	// get size
	size_t sizeOfFileName = sizeof _fileNameFromContent;

	// allocate some memory for the result
	char* result = (char*)::CoTaskMemAlloc(sizeOfFileName);

	// copy 
	strcpy_s(result, sizeOfFileName, _fileNameFromContent);

	// return
	return result;
}
