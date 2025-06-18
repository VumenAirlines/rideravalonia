// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Styling;
using Exp_Parser.Engine;
using Exp_Parser.Model;
using ReactiveUI;
using rideravalonia.Models;
using rideravalonia.Services;
using Splat;

namespace rideravalonia.ViewModels;


public class FirstViewModel : ViewModelBase, IRoutableViewModel
{

    public ObservableCollection<FunctionInput> Inputs { get; }

    private FunctionInput _current;
    public RxCommandUnit AddItemCommand { get; }
    public IScreen HostScreen { get; }
    private string _functionString = "0";

    private readonly IExpTokenizer _tokenizer;
    private readonly IExpParser _parser;
    private readonly IPlotService? _plotService;
    public string UrlPathSegment { get; } = "first"; //Guid.NewGuid().ToString().Substring(0, 5);
    
    public string FunctionString
    {
        get => _functionString;
        set => this.RaiseAndSetIfChanged(ref _functionString, value);
    }

    public FunctionInput Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }
    public FirstViewModel(IScreen screen, IPlotService plotService)
    {
       
        HostScreen = screen;
        _current = new FunctionInput(_functionString, DateTime.Now);
        Inputs = [_current];
        _plotService = plotService;
        _tokenizer = Locator.Current.GetService<IExpTokenizer>();
        _parser = Locator.Current.GetService<IExpParser>();
        if (_plotService is null) throw new Exception();
        
        IObservable<bool> canExecute = this.WhenAnyValue(x =>x.FunctionString, funcStr=>!string.IsNullOrEmpty(funcStr));
        
        this.WhenAnyValue(x => x.Current)
            .Skip(1)
            .Where(current => !string.IsNullOrWhiteSpace(current.Input))
            .ObserveOn(RxApp.TaskpoolScheduler)
            .SelectMany(current => Observable.FromAsync(() => AddFunctionAsync(current)))
            .Subscribe(
                _ => { /* Success handling if needed */ },
                ex => Console.WriteLine($"Error processing function: {ex.Message}")
            );
        AddItemCommand = ReactiveCommand.Create(SendInput, canExecute);

    }

    public void SelectCurrent(Guid id)
    {
        FunctionInput? foundInput = Inputs.FirstOrDefault(x => x.Id == id);
        if (foundInput != null) 
            Current = foundInput.Value;
    }

    private void SendInput()
    {
        FunctionInput newInput = new FunctionInput(FunctionString, DateTime.Now);
        Inputs.Add(newInput);
        
        Current = newInput;
    }
    private async Task AddFunctionAsync(FunctionInput input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input.Input))
                return;

            TokenList tokens =await Task.Run(() =>  _tokenizer.Tokenize(input.Input) );
            
            if (await Task.Run(()=>_parser.BuildExpressionFor<double>(tokens, "x",input.Input))  is not Func<double, double> func)
                throw new InvalidOperationException("Expected Func<double, double>");
            _plotService?.UpdateFunction(func);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

    }
}

