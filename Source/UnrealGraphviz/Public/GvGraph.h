// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"

class UNREALGRAPHVIZ_API FGvGraph
{
public:
	explicit FGvGraph(const FString& GraphName);
	~FGvGraph();

	void AddNode(const FString& NodeName);
	void AddEdge(const FString& NodeA, const FString& NodeB, bool bCreateNodeIfMissing = false);

	FString RenderText() const;
	// void RenderImage();

private:
	struct FImpl;
	TUniquePtr<FImpl> Impl;
};