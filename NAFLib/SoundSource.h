#pragma once
#include "SourceTypes.h"

// Sound source struct
typedef struct {
public:	
	SourceTypes source;
	double height;
	double lat;
	double lon;
	double Vx;
	double Vy;
	double Vz;	
} SoundSource;