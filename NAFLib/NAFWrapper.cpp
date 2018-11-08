/*******************************************************************************

1. This SOFTWARE is owned by NASA, and there shall be no further distribution or
publication of this , neither the source code, nor the executable code, nor
associated run-time applications, whether standalone or embedded, for use by any
third party without the express prior written approval of NASA LaRC.

2. This SOFTWARE, and/or any modified version thereof, shall not be published
for profit, given to another entity, or in any manner offered for sale to the
U.S. Government or any other entity. The Government shall not pay a second time
for the SOFTWARE or any modified/enhanced version thereof.

3. Should the SOFTWARE be modified or enhanced pursuant to a U.S. Government
contract, grant, or other agreement, the Government will be provided the
complete source code of that modified/enhanced version and the intellectual
property rights will be defined by the regulations governing said contract.

4. User agrees and has signed NASA Langley's Software Usage Agreement
(provided with SOFTWARE).

*******************************************************************************/
// NAFExample.cpp : A sample of auralization using NAF built-ins only
#include "stdafx.h"
#include <iostream>
#include <fstream> // For file dump, this will be buried in some wave file handler eventually
#include <memory>

#include "NAF_config.h"
#include "NAFCore/Atmospec.h"
#include "NAFCore/Environment.h"
#include "NAFCore/NAFCoreConfig.h"
#include "NAFCore/Component.h"
#include "NAFCore/GTFSeries.h"
#include "NAFCore/Path.h"
#include "NAFCore/Receiver.h"
#include "NAFCore/Source.h"
#include "NAFCore/SourceFrame.h"
#include "NAFCore/Sink.h"
#include "NAFCore/Terrain.h"
#include "NAFCore/Vec6Trajectory.h"
#include "NAFCore/TrajectoryLoader.h"
#include "NAFGTF/GTFEngine.h"
#include "NAFCore/GTF.h"
#include "NAFPath/Pathfinder.h"
#include "NAFPath/PathTraverser.h"
#include "NAFScene/SceneGenerator.h"
#include "NAFSynth/SynthEngine.h"
#include "NAFSynth/BaseSynth.h"
#include "NAFCore/PolySampleBuf.h"
#include "NAFCore/WaveReaderWriter.h"

int test()
{
	return 0;
}

