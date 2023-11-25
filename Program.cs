using Zipa;
using Spectre.Console;

var controller = new MainController();

try{
  // if(string.IsNullOrWhiteSpace(args[0])) controller.main();
  if(args.Length>=2){
    if(args[0]=="unzip"){
      controller.decompress(args[1]);
    }
  }
  
  else {
    AnsiConsole.Write(new Rule());
    controller.main();
  }
}catch (Exception e){

  Console.WriteLine("\n\n FUDEU");
  Console.WriteLine(e.Message);
  Console.WriteLine(e.StackTrace);
}

