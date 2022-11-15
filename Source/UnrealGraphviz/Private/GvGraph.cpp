// Fill out your copyright notice in the Description page of Project Settings.

#include "GvGraph.h"
#include <graphviz/gvc.h>

namespace
{
	GVC_t* GetContext()
	{
		static GVC_t* Context = gvContext();
		return Context;
	}
}

struct FGvGraph::FImpl
{
	Agraph_t* Graph;
	TMap<FString, Agnode_t*> Nodes;
};

FGvGraph::FGvGraph(const FString& GraphName) :
	Impl(TUniquePtr<FImpl>(new FImpl))
{
	Impl->Graph = agopen(TCHAR_TO_ANSI(*GraphName), Agdirected, nullptr);
}

FGvGraph::~FGvGraph()
{
	agclose(Impl->Graph);
}

void FGvGraph::AddNode(const FString& NodeName)
{
	Agnode_t* Node = agnode(Impl->Graph, TCHAR_TO_ANSI(*NodeName), 1);
	Impl->Nodes.Add(NodeName, Node);
}

void FGvGraph::AddEdge(const FString& NodeA, const FString& NodeB, bool bCreateNodeIfMissing)
{
	auto GetNode = [=](const FString& NodeName)
	{
		TMap<FString, Agnode_t*>& Nodes = Impl->Nodes;
		Agnode_t* Node = Nodes.Contains(NodeName) ? Nodes[NodeName] : nullptr;
		if (!Node && bCreateNodeIfMissing)
		{
			Node = agnode(Impl->Graph, TCHAR_TO_ANSI(*NodeName), 1);
			Impl->Nodes.Add(NodeName, Node);
		}

		return Node;
	};

	Agnode_t* A = GetNode(NodeA);
	Agnode_t* B = GetNode(NodeB);

	if (A && B)
	{
		agedge(Impl->Graph, A, B, nullptr, 1);
	}
}

FString FGvGraph::RenderText() const
{
	GVC_t* Context = GetContext();
	Agraph_t* Graph = Impl->Graph;

	gvLayout(Context, Graph, "dot");

	char* Result;
	unsigned int Length;
	gvRenderData(Context, Graph, "dot", &Result, &Length);

	FString Output = Result;

	gvFreeRenderData(Result);

	return Output;
}