int main_nofile_example() ////int argc, char* argv[])
{
	std::ofstream logfile("NAFExampleLog.txt", std::ios_base::app | std::ios_base::out);
	std::clog.rdbuf(logfile.rdbuf());

	// block size:
	unsigned int blksize = 512;
	// Set Units
	// any files that specify units (eg NetCDF Hemispheres) must be consistent, or they will ovewrite NAF units
	NAF::NAFCoreConfig::Instance()->SetUnit("foot");
	NAF::NAFCoreConfig::Instance()->SetUnit("degree");
	NAF::NAFCoreConfig::Instance()->SetUnit("seconds");
	NAF::NAFCoreConfig::Instance()->SetPref(2e-5);

	/////////////////////////////////////////////////////////////////////////////////////
	// make the scene
	// Create Environment - everything will get added to it later:
	std::shared_ptr<NAF::Environment> environment = std::make_shared<NAF::Environment>();
	// A source frame (eg, an aircraft) has 1 or more sources, a source has 1 or more components
	// make a source frame
	std::shared_ptr<NAF::SourceFrame> aircraft = std::make_shared<NAF::SourceFrame>();
	// make a source and add it to the frame
	std::shared_ptr<NAF::Source> src = std::make_shared<NAF::Source>();
	aircraft->AddChild(src);
	// make components and add them to the source
	std::shared_ptr <NAF::Component>  cmp;
	//  For these synthesis, type of component does not matter, just use base type.

	// random noise is broadband-like
	cmp = NAF::Component::CreateComponent(/*component model type*/ "base",/*component model arguments */"",
		/*synth type*/ "random", /*synth arguments*/"");
	cmp->SetLinearScale(20);
	src->AddComponent(cmp);
	// add a couple tones, one at 1k 
	cmp = NAF::Component::CreateComponent("base", "", "testtone", "freq=1000");
	cmp->SetLinearScale(10);
	src->AddComponent(cmp);
	// and one at 2k
	cmp = NAF::Component::CreateComponent("base", "", "testtone", "freq=2000");
	cmp->SetLinearScale(2.5);
	src->AddComponent(cmp);

	// Make a receiver w/1 sink to hear any sounds:
	std::shared_ptr<NAF::Receiver> receiver = std::make_shared<NAF::Receiver>();
	std::shared_ptr<NAF::Sink> sink = std::make_shared<NAF::Sink>();
	receiver->AddChild(sink);

	// load/create trajectories:
	NAF::TrajectoryLoader loader;
	loader.LoadTrajectory("traj1.csv");
	std::shared_ptr<NAF::Vec6Trajectory> traj = loader.GetVec6Trajectory();
	if (!traj) {
		std::cout << "Could not find trajectory file traj1.csv" << std::endl;
		std::cout << "Using built-in trajectory example." << std::endl;
		// 2 miles in 50 seconds @ 300 ft alt
		traj = std::make_shared<NAF::Vec6Trajectory>(NAF::vec6(-5280, 0, 300, 0, 0, 0)); // creates a trajectory with 1 point pt at time 0
		traj->AddRecord(50, NAF::vec6(5280, 0, 300, 0, 0, 0));
	}
	// source flies at cg of sourceframe if we don't give it a relative trajectory
	environment->AddTrajectory("traj1", traj);
	aircraft->SetVec6Trajectory(traj);
	// source can also have a relative trajectory, is by default stationary at 0,0,0 relative to aircraft

	// Create a stationary trajectory for the receiver
	std::shared_ptr<NAF::Vec6Trajectory> fixedpos = std::make_shared<NAF::Vec6Trajectory>(NAF::vec6(0, 0, 5.0, 0, 0, 0));
	receiver->SetVec6Trajectory(fixedpos);


	// and now, add the sourceframe and the receiver to the environment
	environment->AddReceiver(receiver);
	environment->AddSourceFrame(aircraft);
	// create and set environment atmosphere, terrain
	environment->SetAtmospec(NAF::Atmospec::CreateAtmospec("Default", ""));
	environment->SetTerrain(NAF::Terrain::CreateTerrain("SimpleGround", ""));
	environment->SetGroundRef(NAF::GroundRef::CreateGroundRef("default", ""));
	environment->SetGroundImped(NAF::GroundImped::CreateGroundImped("default", ""));
//	//	environment->SetTerrain(nullptr); // air only
//	// The scene generator will just update an environment, does not store it internally.
//	std::shared_ptr<NAF::SceneGenerator> scene = std::make_shared<NAF::SceneGenerator>();
////	// each path finder will find all paths for a single origin and destination.
////	// create a straight line path finder using the path finder abstract factory
////	std::shared_ptr<NAF::Pathfinder> pathfinder = NAF::Pathfinder::CreatePathfinder("straight", "");
////	// a path traverser takes a path and environment as inputs, and returns a GTFSeries
////	// create using the path traverser abstract factory, empty string will just default to base traverser type
////	std::shared_ptr<NAF::PathTraverser> pathTraverser = NAF::PathTraverser::CreatePathTraverser("", "");
////	//////////////////////////////////////////////////////////////////////////
////	// create waves & initialize for output
	NAF::WaveReaderWriter gtfwav;
	gtfwav.OpenForWrite("example.wav");
	gtfwav.GenerateAndPutHeader(3/*WAVE_FORMAT_IEEE_FLOAT*/, (DWORD)NAF::NAFCoreConfig::Instance()->GetSampleRate(), 1, 32);
////	/////////////////////////////////////////////////////////////////////////////////////
////	// will run from initial value of t to tend
	NAF::Timestamp t0 = 0.0;
	double tprev = -1000.0, tend = 50.0;
	// set the # significant digits to print from cout
	std::cout.setf(std::ios::fixed, std::ios::floatfield);
	std::cout.precision(1);

	std::shared_ptr<NAF::Source> s = std::static_pointer_cast<NAF::Source, NAF::MovableObject>(environment->GetSourceFrame(0)->GetChild(0));
	std::shared_ptr<NAF::Receiver> r = environment->GetReceiver(0);

	// just need a buffer pool of 1 for the source buffer. and 1 for the propagated buffer
	NAF::PolySampleBuf* pbuf = NAF::PolySampleBuf::Create(NAF::PolySampleBuf::SampleType_32f, blksize);
	std::shared_ptr<NAF::PolySampleBuf> srcbuf = std::shared_ptr<NAF::PolySampleBuf>(pbuf, NAF::PolySampleBuf::Destroy);
	// reuse pbuf, but create a new Buffer
	pbuf = NAF::PolySampleBuf::Create(NAF::PolySampleBuf::SampleType_32f, blksize);
	std::shared_ptr<NAF::PolySampleBuf> sumbuf = std::shared_ptr<NAF::PolySampleBuf>(pbuf, NAF::PolySampleBuf::Destroy);
	/////////////////////////////////////////////////////////////////////////////////////////////////
//	// create the Synth Engine and the GTF Engine
//	std::shared_ptr<NAF::SynthEngine> synthEngine = std::make_shared<NAF::SynthEngine>();
//	std::shared_ptr<NAF::GTFEngine> gtfEngine = std::make_shared<NAF::GTFEngine>(NAF::NAFCoreConfig::Instance()->GetSampleRate());
//	gtfEngine->SetDefaultBlksizSamples(blksize); // ensures that internal work buffers will be long enough to extract buffers into.
	bool hasoutput = false;
	std::vector<unsigned int> pathIDs;
	double blocktime = blksize * 1.0 / NAF::NAFCoreConfig::Instance()->GetSampleRate();

	for (double t = t0; t < tend; t += blocktime)
	{	// t feedback on command line
		if (t - tprev > .1) {
			std::cout << t << " of " << tend << "\r";
			tprev = t;
		}
////
////		scene->UpdateEnvironment(t, environment);
////		// find all paths from all sources to all receivers
////		auto paths = pathfinder->FindPaths(t, s, r, environment);
////
////		// We need to have one GTFEngine
////		// Then if a path is new, we need to GTFEngine::AddPath()
////		// If a path goes away, then we need to GTFEngine::RemovePath()
////		// If a path does not already exist, then submit will fail.
////		// Each time a path is added to the GTFEngine, it creates a GTFProcessor for that path
////		// The GTFProcessor contains a delayline for that path
////		for (auto path : paths)
////		{
////			synthEngine->Synthesize(t, path, *srcbuf);
////			auto gtfseries = pathTraverser->Traverse(path, environment);
////			int pathID = path->GetID();
////			// If path does not exist, create one explicitly in the engine
////			if (!gtfEngine->HasPath(pathID)) {
////				gtfEngine->AddPath(pathID);
////				pathIDs.push_back(pathID);
////			}
////			// submit buffer and GTF synchronously.  Future versions will allow for
////			// asynchronous submits.
////			gtfEngine->SubmitBufferAndGTFSeries(t, pathID, srcbuf, gtfseries->MakeCollapsedSeries());
////		}
////		/////
////		// process the new data if it can
////		gtfEngine->ProcessForDuration(blocktime);
////
////		srcbuf->Zero();
////		sumbuf->Zero();
////		hasoutput = false;
////		NAF::PolySampleBuf::Region rgn;
////		rgn.SetSizeAll();
////		NAF::Timestamp outtime;
////
////		for (auto pathID : pathIDs) {
////			if (gtfEngine->IsBufferAvailable(pathID, srcbuf->GetLength())) {
////				if (gtfEngine->GetBuffer(pathID, outtime, srcbuf, rgn)) {
////					hasoutput = true;
////					sumbuf->Add_I(*srcbuf);
////				}
////			}
////		}
////		/////
////		// Accumulate Buffers OR write directly to wave file
////		if (hasoutput)
////			gtfwav.PutRawDataWithoutRecalculating((BYTE*)sumbuf->VoidData(), sumbuf->GetLength() * sizeof(float));
	} // end loop over time
	std::cout << std::endl;
	/////////////////////////////////////////////////////////////////////////////////////
	// save output buffer
	// rewind to data block and write the correct size
	gtfwav.RewriteDataSize();
	gtfwav.CloseFile();
	// close logfile
	logfile.close();

	return 0;
}
