﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using RepoUtils;

/**
 * The following example shows how to use Semantic Kernel with Text Completion as streaming
 */
// ReSharper disable once InconsistentNaming
public static class Example32_StreamingCompletion
{
    public static async Task RunAsync()
    {
        await AzureOpenAITextCompletionStreamAsync();
        await OpenAITextCompletionStreamAsync();
    }

    private static async Task AzureOpenAITextCompletionStreamAsync()
    {
        Console.WriteLine("======== Azure OpenAI - Text Completion - Raw Streaming ========");

        IKernel kernel = new KernelBuilder().WithLogger(ConsoleLogger.Log).Build();
        kernel.Config.AddAzureTextCompletionService(
            Env.Var("AZURE_OPENAI_DEPLOYMENT_NAME"),
            Env.Var("AZURE_OPENAI_ENDPOINT"),
            Env.Var("AZURE_OPENAI_KEY"));

        ITextCompletion textCompletion = kernel.GetService<ITextCompletion>();

        await TextCompletionStreamAsync(textCompletion);
    }

    private static async Task OpenAITextCompletionStreamAsync()
    {
        Console.WriteLine("======== Open AI - Text Completion - Raw Streaming ========");

        IKernel kernel = new KernelBuilder().WithLogger(ConsoleLogger.Log).Build();
        kernel.Config.AddOpenAITextCompletionService("text-davinci-003", Env.Var("OPENAI_API_KEY"), serviceId: "text-davinci-003");

        ITextCompletion textCompletion = kernel.GetService<ITextCompletion>();

        await TextCompletionStreamAsync(textCompletion);
    }

    private static async Task TextCompletionStreamAsync(ITextCompletion textCompletion)
    {
        var requestSettings = new CompleteRequestSettings()
        {
            MaxTokens = 100,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Temperature = 1,
            TopP = 0.5
        };

        var prompt = "Write one paragraph why AI is awesome";

        Console.WriteLine("Prompt: " + prompt);
        await foreach (string message in textCompletion.CompleteStreamAsync(prompt, requestSettings))
        {
            Console.Write(message);
        }

        Console.WriteLine();
    }
}
